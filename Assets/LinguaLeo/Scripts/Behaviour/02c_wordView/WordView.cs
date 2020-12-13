using System.Collections;
using LinguaLeo.Scripts.Helpers;
using LinguaLeo.Scripts.Helpers.Interfaces;
using LinguaLeo.Scripts.Manegers;
using UnityEngine;
using UnityEngine.UI;

namespace LinguaLeo.Scripts.Behaviour._02c_wordView
{
    public class WordView : MonoBehaviour, IObserver
    {
        #region SerializeFields

        [SerializeField]
        private GameObject content = null;

        [SerializeField]
        private WordInfoPanel panelPrefab = null;

        [SerializeField]
        private Color selectWordColor = Color.yellow;

        [SerializeField]
        private Color unselectWordColor = Color.yellow;

        #endregion

        #region Events

        void IObserver.OnNotify(object parameter, GAME_EVENTS notificationName)
        {
            if (notificationName != GAME_EVENTS.LoadedVocabulary) return;

            Debug.Log("Start Size");
            StartCoroutine(CreatePanels());
        }

        #endregion

        #region Unity events

        // Use this for initialization
        private void Start()
        {
            GameManager.Notifications.AddListener(this, GAME_EVENTS.LoadedVocabulary);
        }

        #endregion

        #region Public Methods

        public void HighLightTile(int index)
        {
            var tiles = transform.GetComponentsInChildren<WordInfoPanel>();
            foreach (var item in tiles) item.GetComponent<Image>().color = unselectWordColor;

            var tile = tiles[index];
            tile.GetComponent<Image>().color = selectWordColor;
        }

        public void SetHeightContent(float height)
        {
            var size = new Vector2
            {
                // float tileHeight = content.GetComponent<GridLayoutGroup>().cellSize.y;
                // отцентрировать плитку вертикально
                y = height // -tileHeight / 2
            };
            var rectContent = content.GetComponent<RectTransform>();
            rectContent.localPosition = size;
        }

        /// <summary>
        ///     Расчет высоты контейнера до последней карточки
        /// </summary>
        /// <param name="panelCount"></param>
        /// <param name="columnCount"></param>
        /// <returns></returns>
        public float СalculateHeightContainer(float panelCount, float columnCount = 3)
        {
            SetTileSize();

            var tileHeight = CalcTileHeight();

            // Расчет высоты контейнера до последней карточки
            var height = (tileHeight * panelCount) / columnCount;
            return height;
        }

        #endregion

        #region Private Methods

        private float CalcTileHeight()
        {
            // Растояние между плитками сверху и снизу
            var tileHeight = content.GetComponent<GridLayoutGroup>().cellSize.y;
            var panelYSpace = content.GetComponent<GridLayoutGroup>().spacing.y * 2;
            var fullHeightPanel = tileHeight + panelYSpace;
            return fullHeightPanel;
        }

        private void CalculateContentHeight()
        {
            const float panelHeight = 500;

            //TODO: вычислять колличество колонок динамически
            const float columnCount = 3;
            var panelYSpace = content.GetComponent<GridLayoutGroup>().spacing.y * 2;
            float panelCount = content.transform.childCount;

            var size = new Vector2();
            var rectContent = content.GetComponent<RectTransform>();
            size.y = ((panelHeight + panelYSpace) * panelCount) / columnCount;
            rectContent.sizeDelta = size;

            //обнуляем значения позиции(глюк в unity?)
            //rectContent.localPosition = Vector3.zero;
        }

        private void CreateCard(WordLeo word)
        {
            var setPanel = Instantiate(panelPrefab, content.transform);
            setPanel.Init(word);
        }

        private IEnumerator CreatePanels() //int spriteName
        {
            var group = GameManager.WordManeger.GetAllGroupWords();
            UpdateContentHeight(group.Count);
            foreach (var item in group)
            {
                CreateCard(item);
                yield return null;
            }
        }

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
        ///     Меняет размер плитки в зависимости от
        ///     ширины панели MainPanel
        /// </summary>
        private void SetTileSize()
        {
            const string ratio16X9 = "1.8";
            const string ratio16X10 = "1.6";
            const string ratio5X4 = "1.3";

            var tileSize = content.GetComponent<GridLayoutGroup>().cellSize;
            var sizeDelta = GetComponent<RectTransform>().sizeDelta;
            float pixelWidth = Camera.main.pixelWidth;
            float pixelHeight = Camera.main.pixelHeight;
            var ratioIndex = pixelWidth / pixelHeight;

            switch (ratioIndex.ToString("0.0"))
            {
                case ratio16X9:
                    sizeDelta.x = 1527;
                    tileSize.x = 500;

                    Debug.Log("ratio_16x9");
                    break;
                case ratio16X10:
                    sizeDelta.x = 1227;
                    tileSize.x = 400;
                    Debug.Log("ratio_16x10");
                    break;
                case ratio5X4:
                    sizeDelta.x = 927;
                    tileSize.x = 300;
                    Debug.Log("ratio_5x4");
                    break;
            }

            tileSize.y = tileSize.x;
            content.GetComponent<GridLayoutGroup>().cellSize = tileSize;
            GetComponent<RectTransform>().sizeDelta = sizeDelta;

            Debug.Log("ratioIndex = " + ratioIndex);
            Debug.Log("tileSize = " + tileSize);
        }

        /// <summary>
        ///     Вычисляет высоту всех панелей в 3 колонки
        /// </summary>
        /// <param name="panelCount">колличество панелей</param>
        /// <param name="columnCount">колличество колонок</param>
        private void UpdateContentHeight(float panelCount, float columnCount = 3)
        {
            var height = СalculateHeightContainer(panelCount, columnCount);
            SetSizeContent(height);
        }

        #endregion
    }
}
