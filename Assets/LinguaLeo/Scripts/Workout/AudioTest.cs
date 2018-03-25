using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AudioTest : MonoBehaviour, Observer, IWorkout
{
    [SerializeField]
    private WorkoutNames WorkoutName = WorkoutNames.WordTranslate;

    [SerializeField]
    private Text questionText = null; // Поле для вопроса
    [SerializeField]
    private Image wordImage = null; // Картинка ассоциаци со словом

    InputField AnswerInputField = null;

    private Toggle sayToggle = null; // checkbox для автопроизношения
    private Slider scoreSlider = null; // Протренировано слов прогресс
    private Text scoreText = null; // Текст числа протренированых слов
    [SerializeField]
    private int answersCount; // Число протренированых слов

    private Button RepeatWordButton = null;
    private Workout core;

    private Color correctColor = new Color(59 / 255f,
                                                152 / 255f,
                                                57 / 255f);
    private Color wrongColor = new Color(157 / 255f,
                                                38 / 255f,
                                                29 / 255f);
    private bool isAnswerCorrect;

    WorkoutNames IWorkout.WorkoutName
    {
        get
        {
            return WorkoutName;
        }
    }

    Workout IWorkout.GetCore()
    {
        throw new NotImplementedException();
    }

    WordLeo IWorkout.GetCurrentWord()
    {
        return core.GetCurrentWord();
    }

    void Observer.OnNotify(object parametr, GAME_EVENTS notificationName)
    {
        switch (notificationName)
        {
            case GAME_EVENTS.CoreBuild:
                
                core = parametr as Workout;
                core.buttonsHandler = GameObject.FindObjectOfType<ButtonsHandler>();
                core.DrawTask += Core_DrawTask;
                core.BuildTask(0);
                InitWordCountBar();
                //FindObjectOfType<DebugUI>().FillPanel(questions);
                break;
            case GAME_EVENTS.ShowResult:
                ShowImage();
                WordProgressUpdate();
                core.RunNextQuestion();
                break;
        }
    }

    public QuestionLeo GetCurrentQuest()
    {
        return core.GetCurrentQuest();
    }

    public void CheckAnswer()
    {
        isAnswerCorrect = AnswerInputField.text == core.GetCurrentWord().wordValue;
        //ShowQuestion();
        questionText.text = core.GetCurrentWord().wordValue;
        if (isAnswerCorrect)
        {
            GameManager.Notifications.PostNotification(this, GAME_EVENTS.CorrectAnswer);
            questionText.color = correctColor;
        }
        else
            questionText.color = wrongColor;
        GameManager.Notifications.PostNotification(null, GAME_EVENTS.ShowResult);

    }

    // Use this for initialization
    void Start()
    {
        RepeatWordButton = GameObject.Find("RepeatWordButton").GetComponent<Button>();
        sayToggle = GameObject.Find("AutoSoundToggle").GetComponent<Toggle>();
        scoreSlider = GameObject.Find("ScoreSlider").GetComponent<Slider>();
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        AnswerInputField = GameObject.Find("AnswerInputField").GetComponent<InputField>();

        GameManager.Notifications.AddListener(this, GAME_EVENTS.CoreBuild);
        GameManager.Notifications.AddListener(this, GAME_EVENTS.ShowResult);

        if (RepeatWordButton)
            RepeatWordButton.onClick.AddListener(() => GameManager.AudioPlayer.SayWord());

        GameManager.Notifications.PostNotification(null, GAME_EVENTS.ButtonHandlerLoaded);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Core_DrawTask()
    {
        Debug.Log("Core_DrawTask");

        QuestionLeo questionLeo = core.GetCurrentQuest();
        core.SetSound(questionLeo.questWord.soundURL);
        SetImage(questionLeo.questWord.pictureURL);
        HideImage();
        HideQuestion();
        AnswerInputField.text = string.Empty;

        if (sayToggle.isOn)
            GameManager.AudioPlayer.SayWord();

        WordProgressUpdate();
        WorkoutProgeressUpdate();
        GameObject.FindObjectOfType<DebugUI>().FillPanel(core.tasks);

        // выбор окна диалога как активного, чтобы снять выделение с кнопок диалога
        EventSystem.current.SetSelectedGameObject(this.gameObject);
        GameManager.Notifications.PostNotification(this, GAME_EVENTS.BuildTask);
    }

    /// <summary>
    /// показывает прогресс изучения слова
    /// </summary>
    private void WordProgressUpdate()
    {
        WordLeo word = GetCurrentQuest().questWord;
        float progress = 0;

        if (word.progress.word_translate)
        {
            progress += 0.25f;
        }
        if (word.progress.translate_word)
        {
            progress += 0.25f;
        }
        if (word.progress.audio_word)
        {
            progress += 0.25f;
        }
        if (word.progress.word_puzzle)
        {
            progress += 0.25f;
        }
        //progressImage.fillAmount = progress;
    }

    private void WorkoutProgeressUpdate()
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
    
    private void HideImage()
    {
        wordImage.enabled = false;
    }
    private void ShowImage()
    {
        wordImage.enabled = true;
    }

    private void HideQuestion()
    {
        questionText.gameObject.SetActive(false);
    }
    private void ShowQuestion()
    {
        questionText.gameObject.SetActive(true);
    }
    private void SetImage(string fileName)
    {
        string foloder = "Data/Picture/";
        //Sprite sprite = Resources.Load<Sprite>(foloder + "/" + Utilities.ConverterUrlToName(fileName));
        Sprite sprite = Utilities.LoadSpriteFromFile(foloder + Utilities.ConverterUrlToName(fileName));

        wordImage.sprite = sprite;
        wordImage.preserveAspect = true;
    }

    
}
