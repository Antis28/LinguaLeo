using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordTranslate : MonoBehaviour, Observer
{

    public Text wordText;
    public Image wordImage;

    public Slider scoreSlider;
    public Text scoreText;   
    
    private int answersCount;


    // Use this for initialization
    void Awake()
    {
        GameManager.Notifications.AddListener(this, GAME_EVENTS.BuildTask);
    }

    void Observer.OnNotify(Component sender, GAME_EVENTS notificationName)
    {
        switch (notificationName)
        {
            case GAME_EVENTS.BuildTask:
                ProgeressUpdate();
                break;
        }
    }

    private void ProgeressUpdate()
    {
        answersCount++;
        scoreText.text = answersCount + "/" + scoreSlider.maxValue;
        scoreSlider.value = answersCount;
    }
}
