using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordInfoPanel : MonoBehaviour {
    
    [SerializeField]
    Image wordImage = null;
    [SerializeField]
    Text wordText = null;
    [SerializeField]
    Text levelText = null;
    [SerializeField]
    Text TimeReduceText = null;
    [SerializeField]
    Text TimeUnlockText = null;

    public void Init(WordLeo word)
    {
        wordImage.sprite = Utilities.GetSprite(word.pictureURL);
        wordImage.preserveAspect = true;

        levelText.text = word.progress.license.ToString();
        wordText.text = word.wordValue + " - " + word.translations;
        word.LicenseExpirationCheck();

        TimeSpan time = word.GetLicenseValidityTime();
        TimeReduceText.text = Utilities.FormatTime(time);

        TimeSpan timeUnlock = word.GetLicenseUnlockForRepeat();
        TimeUnlockText.text = Utilities.FormatTime(timeUnlock);

        GetComponent<Transform>().localScale = Vector3.one;
        GetComponent<Transform>().localPosition = Vector3.zero;
    }

    // Use this for initialization
    void Start()
    {
        LevelManeger levelManeger = FindObjectOfType<LevelManeger>();
    }

    // OnMouseDown is called when the user has pressed the mouse button while over the GUIElement or Collider
    public void OnMouseDown()
    {
        Debug.Log("OnMouseDown");
    }
    public string GetName()
    {
        return wordText.text;
    }
}
