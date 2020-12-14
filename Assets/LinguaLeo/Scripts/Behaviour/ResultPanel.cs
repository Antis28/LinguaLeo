using System;
using LinguaLeo.Scripts.Helpers;
using LinguaLeo.Scripts.Managers;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace LinguaLeo.Scripts.Behaviour
{
    public class ResultPanel : MonoBehaviour
    {
        #region Static Fields and Constants

        private const int BedResult = 0;
        private const int GoodResult = 9;
        private const int BestResult = 10;

        #endregion

        #region SerializeFields

        [FormerlySerializedAs("CaptionText")]
        [SerializeField]
        private Text captionText = null;

        [FormerlySerializedAs("LearnText")]
        [SerializeField]
        private Text learnText = null;

        [FormerlySerializedAs("EatingText")]
        [SerializeField]
        private Text eatingText = null;

        [SerializeField]
        private Button[] buttons;

        #endregion

        #region Unity events

        // Use this for initialization
        private void Start()
        {
            float score = GameManager.ScoreKeeper.GetCorrectAnswers();
            ShowCaption(score);
            ShowLearn(score);
            eatingText.text = "100%";

            InitButtons();
            GameManager.ScoreKeeper.ResetScore();
            print("lastWorkout = " + GameManager.SceneLoader.lastWorkout);
        }

        #endregion

        #region Private Methods

        private void CheckContinueWorkout(Button button)
        {
            float score = GameManager.ScoreKeeper.ScoreValue;
            int wordInGroupRemain = GameManager.WordManager.CountUntrainedWordInGroup();
            if (wordInGroupRemain > 0)
            {
                button.interactable = true;
                button.onClick.AddListener(() =>
                                               GameManager.Notifications.PostNotification(
                                                   this, GameEvents.ContinueWorkout)
                );
                button.gameObject.SetActive(true);
            } else
            {
                button.interactable = false;
                button.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Получить количество изученных слов.
        /// </summary>
        /// <param name="score"></param>
        /// <returns></returns>
        private String GetStudiedWordsCount(float score)
        {
            if (score == 0 || score > 4)
                return string.Format("{0} слов изучено, ", score);
            if (score == 1)
                return string.Format("{0} слово изучено, ", score);

            return string.Format("{0} слова изучено, ", score);
        }

        /// <summary>
        /// Получить количество оставшихся слов на изучении.
        /// </summary>
        /// <param name="score"></param>
        private String GetWordsCountLeft()
        {
            int wordsLeft = GameManager.WordManager.CountUntrainedWordInGroup();
            return string.Format("{0} на изучении", wordsLeft);
        }

        private void InitButtons()
        {
            buttons = transform.GetComponentsInChildren<Button>();
            foreach (var button in buttons)
            {
                switch (button.name)
                {
                    case "NextWorkoutButton":
                        //item.onClick.AddListener();
                        break;
                    case "ListWorkoutButton":
                        button.onClick.AddListener(() =>
                                                       GameManager.SceneLoader.LoadLevel(SceneNames.trainingСhoice)
                        );
                        break;
                    case "ContinueWorkoutButton":
                        CheckContinueWorkout(button);
                        break;
                }
            }
        }

        private void ShowCaption(float score)
        {
            if (score == BedResult
            ) { captionText.text = "В этот раз не получилось, но продолжай тренироваться!"; } else if (
                score < GoodResult) { captionText.text = "Неплохо, но есть над чем поработать."; } else if (
                score == GoodResult) { captionText.text = "Круто, отличный результат!"; } else if (score == BestResult
            ) { captionText.text = "Поздравляем, отличный результат!"; }
        }

        private void ShowLearn(float score)
        {
            if (score < 0)
            {
                Debug.LogError("score = " + score);
                score = 0;
            }

            var resultText = GetStudiedWordsCount(score);
            resultText += GetWordsCountLeft();
            learnText.text = resultText;
        }


        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKey(KeyCode.A))
            {
                Debug.Log("Input.GetKey");
                ShowCaption(GameManager.ScoreKeeper.ScoreValue);
                ShowLearn(GameManager.ScoreKeeper.ScoreValue);
            }
        }

        #endregion
    }
}

/*
1:
        2:
        3:

  0 - В этот раз не получилось, но продолжай тренироваться! 
  1 - Неплохо, но есть над чем поработать
  9 - Круто, отличный результат! 
 10 - Поздравляем, отличный результат!
*/
