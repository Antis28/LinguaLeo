
using LinguaLeo.Scripts.Manegers;
using UnityEngine;
using LinguaLeo.Adapters;

namespace LinguaLeo.Scripts.Behaviour
{
    public class QuitHelper : MonoBehaviour {

        /// <summary>
        /// Завершает игру.
        /// </summary>
        public void QuitGame()
        {
            GameManager.Notifications.PostNotification(null, GAME_EVENTS.QuitGame);
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
    }
}
