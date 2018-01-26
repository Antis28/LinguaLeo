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

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PanelClick()
    {
        Debug.Log("Test = " + groupName);
        GameManager.WordManeger.LoadGroup(groupName);
    }

    // OnMouseDown is called when the user has pressed the mouse button while over the GUIElement or Collider
    public void OnMouseDown()
    {
        Debug.Log("OnMouseDown");
    }

    // OnMouseUp is called when the user has released the mouse button
    public void OnMouseUp()
    {
        Debug.Log("OnMouseUp");
    }

    // OnMouseExit is called when the mouse is not any longer over the GUIElement or Collider
    public void OnMouseExit()
    {

    }

    // OnMouseEnter is called when the mouse entered the GUIElement or Collider
    public void OnMouseEnter()
    {

    }
}
