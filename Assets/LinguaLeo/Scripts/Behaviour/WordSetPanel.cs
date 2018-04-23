using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordSetPanel : MonoBehaviour
{
    [SerializeField]
    Image LogoImage = null;
    [SerializeField]
    Text CaptionText = null;
    [SerializeField]
    Text WordCountText = null;

    string sceneName = "trainingСhoice";
    Button learnButton;

    string groupName = string.Empty;

    public void Init(Sprite sprite, string caption, int count)
    {
        LogoImage.sprite = sprite;
        groupName = caption;
        CaptionText.text = groupName.Replace('_', ' ');
        WordCountText.text = count + " cлов";

        GetComponent<Transform>().localScale = Vector3.one;
        GetComponent<Transform>().localPosition = Vector3.zero;
    }

    // Use this for initialization
    void Start()
    {
        learnButton = transform.Find("LearnButton").GetComponent<Button>();
        LevelManeger levelManeger = FindObjectOfType<LevelManeger>();

        learnButton.onClick.AddListener(() => levelManeger.LoadLevel(sceneName));
        learnButton.onClick.AddListener(PanelClick);
    }

    public void PanelClick()
    {
        Debug.Log("LoadGroup = " + groupName);
        GameManager.WordManeger.LoadGroup(groupName);
        foreach (WordLeo item in GameManager.WordManeger.GetAllGroupWords())
        {
            Debug.Log(item.wordValue);
        }
    }

    // OnMouseDown is called when the user has pressed the mouse button while over the GUIElement or Collider
    public void OnMouseDown()
    {
        Debug.Log("OnMouseDown");
    }

    public string GetName()
    {
        return CaptionText.text;
    }
}
