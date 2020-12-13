using LinguaLeo.Scripts.Helpers;
using LinguaLeo.Scripts.Helpers.Interfaces;
using LinguaLeo.Scripts.Manegers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LinguaLeo.Scripts.Workout
{
    public class AudioTest : AbstractWorkout, IObserver
    {
        private event UnityAction EnterButtonEvent;

        [SerializeField]
        private Image progressImage; // Картинка прогресса
       
        [SerializeField]
        private Button checkButton = null;

        private InputField answerInputField = null;

        private Toggle sayToggle = null; // checkbox для автопроизношения
        private Slider scoreSlider = null; // Протренировано слов прогресс
        private Text scoreText = null; // Текст числа протренированых слов
        [SerializeField]
        private int answersCount; // Число протренированых слов

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
                    SetupEnterButton(core.RunNextQuestion);
                    CheckAnswer();
                    ShowQuestion();
                    ShowImage();
                    WordProgressUpdate();

                    break;
            }
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

        public QuestionLeo GetCurrentQuest()
        {
            return core.GetCurrentQuest();
        }

        public void CheckAnswerClick()
        {
            GameManager.Notifications.PostNotification(null, GAME_EVENTS.ShowResult);
        }
        private void CheckAnswer()
        {
            //answerInputField.text = answerInputField.text.Replace("'", "’");
            isAnswerCorrect = answerInputField.text == core.GetCurrentWord().wordValue;
            questionText.text = core.GetCurrentWord().wordValue;
            translateText.text = core.GetCurrentWord().translations;
            if (isAnswerCorrect)
            {
                GameManager.Notifications.PostNotification(this, GAME_EVENTS.CorrectAnswer);
                questionText.color = correctColor;
                mistakeText.text = string.Empty;
            }
            else {
                mistakeText.text = answerInputField.text;
                mistakeText.color = wrongColor;
            }
        }

        private void Core_DrawTask()
        {
            Debug.Log("Core_DrawTask");

            QuestionLeo questionLeo = core.GetCurrentQuest();
            core.SetSound(questionLeo.questWord.soundUrl);
            SetImage(questionLeo.questWord.pictureUrl);
            HideImage();
            HideQuestion();
            answerInputField.text = string.Empty;

            SetupEnterButton(CheckAnswerClick);

            if (sayToggle.isOn)
                GameManager.AudioPlayer.SayWord();

            WordProgressUpdate();
            ProgressBarUpdate();
            GameObject.FindObjectOfType<DebugUI>().FillPanel(core.tasks);

            //передать фокус полю ввода
            answerInputField.ActivateInputField();

            // выбор элемента как активного
            EventSystem.current.SetSelectedGameObject(answerInputField.gameObject);
            GameManager.Notifications.PostNotification(this, GAME_EVENTS.BuildTask);
        }

        /// <summary>
        /// показывает прогресс изучения слова
        /// </summary>
        private void WordProgressUpdate()
        {
            progressImage.fillAmount = GetCurrentQuest().questWord.GetProgressCount();
        }

        private void ProgressBarUpdate()
        {
            answersCount++;
            scoreText.text = answersCount + "/" + scoreSlider.maxValue;
            scoreSlider.value = answersCount;
        }

        private void InitWordCountBar()
        {
            if (core.maxQuestCount < GameManager.WordManeger.CountWordInGroup())
                scoreSlider.maxValue = core.maxQuestCount;
            else
                scoreSlider.maxValue = GameManager.WordManeger.CountWordInGroup();
            scoreText.text = answersCount + "/" + scoreSlider.maxValue;
        }

        private void HideQuestion()
        {
            questionText.gameObject.SetActive(false);
            mistakeText.gameObject.SetActive(false);
            translateText.gameObject.SetActive(false);
            answerInputField.gameObject.SetActive(true);
        }
        private void ShowQuestion()
        {
            questionText.gameObject.SetActive(true);
            mistakeText.gameObject.SetActive(true);
            translateText.gameObject.SetActive(true);
            answerInputField.gameObject.SetActive(false);
        }
        
        // Use this for initialization
        void Start()
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

            answerInputField = GameObject.Find("answerInputField").GetComponent<InputField>();

            GameManager.Notifications.AddListener(this, GAME_EVENTS.CoreBuild);
            GameManager.Notifications.AddListener(this, GAME_EVENTS.ShowResult);

            if (repeatWordButton)
                repeatWordButton.onClick.AddListener(
                    () =>
                    {
                        GameManager.AudioPlayer.SayWord();
                        //передать фокус полю ввода
                        answerInputField.ActivateInputField();
                    });

            GameManager.Notifications.PostNotification(null, GAME_EVENTS.ButtonHandlerLoaded);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyUp(KeyCode.Return))
            {
                EnterButtonEvent();
            }
        }

    }
}
