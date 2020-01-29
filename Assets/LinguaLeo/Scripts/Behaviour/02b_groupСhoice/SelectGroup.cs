using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LinguaLeo.Scripts.Helpers;
using LinguaLeo.Scripts.Helpers.Interfaces;
using LinguaLeo.Scripts.Manegers;
using UnityEngine;
using UnityEngine.UI;

namespace LinguaLeo.Scripts.Behaviour._02b_groupСhoice
{
    /// <summary>
    /// Выбор группы слов для изучения.
    /// </summary>
    public class SelectGroup : MonoBehaviour, IObserver
    {
        private const float PanelHeight = 500;

        /// <summary>
        /// Родительский объект в который будут складываться карточки группы.
        /// </summary>
        [SerializeField] private GameObject content = null;

        /// <summary>
        /// Образец карточки группы.
        /// </summary>
        [SerializeField] private WordSetPanel panelPrefab = null;

        /// <summary>
        /// Список всех UI панелей групп слов.
        /// </summary>
        private WordSetPanel[] wordTiles;

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Start()
        {
            GameManager.Notifications.AddListener(this, GAME_EVENTS.LoadedVocabulary);
        }

        /// <summary>
        /// Получает уведомления о игровых событиях.
        /// </summary>
        /// <param name="parametr">Параметры</param>
        /// <param name="notificationName">Тип событя</param>
        void IObserver.OnNotify(object parametr, GAME_EVENTS notificationName)
        {
            switch (notificationName)
            {
                case GAME_EVENTS.LoadedVocabulary:
                    Debug.Log("Start Size");
                    StartCoroutine(CreatePanels());
                    break;
            }
        }

        /// <summary>
        /// Генерирует панели групп слов.
        /// Ооочень медленно.
        /// </summary>
        /// <returns>необходимо для корутины</returns>
        private IEnumerator CreatePanels()
        {
            List<WordGroup> group = GameManager.WordManeger.GetGroupNames();
            UpdateContentHeight(group.Count);
            foreach (var item in group)
            {
                string path = "Data/Covers" + "/" + item.pictureName + ".png";

                Sprite sprite = Utilities.LoadSpriteFromFile(path);
                CreateCard(sprite, item.name, item.wordCount);
                yield return null;
            }
        }

        /// <summary>
        /// Создает отдельную панель для описания группы слов.
        /// </summary>
        /// <param name="sprite">Обложка</param>
        /// <param name="caption">Название группы</param>
        /// <param name="count">Количество слов в группе</param>
        private void CreateCard(Sprite sprite, string caption, int count)
        {
            WordSetPanel setPanel = Instantiate(panelPrefab, content.transform);
            setPanel.Init(sprite, caption, count);
        }

        /// <summary>
        /// Выделить плитку по индексу
        /// и снять выделение со всех остальных.
        /// </summary>
        /// <param name="index">Индекс плитки</param>
        public void HighLightTile(int index)
        {
            WordSetPanel[] tiles = GetTiles();
            foreach (var item in tiles)
            {
                item.GetComponent<Image>().color = Color.white;
            }

            WordSetPanel tile = tiles[index];
            tile.GetComponent<Image>().color = Color.yellow;
        }

        /// <summary>
        /// Получить все доступные плитки с группами слов.
        /// </summary>
        /// <returns>Плитки с группами слов</returns>
        public WordSetPanel[] GetTiles()
        {
            if (wordTiles == null || wordTiles.Length < GameManager.WordManeger.GetGroupNames().Count)
            {
                wordTiles = transform.GetComponentsInChildren<WordSetPanel>();
                var panels = from p in wordTiles
                    orderby p.GetName()
                    select p;
                wordTiles = panels.ToArray();
            }

            return wordTiles;
        }

        /// <summary>
        /// Расчет высоты Content
        /// </summary>
        private void CalulateContentHight()
        {
            // TODO: вычислять колличество колонок динамически
            float clumnCount = 3;
            float panelYSpace = content.GetComponent<GridLayoutGroup>().spacing.y * 2;
            float panelCount = content.transform.childCount;

            Vector2 size = new Vector2();
            RectTransform rectContent = content.GetComponent<RectTransform>();
            size.y = (PanelHeight + panelYSpace) * panelCount / clumnCount;
            rectContent.sizeDelta = size;

            // обнуляем значения позиции(глюк в unity?)
            // rectContent.localPosition = Vector3.zero;
        }

