using System.Collections;
using System.Collections.Generic;
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

    private void CreatePanels()
    {
        var binary = Resources.Load<Sprite>("Covers" + "/" + 108);
        List<string> group = GameManager.WordManeger.GetGroupNames();// WordManeger.Vocabulary.FilterGroup();

        foreach (var item in group)
        {
            CreateCard(binary, item);
        }
    }

    private void CreateCard(Sprite binary, string caption)
    {
        WordSetPanel setPanel = Instantiate(panelPrefab, content.transform);
        setPanel.Init(binary, caption, 2);
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
