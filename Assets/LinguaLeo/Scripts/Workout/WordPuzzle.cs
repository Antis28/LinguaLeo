﻿using LinguaLeo.Scripts.Helpers;
using LinguaLeo.Scripts.Helpers.Interfaces;
using LinguaLeo.Scripts.Managers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LinguaLeo.Scripts.Workout
{
    public class WordPuzzle : AbstractWorkout, IObserver
    {
        #region SerializeFields

        [SerializeField]
        private Image progressImage; // Картинка прогресса

        [SerializeField]
        private Button checkButton = null;

        [SerializeField]
        private int answersCount; // Число протренированых слов

        #endregion

        #region Private variables

        private InputField answerInputField = null;

        private Toggle sayToggle = null;   // checkbox для автопроизношения
        private Slider scoreSlider = null; // Протренировано слов прогресс
        private Text scoreText = null;     // Текст числа протренированых слов

        private Button repeatWordButton = null;

        private Color correctColor = new Color(59 / 255f,
                                               152 / 255f,
                                               57 / 255f);

        private Color wrongColor = new Color(157 / 255f,
                                             38 / 255f,
                                             29 / 255f);

        private bool isAnswerCorrect;
        private Text mistakeText;
        private Text translateText;

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
                    InitWordCountBar();
                    //FindObjectOfType<DebugUI>().FillPanel(questions);
                    break;
                case GameEvents.ShowResult:
                    SetupEnterButton(core.RunNextQuestion);
                    CheckAnswer();
                    ShowQuestion();
                    ShowImage();
                    WordProgressUpdate();

                    break;
            }
        }

        #endregion

        #region Unity events

        // Use this for initialization
        private void Start()
        {
            repeatWordButton = GameObject.Find("RepeatWordButton").GetComponent<Button>();
            checkButton = GameObject.Find("CheckButton").GetComponent<Button>();

            sayToggle = GameObject.Find("AutoSoundToggle").GetComponent<Toggle>();
            scoreSlider = GameObject.Find("ScoreSlider").GetComponent<Slider>();

            progressImage = GameObject.Find("ProgressImage").GetComponent<Image>();
            wordImage = GameObject.Find("WordImage").GetComponent<Image>();

            scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
            mistakeText = GameObject.Find("MistakeText").GetComponent<Text>();
            translateText = GameObject.Find("TranslateText").GetComponent<Text>();

            answerInputField = GameObject.Find("AnswerInputField").GetComponent<InputField>();

            GameManager.Notifications.AddListener(this, GameEvents.CoreBuild);
            GameManager.Notifications.AddListener(this, GameEvents.ShowResult);

            if (repeatWordButton)
                repeatWordButton.onClick.AddListener(
                    () =>
                    {
                        GameManager.AudioPlayer.SayWord();
                        //передать фокус полю ввода
                        answerInputField.ActivateInputField();
                    });

            GameManager.Notifications.PostNotification(null, GameEvents.ButtonHandlerLoaded);
        }

        #endregion

        #region Public Methods

        public QuestionLeo GetCurrentQuest()
        {
            return core.GetCurrentQuest();
        }

        #endregion

        #region Private Methods

        private void CheckAnswer()
        {
            if (sayToggle.isOn)
                GameManager.AudioPlayer.SayWord();

            ShowRepeatWordButton();

            //AnswerInputField.text = AnswerInputField.text.Replace("'", "’");
            isAnswerCorrect = answerInputField.text == core.GetCurrentWord().wordValue;
            questionText.text = core.GetCurrentWord().wordValue;

            if (isAnswerCorrect)
            {
                GameManager.Notifications.PostNotification(this, GameEvents.CorrectAnswer);
                questionText.color = correctColor;
                mistakeText.text = string.Empty;
            } else
            {
                mistakeText.text = answerInputField.text;
                mistakeText.color = wrongColor;
            }
        }

        private void CheckAnswerClick()
        {
            GameManager.Notifications.PostNotification(null, GameEvents.ShowResult);
        }

        private void Core_DrawTask()
        {
            Debug.Log("Core_DrawTask");

            QuestionLeo questionLeo = core.GetCurrentQuest();
            translateText.text = core.GetCurrentWord().translations;
            core.SetSound(questionLeo.questWord.soundUrl);
            SetImage(questionLeo.questWord.pictureUrl);
            HideImage();
            HideQuestion();
            HideRepeatWordButton();
            answerInputField.text = string.Empty;

            SetupEnterButton(CheckAnswerClick);

            WordProgressUpdate();
            ProgressBarUpdate();
            FindObjectOfType<DebugUi>().FillPanel(core.tasks);

            //передать фокус полю ввода
            answerInputField.ActivateInputField();

            // выбор элемента как активного
            EventSystem.current.SetSelectedGameObject(answerInputField.gameObject);
            GameManager.Notifications.PostNotification(this, GameEvents.BuildTask);
        }

        private void HideQuestion()
        {
            questionText.gameObject.SetActive(false);
            mistakeText.gameObject.SetActive(false);
            //translateText.gameObject.SetActive(false);
            answerInputField.gameObject.SetActive(true);
        }

        private void HideRepeatWordButton()
        {
            repeatWordButton.gameObject.SetActive(false);
        }

        private void InitWordCountBar()
        {
            if (core.maxQuestCount < GameManager.WordManager.CountWordInGroup())
                scoreSlider.maxValue = core.maxQuestCount;
            else
                scoreSlider.maxValue = GameManager.WordManager.CountWordInGroup();
            scoreText.text = answersCount + "/" + scoreSlider.maxValue;
        }

        private void ProgressBarUpdate()
        {
            answersCount++;
            scoreText.text = answersCount + "/" + scoreSlider.maxValue;
            scoreSlider.value = answersCount;
        }

        private void SetupEnterButton(UnityAction action)
        {
            // очистить ui кнопку от подписчиков
            checkButton.onClick.RemoveAllListeners();
            // очистить кнопку return от подписчиков
            EnterButtonEvent = null;

            EnterButtonEvent += action;
            checkButton.onClick.AddListener(action);
        }

        private void ShowQuestion()
        {
            questionText.gameObject.SetActive(true);
            mistakeText.gameObject.SetActive(true);
            //translateText.gameObject.SetActive(true);
            answerInputField.gameObject.SetActive(false);
        }

        private void ShowRepeatWordButton()
        {
            repeatWordButton.gameObject.SetActive(true);
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Return)) { EnterButtonEvent(); }
        }

        /// <summary>
        /// показывает прогресс изучения слова
        /// </summary>
        private void WordProgressUpdate()
        {
            progressImage.fillAmount = GetCurrentQuest().questWord.GetProgressCount();
        }

        #endregion

        private event UnityAction EnterButtonEvent;
    }
}
