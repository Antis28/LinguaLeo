﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitHelper : MonoBehaviour {

    /// <summary>
    /// Завершает игру.
    /// </summary>
    public void QuitGame()
    {
        GameManager.Notifications.PostNotification(null, GAME_EVENTS.QuitGame);
        GameManager.LevelManeger.QuitGame();
    }

    /// <summary>
    /// Возращает на сцену 0.
    /// </summary>
    public void RestartGame()
    {
        //Load first level        
        SceneManagerAdapt.LoadScene(0);
    }
}
