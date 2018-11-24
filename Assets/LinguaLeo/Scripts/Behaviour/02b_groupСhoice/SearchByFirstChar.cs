using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SearchByFirstChar : MonoBehaviour
{
    public Text gt;
    WordSetPanel[] allWordSetPanel = null;
    SelectGroup selectGroup = null;

    void Start()
    {
        gt = GetComponent<Text>();
        allWordSetPanel = GetAllWordSetPanel();
        selectGroup = FindObjectOfType<SelectGroup>();
    }
    void Update()
    {
        FindSubString();
    }

    private void FindSubString()
    {
        foreach (char c in Input.inputString)
        {
            if (c == "\b"[0])
            {
                if (gt.text.Length != 0)

                    gt.text = gt.text.Substring(0, gt.text.Length - 1);
            }
            else
                gt.text += c;


            WordSetPanel panel = FindPanelByCaption(gt.text);
            if (panel == null)
                return;
            GoToPanel(panel);
            EventSystem.current.SetSelectedGameObject(panel.gameObject);
        }
    }

    private void GoToPanel(WordSetPanel panel)
    {
        int index = allWordSetPanel.ToList().FindIndex((x) => panel.GetName() == x.GetName());

        float high = selectGroup.CalulateHightContainer(index + 1);
        selectGroup.SetHeigtContent(high);
        selectGroup.HighLightTile(index);
    }

    private WordSetPanel FindPanelByCaption(string substring)
    {
        allWordSetPanel = GetAllWordSetPanel();

        var t = from p in allWordSetPanel
                orderby p.GetName()
                select p;
        allWordSetPanel = t.ToArray();

        foreach (WordSetPanel panel in allWordSetPanel)
        {
            string panelName = panel.GetName().ToLower();
            substring = substring.ToLower();
            if (panelName.StartsWith(substring))
            {
                return panel;
            }
        }
        foreach (WordSetPanel panel in allWordSetPanel)
        {
            string panelName = panel.GetName().ToLower();
            substring = substring.ToLower();
            if (panelName.Contains(substring))
            {
                return panel;
            }
        }
        return null;
    }

    private WordSetPanel[] GetAllWordSetPanel()
    {
        WordSetPanel[] allWordSetPanel = FindObjectsOfType<WordSetPanel>();
        return allWordSetPanel;
    }
}
