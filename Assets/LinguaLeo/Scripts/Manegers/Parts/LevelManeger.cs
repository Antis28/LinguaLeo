using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManeger : MonoBehaviour, Observer
{

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

    void Observer.OnNotify(Component sender, GAME_EVENTS notificationName)
    {
        switch (notificationName)
        {
            case GAME_EVENTS.Exit:
                LoadLevel("result");
                break;
        }

    }

    public void Start()
    {
        GameManager.Notifications.AddListener(this, GAME_EVENTS.Exit);
    }
}
