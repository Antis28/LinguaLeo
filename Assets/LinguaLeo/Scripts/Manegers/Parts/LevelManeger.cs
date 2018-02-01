using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManeger : MonoBehaviour, Observer
{
    public int lastWorkout = -1;

    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
    }
    public void QuitGame(string name)
    {
        Application.Quit();
    }

    public void LoadNextLevel()
    {
        //Application.loadedLevel
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadLastWorkOut()
    {
        if (lastWorkout != -1)
            SceneManager.LoadScene(lastWorkout);
        else
            print("lastWorkout == -1");
    }

    void Observer.OnNotify(Component sender, GAME_EVENTS notificationName)
    {
        switch (notificationName)
        {
            case GAME_EVENTS.WordsEnded:
                print("ScoreValue = " + GameManager.ScoreKeeper.ScoreValue);
                lastWorkout = SceneManager.GetActiveScene().buildIndex;
                LoadLevel("result");
                break;
            case GAME_EVENTS.ContinueWorkout:
                LoadLastWorkOut();
                break;
        }

    }

    public void Start()
    {
        GameManager.Notifications.AddListener(this, GAME_EVENTS.WordsEnded);
        GameManager.Notifications.AddListener(this, GAME_EVENTS.ContinueWorkout);
    }

    public void OnDestroy()
    {
        print("OnDestroy");
    }
}
