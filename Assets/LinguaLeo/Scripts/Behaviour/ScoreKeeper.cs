using Helpers.Interfaces;
using Managers;
using UnityEngine;

namespace Behaviour
{
    public class ScoreKeeper : MonoBehaviour, IObserver
    {
        #region SerializeFields

        [SerializeField]
        private float score;

        #endregion

        #region Public variables

        public float ScoreValue
        {
            get { return score; }
        }

        #endregion

        #region Private variables

        private float priceScore = 1;
        private float scoreFactor;

        #endregion

        #region Events

        public void OnNotify(object parametr, GAME_EVENTS notificationName)
        {
            switch (notificationName)
            {
                case GAME_EVENTS.CorrectAnswer:
                    AddScore(priceScore);
                    print("Верный ответ");
                    break;
            }
        }

        #endregion

        #region Unity events

        // Use this for initialization
        private void Start()
        {
            GameManager.Notifications.AddListener(this, GAME_EVENTS.CorrectAnswer);
            ResetScore();
        }

        #endregion

        #region Public Methods

        public void AddScore(float points)
        {
            score += points;
        }

        public float GetCorrectAnswers()
        {
            return scoreFactor * score;
        }

        public void ResetScore()
        {
            score = 0;
        }

        /// <summary>
        /// Выставляет цену за выученое слово в тренировке.
        /// </summary>
        public void SetScoreFactor(float max)
        {
            scoreFactor = max;
        }

        #endregion
    }
}
