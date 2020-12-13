#region

using LinguaLeo.Scripts.Helpers;
using LinguaLeo.Scripts.Helpers.Interfaces;
using LinguaLeo.Scripts.Manegers;
using LinguaLeo.Scripts.Workout;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace LinguaLeo.Scripts.Behaviour
{
    public class AnswerResult : MonoBehaviour, IObserver
    {
        #region SerializeFields

        [SerializeField]
        private Text ResultText = null;

        [SerializeField]
        private Text SampleText = null;

        #endregion

        #region Private variables

        private readonly Color correctColor = new Color(59 / 255f,
                                                        152 / 255f,
                                                        57 / 255f);

        private readonly Color wrongColor = new Color(157 / 255f,
                                                      38 / 255f,
                                                      29 / 255f);

        #endregion

        #region Events

        public void OnNotify(object parametr, GAME_EVENTS notificationName)
        {
            ButtonComponent button = null;
            if (parametr != null)
                button = ((Component) parametr).GetComponent<ButtonComponent>();
            if (ResultText == null)
                return;
            switch (notificationName)
            {
                case GAME_EVENTS.CorrectAnswer:
                    ResultText.text = "Верный ответ.";
                    ResultText.color = correctColor;
                    break;
                case GAME_EVENTS.NonCorrectAnswer:
                    ResultText.text = "Неверный ответ.";
                    ResultText.color = wrongColor;
                    FillSamleText(button);
                    break;
                case GAME_EVENTS.BuildTask:
                    ResultText.text = string.Empty;
                    SampleText.text = string.Empty;
                    break;
            }
        }

        #endregion

        #region Unity events

        // Use this for initialization
        private void Start()
        {
            SubscribeToEvents();
        }

        #endregion

        #region Private Methods

        private void FillSamleText(ButtonComponent button)
        {
            var wordToTranslate = FindObjectOfType<WordToTranslate>();
            if (!button || !wordToTranslate)
            {
                Debug.LogWarning("Не найден WordToTranslate");
                return;
            }

            var quest = wordToTranslate.GetCurrentQuest();
            WordLeo word = null;
            foreach (var item in quest.answers)
            {
                if (button.text.text == item.translations
                    || button.text.text == item.wordValue)
                {
                    word = item;
                    break;
                }
            }

            if (word == null)
                return;
            if (button.text.text == word.translations)
                SampleText.text = word.translations + " → " + word.wordValue;
            else
                SampleText.text = word.wordValue + " → " + word.translations;
        }

        private void SubscribeToEvents()
        {
            var notification = GameManager.Notifications;

            notification.AddListener(this, GAME_EVENTS.CorrectAnswer);
            notification.AddListener(this, GAME_EVENTS.NonCorrectAnswer);
            notification.AddListener(this, GAME_EVENTS.BuildTask);
        }

        #endregion
    }
}
