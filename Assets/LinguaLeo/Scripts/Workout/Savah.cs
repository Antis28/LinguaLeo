using System.Collections.Generic;
using LinguaLeo.Scripts.Helpers;
using LinguaLeo.Scripts.Helpers.Interfaces;
using LinguaLeo.Scripts.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace LinguaLeo.Scripts.Workout
{
    public class Savah : MonoBehaviour, IObserver, IWorkout
    {
        #region SerializeFields

        [FormerlySerializedAs("WorkoutName")]
        [SerializeField]
        private WorkoutNames workoutName = WorkoutNames.WordTranslate;

        //[SerializeField]
        //private int questCount = 30;

        [SerializeField]
        private Text questionText = null;

        #endregion

        #region Private variables

        WorkoutNames IWorkout.WorkoutName
        {
            get { return workoutName; }
        }


        private Workout core;

        #endregion

        #region Events

        void IObserver.OnNotify(object parametr, GameEvents notificationName)
        {
            switch (notificationName)
            {
                case GameEvents.CoreBuild:
                    core = parametr as Workout;
                    core.buttonsHandler = FindObjectOfType<ButtonsHandler>();
                    core.DrawTask += Core_DrawTask;
                    core.BuildFirstTask();
                    //FindObjectOfType<DebugUI>().FillPanel(questions);
                    break;
                case GameEvents.ShowResult:
                    core.RunNextQuestion();
                    break;
            }
        }

        #endregion

        #region Unity events

        // Use this for initialization
        private void Start()
        {
            GameManager.Notifications.AddListener(this, GameEvents.CoreBuild);
            GameManager.Notifications.AddListener(this, GameEvents.ShowResult);
        }

        #endregion

        #region Public Methods

        public Workout GetCore()
        {
            return core.GetCore();
        }

        public QuestionLeo GetCurrentQuest()
        {
            return core.GetCurrentQuest();
        }

        #endregion

        #region Private Methods

        private void Core_DrawTask()
        {
            QuestionLeo questionLeo = core.GetCurrentQuest();

            // добавление слова для перевода
            SetQuestion(questionLeo.questWord.wordValue);
            SetButtons(questionLeo, questionLeo.questWord);

            //Выше заполнить GUI
            ResetSelection();
        }

        WordLeo IWorkout.GetCurrentWord()
        {
            return core.GetCurrentWord();
        }

        private void ResetSelection()
        {
            // выбор окна диалога как активного, чтобы снять выделение с кнопок диалога
            EventSystem.current.SetSelectedGameObject(gameObject);
            GameManager.Notifications.PostNotification(this, GameEvents.BuildTask);
        }

        /// <summary>
        /// Заполнить кнопки вариантами ответов
        /// </summary>
        /// <param name="questionLeo"></param>
        /// <param name="questionWord"></param>
        private void SetButtons(QuestionLeo questionLeo, WordLeo questionWord)
        {
            List<string> answers = new List<string>(Workout.ANSWER_COUNT);

            foreach (WordLeo item in questionLeo.answers)
                answers.Add(item.translations);

            core.SetButtons(answers, questionWord.translations);
        }

        private void SetQuestion(string quest)
        {
            questionText.text = quest;
        }

        #endregion
    }
}
