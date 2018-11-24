using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SearchWordCardByChar : MonoBehaviour {

    public Text gt;
    WordInfoPanel[] allWordInfoPanel = null;
    WordView wordView = null;

    void Start()
    {
        gt = GetComponent<Text>();
        allWordInfoPanel = GetAllWordSetPanel();
        wordView = FindObjectOfType<WordView>();
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


            WordInfoPanel panel = FindPanelByCaption(gt.text);
            if (panel == null)
                return;
            GoToPanel(panel);
            EventSystem.current.SetSelectedGameObject(panel.gameObject);
        }
    }

    private void GoToPanel(WordInfoPanel panel)
    {
        int index = allWordInfoPanel.ToList().FindIndex((x) => panel.GetName() == x.GetName());

        float high = wordView.CalulateHightContainer(index + 1);
        wordView.SetHeigtContent(high);
        wordView.HighLightTile(index);
    }

    private WordInfoPanel FindPanelByCaption(string substring)
    {
        allWordInfoPanel = GetAllWordSetPanel();

        var t = from p in allWordInfoPanel
                orderby p.GetName()
                select p;
        allWordInfoPanel = t.ToArray();

        foreach (WordInfoPanel panel in allWordInfoPanel)
        {
            string panelName = panel.GetName().ToLower();
            substring = substring.ToLower();
            if (panelName.StartsWith(substring))
            {
                return panel;
            }
        }
        foreach (WordInfoPanel panel in allWordInfoPanel)
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

    private WordInfoPanel[] GetAllWordSetPanel()
    {
        WordInfoPanel[] allWordSetPanel = FindObjectsOfType<WordInfoPanel>();
        return allWordSetPanel;
    }
}
