using LinguaLeo.Scripts.Helpers.Interfaces;
using LinguaLeo.Scripts.Manegers;
using UnityEngine;

namespace LinguaLeo.Scripts.Behaviour
{
    public class ScoreKeeper : MonoBehaviour, IObserver {
        [SerializeField]
        private float score;
        private float priceScore = 1;
        private float scoreFactor;

        /// <summary>
        /// Выставляет цену за выученое слово в тренировке.
        /// </summary>
        public void SetScoreFactor(float max)
        {
            scoreFactor = max;
        }

        public float ScoreValue
        {
            get
            {
                return score;
            }
        }

        public float GetCorrectAnswers()
        {
            return scoreFactor * score;
        }

        // Use this for initialization
        void Start()
        {
            GameManager.Notifications.AddListener(this, GAME_EVENTS.CorrectAnswer);
            ResetScore();
        }

        public void AddScore(float points)
        {
            score += points;
        }

        public void ResetScore()
        {
            score = 0;
        }

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
    }
}
