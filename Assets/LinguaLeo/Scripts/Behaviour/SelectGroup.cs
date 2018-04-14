using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SelectGroup : MonoBehaviour, Observer
{
    public const int PANEL_HEIGHT = 500;

    [SerializeField]
    GameObject content = null;
    [SerializeField]
    WordSetPanel panelPrefab = null;

    // Use this for initialization
    void Start()
    {
        GameManager.Notifications.AddListener(this, GAME_EVENTS.LoadedVocabulary);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void Observer.OnNotify(object parametr, GAME_EVENTS notificationName)
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

    private void CreateCard(Sprite sprite, string caption, int count)
    {
        WordSetPanel setPanel = Instantiate(panelPrefab, content.transform);
        setPanel.Init(sprite, caption, count);
    }

    private void CalulateContentHight()
    {
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
        Vector2 size = new Vector2();
        size.y = height;
        RectTransform rectContent = content.GetComponent<RectTransform>();
        rectContent.sizeDelta = size;
        rectContent.localPosition = Vector3.zero;
    }

    public void SetHeigtContent(float height)
    {
        Vector2 size = new Vector2();
        size.y = height;
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
        // Растояние между панелями сверху и снизу
        float panelYSpace = content.GetComponent<GridLayoutGroup>().spacing.y * 2;
        float fullHeightPanel = (PANEL_HEIGHT + panelYSpace);
        // Расчет высоты контейнера до последней карточки
        float height = fullHeightPanel * panelCount / columnCount - fullHeightPanel;
        return height;
    }
}
