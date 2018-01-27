using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SelectGroup : MonoBehaviour, Observer
{
    const int PANEL_HEIGHT = 500;

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
    void Observer.OnNotify(Component sender, GAME_EVENTS notificationName)
    {
        switch (notificationName)
        {
            case GAME_EVENTS.LoadedVocabulary:
                Debug.Log("Start Size");
                CreatePanels();
                CalulateContentHight();
                break;
        }
    }

    private void CreatePanels() //int spriteName
    {
       List<WordGroup> group = GameManager.WordManeger.GetGroupNames();
        foreach (var item in group)
        {
            Sprite sprite = LoadSpriteFromFile(item.pictureName);
            CreateCard(sprite, item.name, item.wordCount);
        }
    }

    private Sprite LoadSpriteFromFile(string pictureName)
    {
        string path = "Data/Covers" + "/" + pictureName + ".png";

        byte[] picture;
        using (FileStream stream = new FileStream(path, FileMode.Open))
        {
            picture = new byte[stream.Length];
            // считываем данные
            stream.Read(picture, 0, picture.Length);
        }

        Texture2D texture2D = new Texture2D(1, 1);
        texture2D.LoadImage(picture);

        Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(.5f, .5f));
        return sprite;
    }

    private void CreateCard(Sprite binary, string caption, int count)
    {
        WordSetPanel setPanel = Instantiate(panelPrefab, content.transform);
        setPanel.Init(binary, caption, count);
    }

    private void CalulateContentHight()
    {
        //TODO: вычислять колличество строк динамически
        float rowCount = 3;
        float panelYSpace = content.GetComponent<GridLayoutGroup>().spacing.y * 2;
        float panelCount = content.transform.childCount;

        Vector2 size = new Vector2();
        RectTransform rectContent = content.GetComponent<RectTransform>();
        size.y = (PANEL_HEIGHT + panelYSpace) * panelCount / rowCount;
        rectContent.sizeDelta = size;

        //обнуляем значения позиции(глюк в unity?)
        rectContent.localPosition = Vector3.zero;
    }
}