        /// <summary>
        ///  Вычисляет высоту всех панелей в 3 колонки
        /// </summary>
        /// <param name="panelCount">колличество панелей</param>
        /// <param name="columnCount">колличество колонок</param>
        private void UpdateContentHeight(float panelCount, float columnCount = 3)
        {
            float height = CalulateHightContainer(panelCount, columnCount);
            SetSizeContent(height);
        }

        /// <summary>
        /// Перемещает объект Content так, чтобы выбранная плитка была видна.!!!!!!!!!!!!
        /// </summary>
        /// <param name="height">Высота</param>
        private void SetSizeContent(float height)
        {
            Vector2 size = new Vector2
            {
                y = height
            };
            RectTransform rectContent = content.GetComponent<RectTransform>();
            rectContent.sizeDelta = size;
            rectContent.localPosition = Vector3.zero;
        }

        /// <summary>
        /// Выставляет высоту Content так, чтобы все плитки поместились.
        /// </summary>
        /// <param name="height">Высота</param>
        public void SetHeigtContent(float height)
        {
            Vector2 size = new Vector2
            {
                y = height // -tileHeight / 2
            };

            // float tileHeight = content.GetComponent<GridLayoutGroup>().cellSize.y;
            // отцентрировать плитку вертикально
            RectTransform rectContent = content.GetComponent<RectTransform>();
            rectContent.localPosition = size;
        }

        /// <summary>
        /// Расчет высоты контейнера до последней карточки
        /// </summary>
        /// <param name="panelIndex">номер панели</param>
        /// <param name="columnCount">колличество колонок</param>
        /// <returns>Высота контейнера до последней карточки</returns>
        public float CalulateHightContainer(float panelIndex, float columnCount = 3)
        {
            SetTileSize();

            var tileHeight = CalcTileHeight();
            var deltaRow = Mathf.Ceil(panelIndex / columnCount);
            var deltaContentHeight = tileHeight * deltaRow;

            var deltaTileHeight = tileHeight + (tileHeight / 2) + 15;

            var height = deltaContentHeight - deltaTileHeight;
            return height;
        }

        /// <summary>
        /// Меняет размер плитки в зависимости от
        /// ширины панели MainPanel
        /// </summary>
        private void SetTileSize()
        {
            string ratio_16x9 = "1.8";
            string ratio_16x10 = "1.6";
            string ratio_5x4 = "1.3";

            Vector2 tileSize = content.GetComponent<GridLayoutGroup>().cellSize;
            Vector2 sizeDelta = GetComponent<RectTransform>().sizeDelta;
            float pixelWidth = Camera.main.pixelWidth;
            float pixelHeight = Camera.main.pixelHeight;
            float ratioIndex = pixelWidth / pixelHeight;

            if (ratio_16x9 == ratioIndex.ToString("0.0"))
            {
                sizeDelta.x = 1527;
                tileSize.x = 500;

                Debug.Log("ratio_16x9");
            }
            else if (ratio_16x10 == ratioIndex.ToString("0.0"))
            {
                sizeDelta.x = 1227;
                tileSize.x = 400;
                Debug.Log("ratio_16x10");
            }
            else if (ratio_5x4 == ratioIndex.ToString("0.0"))
            {
                sizeDelta.x = 927;
                tileSize.x = 300;
                Debug.Log("ratio_5x4");
            }

            tileSize.y = tileSize.x;
            content.GetComponent<GridLayoutGroup>().cellSize = tileSize;
            GetComponent<RectTransform>().sizeDelta = sizeDelta;

            Debug.Log("ratioIndex = " + ratioIndex);
            Debug.Log("tileSize = " + tileSize);
        }

        /// <summary>
        /// Расчитывает высоту плитки.
        /// </summary>
        /// <returns>Высота плитки</returns>
        private float CalcTileHeight()
        {
            // Растояние между плитками сверху и снизу
            float tileHeight = content.GetComponent<GridLayoutGroup>().cellSize.y;
            float panelYSpace = content.GetComponent<GridLayoutGroup>().spacing.y * 2;
            float fullHeightPanel = tileHeight + panelYSpace;
            return fullHeightPanel;
        }
    }
}