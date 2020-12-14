using System.Collections.Generic;
using LinguaLeo.Scripts.Helpers;
using LinguaLeo.Scripts.Helpers.Interfaces;
using LinguaLeo.Scripts.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LinguaLeo.Scripts.Workout
{
    public class WordToTranslate : AbstractWorkout, IObserver
    {
        #region SerializeFields

        [SerializeField]
        private bool isReverse = false;

        [SerializeField]
        private Image progressImage = null; // Картинка прогесса изучения слова

        [SerializeField]
        private Toggle sayToggle = null; // checkbox для автопроизношения

        [SerializeField]
        private Slider scoreSlider = null; // Протренировано слов прогресс

        [SerializeField]
        private Text scoreText = null; // Текст числа протренированых слов

        [SerializeField]
        private int answersCount; // Число протренированых слов

        [SerializeField]
        private Text contextText = null; //Текст для контекста

        #endregion

        #region Private variables

        private Text transcriptText = null; // Поле для транскрипции

        private GameObject repeatWordButton = null;

        private GameObject contextPanel; //Панель для контекста

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
                    if (isReverse && sayToggle.isOn)
                        GameManager.AudioPlayer.SayWord();
                    ShowImage();
                    ShowTranscript();
                    ShowRepeatWordButton();
                    WordProgressUpdate();
                    ShowContext();
                    core.SetNextQuestion();
                    break;
            }
        }

        #endregion

        #region Unity events

        // Use this for initialization
        private void Start()
        {
            GameManager.Notifications.AddListener(this, GameEvents.ShowResult);
            GameManager.Notifications.AddListener(this, GameEvents.CoreBuild);

            questionText = GameObject.Find("QuestionText").GetComponent<Text>();
            transcriptText = GameObject.Find("TranscriptText").GetComponent<Text>();
            wordImage = GameObject.Find("WordImage").GetComponent<Image>();

            contextPanel = contextText.transform.parent.gameObject;
            repeatWordButton = GameObject.Find("RepeatWordButton");

            if (repeatWordButton)
            {
                repeatWordButton.GetComponent<Button>().onClick.AddListener(
                    () => GameManager.AudioPlayer.SayWord());
            }

            GameManager.Notifications.PostNotification(this, GameEvents.WorkoutLoaded);
        }

        #endregion

        #region Public Methods

        public QuestionLeo GetCurrentQuest()
        {
            return core.GetCurrentQuest();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Перевод-Слово
        /// </summary>
        private void BuildUiToTranslateWord()
        {
            QuestionLeo questionLeo = core.GetCurrentQuest();
            // добавление слова для перевода
            string translations = questionLeo.questWord.translations;
            string questionWord = translations.Split(',')[0];

            SetQuestion(questionWord);
            SetTranscript(questionLeo.questWord.transcription);

            SetButtons(questionLeo, questionLeo.questWord);

            SetImage(questionLeo.questWord.pictureUrl);

            core.SetSound(questionLeo.questWord.soundUrl);
            SetContext(questionLeo.questWord.highlightedContext);

            HideImage(); //ShowImage();
            HideContext();
            HideTranscript();
            HideRepeatWordButton();
        }

        /// <summary>
        /// Слово-Перевод
        /// </summary>
        private void BuildUiToWordTranslate()
        {
            QuestionLeo questionLeo = core.GetCurrentQuest();

            // добавление слова для перевода
            SetQuestion(questionLeo.questWord.wordValue);
            SetTranscript(questionLeo.questWord.transcription);

            SetButtons(questionLeo, questionLeo.questWord);

            SetImage(questionLeo.questWord.pictureUrl);
            HideImage();

            core.SetSound(questionLeo.questWord.soundUrl);
            if (sayToggle.isOn)
                GameManager.AudioPlayer.SayWord();

            SetContext(questionLeo.questWord.highlightedContext);
            HideContext();
        }

        private void Core_DrawTask()
        {
            if (isReverse)
                BuildUiToTranslateWord();
            else
                BuildUiToWordTranslate();

            WordProgressUpdate();
            WorkoutProgeressUpdate();
            FindObjectOfType<DebugUi>().FillPanel(core.tasks);

            // выбор окна диалога как активного, чтобы снять выделение с кнопок диалога
            EventSystem.current.SetSelectedGameObject(gameObject);
            GameManager.Notifications.PostNotification(this, GameEvents.BuildTask);
        }

        private void HideContext()
        {
            contextPanel.SetActive(false);
        }

        private void HideRepeatWordButton()
        {
            repeatWordButton.SetActive(false);
        }

        private void HideTranscript()
        {
            transcriptText.enabled = false;
        }

        private void InitWordCountBar()
        {
            if (core.maxQuestCount < GameManager.WordManager.CountWordInGroup())
                scoreSlider.maxValue = core.maxQuestCount;
            else
                scoreSlider.maxValue = GameManager.WordManager.CountWordInGroup();
            scoreText.text = answersCount + "/" + scoreSlider.maxValue;
        }

        /// <summary>
        /// Заполнить кнопки вариантами ответов
        /// </summary>
        /// <param name="questionLeo"></param>
        /// <param name="questionWord"></param>
        private void SetButtons(QuestionLeo questionLeo, WordLeo questionWord)
        {
            List<string> answers = new List<string>(Workout.ANSWER_COUNT);
            if (isReverse)
            {
                foreach (WordLeo item in questionLeo.answers)
                    answers.Add(item.wordValue);

                core.SetButtons(answers, questionWord.wordValue);
            } else
            {
                foreach (WordLeo item in questionLeo.answers)
                    answers.Add(item.translations);

                core.SetButtons(answers, questionWord.translations);
            }
        }

        private void SetContext(string context)
        {
            if (context != string.Empty)
                contextText.text = context;
            else
                contextText.text = "(нет контекста)";
        }

        private void SetQuestion(string quest)
        {
            questionText.text = quest;
        }

        private void SetTranscript(string transcript)
        {
            transcriptText.text = transcript;
        }

        private void ShowContext()
        {
            contextPanel.SetActive(true);
        }

        private void ShowRepeatWordButton()
        {
            repeatWordButton.SetActive(true);
        }

        private void ShowTranscript()
        {
            transcriptText.enabled = true;
        }

        /// <summary>
        /// показывает прогресс изучения слова
        /// </summary>
        private void WordProgressUpdate()
        {
            progressImage.fillAmount = GetCurrentQuest().questWord.GetProgressCount();
        }

        /// <summary>
        /// Обновить шкалу колличества пройденых слов
        /// </summary>
        private void WorkoutProgeressUpdate()
        {
            answersCount++;
            scoreText.text = answersCount + "/" + scoreSlider.maxValue;
            scoreSlider.value = answersCount;
        }

        #endregion
    }
}
