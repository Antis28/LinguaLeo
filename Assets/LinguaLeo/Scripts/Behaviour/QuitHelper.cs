﻿using LinguaLeo._Adapters;
using LinguaLeo.Scripts.Managers;
using UnityEngine;

namespace LinguaLeo.Scripts.Behaviour
{
    public class QuitHelper : MonoBehaviour
    {
        #region Public Methods

        /// <summary>
        /// Завершает игру.
        /// </summary>
        public void QuitGame()
        {
            GameManager.Notifications.PostNotification(null, GameEvents.QuitGame);
            GameManager.SceneLoader.QuitGame();
        }

        /// <summary>
        /// Возращает на сцену 0.
        /// </summary>
        public void RestartGame()
        {
            //Load first level        
            SceneManagerAdapt.LoadScene(0);
        }

        #endregion
    }
}
