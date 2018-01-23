using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using URandom = UnityEngine.Random;

public class WordToTranslate : MonoBehaviour, Observer
{
    [SerializeField]
    private Text questionText = null; // Поле для вопроса

    [SerializeField]
    private Text transcriptText = null; // Поле для транскрипции

    [SerializeField]
    private Image wordImage = null; // Картинка ассоциаци со словом

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

    private List<QuestionLeo> questions;
    private int questionID;

    private const int QUEST_COUNT = 10;
    private const int ANSWER_COUNT = 5;

    private bool trainingСompleted;

    private ButtonsHandler buttonsHandler;


    // Use this for initialization
    void Awake()
    {
        GameManager.Notifications.AddListener(this, GAME_EVENTS.ShowResult);
        GameManager.Notifications.AddListener(this, GAME_EVENTS.BuildTask);
        GameManager.Notifications.AddListener(this, GAME_EVENTS.LoadedVocabulary);

        contextPanel = contextText.transform.parent.gameObject;
        buttonsHandler = FindObjectOfType<ButtonsHandler>();
    }

    void Observer.OnNotify(Component sender, GAME_EVENTS notificationName)
    {
        switch (notificationName)
        {
            case GAME_EVENTS.BuildTask:
                ProgeressUpdate();
                HideImage();
                break;
            case GAME_EVENTS.LoadedVocabulary:
                LoadTasks(WordManeger.Vocabulary);
                BuildTask(0);
                break;
            case GAME_EVENTS.ShowResult:
                ShowContext();
                buttonsHandler.SetNextQuestion(questions[questionID].answers,
                                                () => BuildTask(questionID + 1));
                break;

        }
    }
    private void ProgeressUpdate()
    {
        answersCount++;
        scoreText.text = answersCount + "/" + scoreSlider.maxValue;
        scoreSlider.value = answersCount;
    }

    public void SetQuestion(string quest)
    {
        questionText.text = quest;
    }
    public void SetTranscript(string transcript)
    {
        transcriptText.text = transcript;
    }

    public void ClearTextInQestion()
    {
        questionText.text = "";
    }

    public void HideImage()
    {
        wordImage.sprite = null;
    }

    public void SetImage(string fileName)
    {
        string foloder = "!Pict";
        Sprite sprite = Resources.Load<Sprite>(foloder + "/" + Utilities.ConverterUrlToName(fileName));
        wordImage.sprite = sprite;
        wordImage.preserveAspect = true;
    }

    public void SetSound(string file)
    {
        GameManager.AudioPlayer.SetSound(Utilities.ConverterUrlToName(file));
        if (sayToggle.isOn)
            GameManager.AudioPlayer.SayWord();
    }

    public void SetContext(string context)
    {
        if (context != string.Empty)
            contextText.text = context;
        else
            contextText.text = "(нет контекста)";
    }

    public void ShowContext()
    {
        contextPanel.SetActive(true);
    }
    public void HideContext()
    {
        contextPanel.SetActive(false);
    }


    public QuestionLeo GetCurrentQuest()
    {
        return questions[questionID];
    }

    private void LoadTasks(WordCollection words)
    {
        questions = new List<QuestionLeo>(QUEST_COUNT);
        for (int i = 0; i < QUEST_COUNT; i++)
        {
            QuestionLeo question = GeneratorTask(i, words);
            questions.Add(question);
        }
    }

    private void BuildTask(int current)
    {
        buttonsHandler.ClearTextInButtons();
        if (trainingСompleted)
        {
            GameManager.Notifications.PostNotification(this, GAME_EVENTS.WordsEnded);
            return;
        }

        questionID = FindNodeByID(current);
        if (questionID < 0)
        {
            Debug.LogError(this + "отсутствует или указан неверно идентификатор узла.");
            return;
        }

        GameManager.Notifications.PostNotification(this, GAME_EVENTS.BuildTask);
        int toNode = questionID + 1;
        if (QUEST_COUNT == toNode)
        {
            toNode = 0;
            trainingСompleted = true;
        }

        // добавление слова для перевода
        string questionWord = questions[questionID].questWord.wordValue;
        SetQuestion(questionWord);
       SetTranscript(questions[questionID].questWord.transcription);

        //TODO: заполнять все кнопки одновременно
        buttonsHandler.FillingButtonsWithOptions(questions[questionID].answers, questionWord);
        buttonsHandler.FillingEnterButton(true);

        SetImage(questions[questionID].questWord.pictureURL);
        SetSound(questions[questionID].questWord.soundURL);
        SetContext(questions[questionID].questWord.highlightedContext);
        HideContext();

        // выбор окна диалога как активного, чтобы снять выделение с кнопок диалога
        EventSystem.current.SetSelectedGameObject(this.gameObject);
    }

    private QuestionLeo GeneratorTask(int id, WordCollection words)
    {
        QuestionLeo questionLeo = new QuestionLeo();
        questionLeo.id = id;

        questionLeo.answers = words.GetRandomWords(ANSWER_COUNT);

        int indexOfQuestWord = URandom.Range(0, ANSWER_COUNT);
        questionLeo.questWord = questionLeo.answers[indexOfQuestWord];

        return questionLeo;
    }

    /// <summary>
    /// Поиск вопроса по ID 
    /// </summary>
    /// <param name="i">ID для поиска</param>
    /// <returns></returns>
    int FindNodeByID(int i)
    {
        int j = 0;
        foreach (var quest in questions)
        {
            if (quest.id == i)
                return j;
            j++;
        }

        return -1;
    }

    
}