#region

using LinguaLeo.Scripts.Helpers;
using LinguaLeo.Scripts.Helpers.Interfaces;
using LinguaLeo.Scripts.Managers;
using LinguaLeo.Scripts.Workout;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

#endregion

namespace LinguaLeo.Scripts.Behaviour
{
    public class AnswerResult : MonoBehaviour, IObserver
    {
        #region SerializeFields

        [FormerlySerializedAs("ResultText")]
        [SerializeField]
        private Text resultText = null;

        [FormerlySerializedAs("SampleText")]
        [SerializeField]
        private Text sampleText = null;

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

        public void OnNotify(object parametr, GameEvents notificationName)
        {
            ButtonComponent button = null;
            if (parametr != null)
                button = ((Component) parametr).GetComponent<ButtonComponent>();
            if (resultText == null)
                return;
            switch (notificationName)
            {
                case GameEvents.CorrectAnswer:
                    resultText.text = "Верный ответ.";
                    resultText.color = correctColor;
                    break;
                case GameEvents.NonCorrectAnswer:
                    resultText.text = "Неверный ответ.";
                    resultText.color = wrongColor;
                    FillSamleText(button);
                    break;
                case GameEvents.BuildTask:
                    resultText.text = string.Empty;
                    sampleText.text = string.Empty;
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
                sampleText.text = word.translations + " → " + word.wordValue;
            else
                sampleText.text = word.wordValue + " → " + word.translations;
        }

        private void SubscribeToEvents()
        {
            var notification = GameManager.Notifications;

            notification.AddListener(this, GameEvents.CorrectAnswer);
            notification.AddListener(this, GameEvents.NonCorrectAnswer);
            notification.AddListener(this, GameEvents.BuildTask);
        }

        #endregion
    }
}
