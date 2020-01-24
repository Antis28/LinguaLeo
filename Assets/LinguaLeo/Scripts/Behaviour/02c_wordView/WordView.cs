using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class WordView : MonoBehaviour, IObserver
{

    [SerializeField]
    GameObject content = null;
    [SerializeField]
    WordInfoPanel panelPrefab = null;
    [SerializeField]
    private Color unselectWordColor = Color.yellow;
    [SerializeField]
    private Color selectWordColor = Color.yellow;

    // Use this for initialization
    void Start()
    {
        GameManager.Notifications.AddListener(this, GAME_EVENTS.LoadedVocabulary);
    }

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

    private IEnumerator CreatePanels() //int spriteName
    {
        List<WordLeo> group = GameManager.WordManeger.GetAllGroupWords();
        UpdateContentHeight(group.Count);
        foreach (var item in group)
        {
            CreateCard(item);
            yield return null;
        }
    }

    private void CreateCard(WordLeo word)
    {
        WordInfoPanel setPanel = Instantiate(panelPrefab, content.transform);
        setPanel.Init(word);
    }

    internal void HighLightTile(int index)
    {
        WordInfoPanel[] tiles = transform.GetComponentsInChildren<WordInfoPanel>();
        foreach (var item in tiles)
        {
            item.GetComponent<Image>().color = unselectWordColor;
        }

        WordInfoPanel tile = tiles[index];
        tile.GetComponent<Image>().color = selectWordColor;
    }

    private void CalulateContentHight()
    {
        float PANEL_HEIGHT = 500;
        //TODO: вычислять колличество колонок динамически
        float clumnCount = 3;
        float panelYSpace = content.GetComponent<GridLayoutGroup>().spacing.y * 2;
        float panelCount = content.transform.childCount;

        Vector2 size = new Vector2();
        RectTransform rectContent = content.GetComponent<RectTransform>();
        size.y = (PANEL_HEIGHT + panelYSpace) * panelCount / clumnCount;
        rectContent.sizeDelta = size;

        //обнуляем значения позиции(глюк в unity?)
        //rectContent.localPosition = Vector3.zero;
    }

    /// <summary>
    ///  Вычисляет высоту всех панелей в 3 колонки
    /// </summary>
    /// <param name="panelCount">колличество панелей</param>
    private void UpdateContentHeight(float panelCount, float columnCount = 3)
    {
        float height = CalulateHightContainer(panelCount, columnCount);
        SetSizeContent(height);
    }

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

    public void SetHeigtContent(float height)
    {
        Vector2 size = new Vector2
        {
            // float tileHeight = content.GetComponent<GridLayoutGroup>().cellSize.y;
            // отцентрировать плитку вертикально
            y = height// -tileHeight / 2
        };
        RectTransform rectContent = content.GetComponent<RectTransform>();
        rectContent.localPosition = size;
    }

    /// <summary>
    /// Расчет высоты контейнера до последней карточки
    /// </summary>
    /// <param name="panelCount"></param>
    /// <param name="columnCount"></param>
    /// <returns></returns>
    public float CalulateHightContainer(float panelCount, float columnCount = 3)
    {
        SetTileSize();

        float tileHeight = CalcTileHeight();
        // Расчет высоты контейнера до последней карточки
        float height = tileHeight * panelCount / columnCount;
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

    private float CalcTileHeight()
    {
        // Растояние между плитками сверху и снизу
        float tileHeight = content.GetComponent<GridLayoutGroup>().cellSize.y;
        float panelYSpace = content.GetComponent<GridLayoutGroup>().spacing.y * 2;
        float fullHeightPanel = (tileHeight + panelYSpace);
        return fullHeightPanel;
    }
}
