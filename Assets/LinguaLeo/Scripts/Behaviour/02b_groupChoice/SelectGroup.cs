#region

using System.Collections;
using System.Linq;
using LinguaLeo.Scripts.Helpers.Interfaces;
using LinguaLeo.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace LinguaLeo.Scripts.Behaviour._02b_groupChoice
{
    /// <summary>
    /// Выбор группы слов для изучения.
    /// </summary>
    public class SelectGroup : MonoBehaviour, IObserver
    {
        #region Static Fields and Constants

        private const float PanelHeight = 500;

        #endregion

        #region SerializeFields

        /// <summary>
        /// Родительский объект в который будут складываться карточки группы.
        /// </summary>
        [SerializeField]
        private GameObject content = null;

        /// <summary>
        /// Образец карточки группы.
        /// </summary>
        [SerializeField]
        private WordSetPanel panelPrefab = null;

        #endregion

        #region Private variables

        /// <summary>
        /// Список всех UI панелей групп слов.
        /// </summary>
        private WordSetPanel[] wordTiles;

        #endregion

        #region Events

        /// <summary>
        /// Получает уведомления о игровых событиях.
        /// </summary>
        /// <param name="parametr">Параметры</param>
        /// <param name="notificationName">Тип событя</param>
        void IObserver.OnNotify(object parametr, GameEvents notificationName)
        {
            switch (notificationName)
            {
                case GameEvents.LoadedVocabulary:
                    Debug.Log("Start Size");
                    StartCoroutine(CreatePanels());
                    break;
            }
        }


        #endregion

        #region Unity events

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Start()
        {
            GameManager.Notifications.AddListener(this, GameEvents.LoadedVocabulary);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Получить высоту контейнера до последней плитки
        /// </summary>
        /// <param name="panelIndex">номер плитки</param>
        /// <param name="columnCount">колличество колонок</param>
        /// <returns>Высота контейнера до последней карточки</returns>
        public float GetHightContainer(float panelIndex, float columnCount = 3)
        {
            SetTileSize();
            return DeltaContentHeight(panelIndex, columnCount);
        }

        /// <summary>
        /// Получить высоту контейнера до плитки
        /// </summary>
        /// <param name="panelIndex">номер плитки</param>
        /// <param name="columnCount">колличество колонок</param>
        /// <returns></returns>
        public float GetHightToTile(float panelIndex, float columnCount = 3)
        {
            var deltaContentHeight = DeltaContentHeight(panelIndex + 1, columnCount);
            var tileHeight = CalcTileHeight();
            var deltaTileHeight = tileHeight + (tileHeight / 2);

            var height = deltaContentHeight - deltaTileHeight;
            return height;
        }

        /// <summary>
        /// Получить все доступные плитки с группами слов.
        /// </summary>
        /// <returns>Плитки с группами слов</returns>
        public WordSetPanel[] GetTiles()
        {
            if (wordTiles == null || wordTiles.Length < GameManager.WordManager.GetGroupNames().Count)
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
        /// Выделить плитку по индексу
        /// и снять выделение со всех остальных.
        /// </summary>
        /// <param name="index">Индекс плитки</param>
        public void HighLightTile(int index)
        {
            var tiles = GetTiles();
            foreach (var item in tiles) { item.GetComponent<Image>().color = Color.white; }

            var tile = tiles[index];
            tile.GetComponent<Image>().color = Color.yellow;
        }

        /// <summary>
        /// Выставляет высоту Content так, чтобы все плитки поместились.
        /// </summary>
        /// <param name="height">Высота</param>
        public void SetHeigtContent(float height)
        {
            var size = new Vector2
            {
                y = height // -tileHeight / 2
            };

            // float tileHeight = content.GetComponent<GridLayoutGroup>().cellSize.y;
            // отцентрировать плитку вертикально
            var rectContent = content.GetComponent<RectTransform>();
            rectContent.localPosition = size;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Расчитывает высоту плитки.
        /// </summary>
        /// <returns>Высота плитки</returns>
        private float CalcTileHeight()
        {
            // Растояние между плитками сверху и снизу
            var tileHeight = content.GetComponent<GridLayoutGroup>().cellSize.y;
            var panelYSpace = content.GetComponent<GridLayoutGroup>().spacing.y * 2;
            var fullHeightPanel = tileHeight + panelYSpace;
            return fullHeightPanel;
        }

        /// <summary>
        /// Расчет высоты Content
        /// </summary>
        private void CalulateContentHight()
        {
            // TODO: вычислять колличество колонок динамически
            float clumnCount = 3;
            var panelYSpace = content.GetComponent<GridLayoutGroup>().spacing.y * 2;
            float panelCount = content.transform.childCount;

            var size = new Vector2();
            var rectContent = content.GetComponent<RectTransform>();
            size.y = (PanelHeight + panelYSpace) * panelCount / clumnCount;
            rectContent.sizeDelta = size;

            // обнуляем значения позиции(глюк в unity?)
            // rectContent.localPosition = Vector3.zero;
        }

        /// <summary>
        /// Создает отдельную панель для описания группы слов.
        /// </summary>
        /// <param name="sprite">Обложка</param>
        /// <param name="caption">Название группы</param>
        /// <param name="count">Количество слов в группе</param>
        private void CreateCard(Sprite sprite, string caption, int count)
        {
            var setPanel = Instantiate(panelPrefab, content.transform);
            setPanel.Init(sprite, caption, count);
        }

        /// <summary>
        /// Генерирует панели групп слов.
        /// Ооочень медленно.
        /// </summary>
        /// <returns>необходимо для корутины</returns>
        private IEnumerator CreatePanels()
        {
            var groups = GameManager.WordManager.GetGroupNames();
            UpdateContentHeight(groups.Count);
            foreach (var item in groups)
            {
                var sprite = GameManager.ResourcesLoader.GetCover(item.pictureName);

                CreateCard(sprite, item.name, item.wordCount);
                yield return null;
            }
        }

        /// <summary>
        /// Расчет высоты контейнера до верхней грани плитки.
        /// </summary>
        /// <param name="panelIndex">номер плитки</param>
        /// <param name="columnCount">количество столбцов</param>
        /// <returns>Высота до вершины плитки</returns>
        private float DeltaContentHeight(float panelIndex, float columnCount)
        {
            var tileHeight = CalcTileHeight();
            var deltaRow = Mathf.Ceil(panelIndex / columnCount);
            var deltaContentHeight = tileHeight * deltaRow;
            return deltaContentHeight;
        }

        /// <summary>
        /// Перемещает объект Content так, чтобы выбранная плитка была видна.!!!!!!!!!!!!
        /// </summary>
        /// <param name="height">Высота</param>
        private void SetSizeContent(float height)
        {
            var size = new Vector2
            {
                y = height
            };
            var rectContent = content.GetComponent<RectTransform>();
            rectContent.sizeDelta = size;
            rectContent.localPosition = Vector3.zero;
        }

        /// <summary>
        /// Меняет размер плитки в зависимости от
        /// ширины панели MainPanel
        /// </summary>
        private void SetTileSize()
        {
            var ratio16X9 = "1.8";
            var ratio16X10 = "1.6";
            var ratio5X4 = "1.3";

            var tileSize = content.GetComponent<GridLayoutGroup>().cellSize;
            var sizeDelta = GetComponent<RectTransform>().sizeDelta;
            float pixelWidth = Camera.main.pixelWidth;
            float pixelHeight = Camera.main.pixelHeight;
            var ratioIndex = pixelWidth / pixelHeight;

            if (ratio16X9 == ratioIndex.ToString("0.0"))
            {
                sizeDelta.x = 1527;
                tileSize.x = 500;

                Debug.Log("ratio_16x9");
            } else if (ratio16X10 == ratioIndex.ToString("0.0"))
            {
                sizeDelta.x = 1227;
                tileSize.x = 400;
                Debug.Log("ratio_16x10");
            } else if (ratio5X4 == ratioIndex.ToString("0.0"))
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
        ///  Вычисляет высоту всех панелей в 3 колонки
        /// </summary>
        /// <param name="panelCount">колличество панелей</param>
        /// <param name="columnCount">колличество колонок</param>
        private void UpdateContentHeight(float panelCount, float columnCount = 3)
        {
            var height = GetHightContainer(panelCount, columnCount);
            SetSizeContent(height);
        }

        #endregion
      
    }
}
