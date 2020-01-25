using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class LevelManeger : MonoBehaviour
{
    public int lastWorkout = -1;

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }

    public void LoadLevel(string name)
    {
        SceneManagerAdapt.LoadScene(name);
    }

    public void LoadResultWorkOut()
    {
        const string nameScene = "result";
        lastWorkout = SceneManagerAdapt.GetActiveScene().buildIndex;
        LoadLevel(nameScene);
    }

    public void LoadNextLevel()
    {
        //Application.loadedLevel
        int buildIndex = SceneManagerAdapt.GetActiveScene().buildIndex + 1;
        SceneManagerAdapt.LoadScene(buildIndex);
    }

    void Start()
    {
        GameManager.Notifications.AddListener(this, GAME_EVENTS.WordsEnded);
    }
}
