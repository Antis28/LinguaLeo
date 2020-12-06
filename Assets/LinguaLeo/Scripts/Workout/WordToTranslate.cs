using System.Collections.Generic;
using LinguaLeo.Scripts.Helpers;
using LinguaLeo.Scripts.Helpers.Interfaces;
using LinguaLeo.Scripts.Helpers.ResourceLoading;
using LinguaLeo.Scripts.Manegers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LinguaLeo.Scripts.Workout
{
    public class WordToTranslate : MonoBehaviour, IObserver, IWorkout
    {
        [SerializeField]
        private WorkoutNames WorkoutName = WorkoutNames.WordTranslate;

        [SerializeField]
        private bool isReverse = false;

        WorkoutNames IWorkout.WorkoutName
        {
            get
            {
                return WorkoutName;
            }
        }

        private Text questionText = null; // Поле для вопроса
        private Text transcriptText = null; // Поле для транскрипции
        private Image wordImage = null; // Картинка ассоциаци со словом

        private GameObject RepeatWordButton = null;

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
        private GameObject contextPanel; //Панель для контекста


        private Workout core;

        // Use this for initialization
        void Start()
        {
            GameManager.Notifications.AddListener(this, GAME_EVENTS.ShowResult);
            GameManager.Notifications.AddListener(this, GAME_EVENTS.CoreBuild);

            questionText = GameObject.Find("QuestionText").GetComponent<Text>();
            transcriptText = GameObject.Find("TranscriptText").GetComponent<Text>();
            wordImage = GameObject.Find("WordImage").GetComponent<Image>();

            contextPanel = contextText.transform.parent.gameObject;
            RepeatWordButton = GameObject.Find("RepeatWordButton");

            if (RepeatWordButton)
            {
                RepeatWordButton.GetComponent<Button>().onClick.AddListener(
                    () => GameManager.AudioPlayer.SayWord());
            }

            GameManager.Notifications.PostNotification(this, GAME_EVENTS.WorkoutLoaded);
        }

        private void Core_DrawTask()
        {
            if (isReverse)
                BuildUiToTranslateWord();
            else
                BuildUiToWordTranslate();

            WordProgressUpdate();
            WorkoutProgeressUpdate();
            GameObject.FindObjectOfType<DebugUI>().FillPanel(core.tasks);

            // выбор окна диалога как активного, чтобы снять выделение с кнопок диалога
            EventSystem.current.SetSelectedGameObject(this.gameObject);
            GameManager.Notifications.PostNotification(this, GAME_EVENTS.BuildTask);
        }

        void IObserver.OnNotify(object parametr, GAME_EVENTS notificationName)
        {
            switch (notificationName)
            {
                case GAME_EVENTS.CoreBuild:
                    core = parametr as Workout;
                    core.buttonsHandler = GameObject.FindObjectOfType<ButtonsHandler>();
                    core.DrawTask += Core_DrawTask;
                    core.BuildFirstTask();
                    InitWordCountBar();
                    //FindObjectOfType<DebugUI>().FillPanel(questions);
                    break;
                case GAME_EVENTS.ShowResult:
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

        private void InitWordCountBar()
        {
            if (core.maxQuestCount < GameManager.WordManeger.CountWordInGroup())
                scoreSlider.maxValue = core.maxQuestCount;
            else
                scoreSlider.maxValue = GameManager.WordManeger.CountWordInGroup();
            scoreText.text = answersCount + "/" + scoreSlider.maxValue;
        }

        private void HideRepeatWordButton()
        {
            RepeatWordButton.SetActive(false);
        }

        private void HideTranscript()
        {
            transcriptText.enabled = false;
        }
        private void ShowRepeatWordButton()
        {
            RepeatWordButton.SetActive(true);
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

        private void SetQuestion(string quest)
        {
            questionText.text = quest;
        }
        private void SetTranscript(string transcript)
        {
            transcriptText.text = transcript;
        }

        private void HideImage()
        {
            wordImage.enabled = false;
        }
        private void ShowImage()
        {
            wordImage.enabled = true;
        }

        private void SetImage(string fileName)
        {
            var sprite = SpriteLoader.GetSpriteFromPicture(fileName);

            wordImage.sprite = sprite;
            wordImage.preserveAspect = true;
        }

        private void SetContext(string context)
        {
            if (context != string.Empty)
                contextText.text = context;
            else
                contextText.text = "(нет контекста)";
        }

        private void ShowContext()
        {
            contextPanel.SetActive(true);
        }
        private void HideContext()
        {
            contextPanel.SetActive(false);
        }

        public QuestionLeo GetCurrentQuest()
        {
            return core.GetCurrentQuest();
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

            SetImage(questionLeo.questWord.pictureURL);
            HideImage();

            core.SetSound(questionLeo.questWord.soundURL);
            if (sayToggle.isOn)
                GameManager.AudioPlayer.SayWord();

            SetContext(questionLeo.questWord.highlightedContext);
            HideContext();
        }

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

            SetImage(questionLeo.questWord.pictureURL);

            core.SetSound(questionLeo.questWord.soundURL);
            SetContext(questionLeo.questWord.highlightedContext);

            HideImage();  //ShowImage();
            HideContext();
            HideTranscript();
            HideRepeatWordButton();
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
            }
            else
            {
                foreach (WordLeo item in questionLeo.answers)
                    answers.Add(item.translations);

                core.SetButtons(answers, questionWord.translations);
            }
        }

        WordLeo IWorkout.GetCurrentWord()
        {
            return core.GetCurrentWord();
        }

        Workout IWorkout.GetCore()
        {
            return core;
        }
    }
}