using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour, Observer {

    Text ScoreValueText;
    static int score;

    public static int ScoreValue
    {
        get
        {
            return score;
        }

        set
        {
            score = value;
        }
    }

    // Use this for initialization
    void Start () {
        GameManager.Notifications.AddListener(this, GAME_EVENTS.CorrectAnswer);
        GameManager.Notifications.AddListener(this, GAME_EVENTS.NonCorrectAnswer);

    }

    public void Score(int points )
    {
        ScoreValue += points;
        UpdateScore();
    }  

    public static void Reset()
    {
        ScoreValue = 0;
    }

    private void UpdateScore()
    {
        if( ScoreValueText )
            ScoreValueText.text = ScoreValue.ToString();
    }

    public void OnNotify(Component sender, GAME_EVENTS notificationName)
    {
        switch (notificationName)
        {
            case GAME_EVENTS.CorrectAnswer:
                ScoreValue++;
                break;
        }
    }
}
