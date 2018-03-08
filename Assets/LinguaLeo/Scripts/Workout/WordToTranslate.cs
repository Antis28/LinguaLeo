using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using URandom = UnityEngine.Random;

public class WordToTranslate : MonoBehaviour, Observer, IWorkout
{
    public bool isReverse = false;

    [SerializeField]
    private Text questionText = null; // Поле для вопроса

    [SerializeField]
    private Text transcriptText = null; // Поле для транскрипции

    [SerializeField]
    private Image wordImage = null; // Картинка ассоциаци со словом
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

    private List<QuestionLeo> questions;
    private int questionID;

    private const int QUEST_COUNT = 10;
    private const int ANSWER_COUNT = 5;

    private bool trainingСompleted;

    private ButtonsHandler buttonsHandler;

    List<WordLeo> untrainedWords;

    WorkoutNames IWorkout.WorkoutName
    {
        get
        {
            return WorkoutNames.translate;
            //return "reverse";
            //return "audio";
            //return "puzzle";
        }
    }


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
                WordProgressUpdate();
                ProgeressUpdate();
                break;
            case GAME_EVENTS.LoadedVocabulary:
                LoadTasks();
                BuildTask(0);
                FindObjectOfType<DebugUI>().FillPanel(questions);
                break;
            case GAME_EVENTS.ShowResult:
                if (isReverse && sayToggle.isOn)
                    GameManager.AudioPlayer.SayWord();
                ShowImage();
                WordProgressUpdate();
                ShowContext();
                buttonsHandler.SetNextQuestion(questions[questionID].answers,
                                                () => BuildTask(questionID + 1));
                break;

        }
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
        progressImage.fillAmount = progress;
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
        wordImage.enabled = false;
    }
    public void ShowImage()
    {
        wordImage.enabled = true;
    }

    public void SetImage(string fileName)
    {
        string foloder = "Data/Picture/";
        //Sprite sprite = Resources.Load<Sprite>(foloder + "/" + Utilities.ConverterUrlToName(fileName));
        Sprite sprite = Utilities.LoadSpriteFromFile(foloder + Utilities.ConverterUrlToName(fileName));

        wordImage.sprite = sprite;
        wordImage.preserveAspect = true;
    }

    public void SetSound(string file)
    {
        GameManager.AudioPlayer.SetSound(Utilities.ConverterUrlToName(file, false));
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
    private void HideContext()
    {
        contextPanel.SetActive(false);
    }


    public QuestionLeo GetCurrentQuest()
    {
        return questions[questionID];
    }

    private void LoadTasks()
    {
        questions = new List<QuestionLeo>(QUEST_COUNT);
        untrainedWords = GameManager.WordManeger.GetUntrainedGroupWords();

        for (int i = 0; i < QUEST_COUNT; i++)
        {
            untrainedWords = ShuffleList(untrainedWords);
            QuestionLeo question = GeneratorTask(i, questions);

            if (question == null)
                break;
            questions.Add(question);
        }
    }

    private void BuildTask(int current)
    {
        buttonsHandler.ClearTextInButtons();

        if (!AvaiableBuilding(current))
        {
            GameManager.Notifications.PostNotification(this, GAME_EVENTS.WordsEnded);
            return;
        }

        trainingСompleted = CheckTrainingСompleted();
        if (isReverse)
            BuildUiToTranslateWord();
        else
            BuildUiToWordTranslate();

        // выбор окна диалога как активного, чтобы снять выделение с кнопок диалога
        EventSystem.current.SetSelectedGameObject(this.gameObject);
        GameManager.Notifications.PostNotification(this, GAME_EVENTS.BuildTask);
    }

    private bool CheckTrainingСompleted()
    {
        int toNode = questionID + 1;
        if (questions.Count <= toNode)
        {
            toNode = 0;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Слово-Перевод
    /// </summary>
    private void BuildUiToWordTranslate()
    {
        QuestionLeo questionLeo = questions[questionID];

        // добавление слова для перевода
        SetQuestion(questionLeo.questWord.wordValue);
        SetTranscript(questionLeo.questWord.transcription);

        FillingButtons(questionLeo, questionLeo.questWord);

        SetImage(questionLeo.questWord.pictureURL);
        HideImage();

        SetSound(questionLeo.questWord.soundURL);
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
        QuestionLeo questionLeo = questions[questionID];
        // добавление слова для перевода
        string translations = questionLeo.questWord.translations;
        string questionWord = translations.Split(',')[0];

        SetQuestion(questionWord);
        SetTranscript(questionLeo.questWord.transcription);

        FillingButtons(questionLeo, questionLeo.questWord);

        SetImage(questionLeo.questWord.pictureURL);
        ShowImage();

        SetSound(questionLeo.questWord.soundURL);
        SetContext(questionLeo.questWord.highlightedContext);
        //HideContext();
    }

    /// <summary>
    /// Заполнить кнопки вариантами ответов
    /// </summary>
    /// <param name="questionLeo"></param>
    /// <param name="questionWord"></param>
    private void FillingButtons(QuestionLeo questionLeo, WordLeo questionWord)
    {
        List<string> answers = new List<string>(ANSWER_COUNT);
        if (isReverse)
        {
            foreach (WordLeo item in questionLeo.answers)
                answers.Add(item.wordValue);

            buttonsHandler.FillingButtonsWithOptions(answers, questionWord.wordValue);
        }
        else
        {
            foreach (WordLeo item in questionLeo.answers)
                answers.Add(item.translations);

            buttonsHandler.FillingButtonsWithOptions(answers, questionWord.translations);
        }
        
        buttonsHandler.FillingEnterButton(true);
    }

    /// <summary>
    /// Построение задания доступно?
    /// </summary>
    /// <param name="current"></param>
    /// <returns></returns>
    private bool AvaiableBuilding(int current)
    {
        if (trainingСompleted || questions.Count == 0)
        {
            return false;
        }
        questionID = FindNodeByID(current);
        if (questionID < 0)
        {
            Debug.LogError(this + "отсутствует или указан неверно идентификатор узла.");
            return false;
        }

        return true;
    }

    private QuestionLeo GeneratorTask(int id, List<QuestionLeo> exceptWords)
    {
        QuestionLeo questionLeo = new QuestionLeo();
        questionLeo.id = id;

        //if (words.GroupExist())

        questionLeo.questWord = GetNewWord(exceptWords, untrainedWords);

        if (questionLeo.questWord == null)
        {
            Debug.LogWarning("Уникальных слов нет");
            return null;
        }

        FillInAnswers(questionLeo);

        return questionLeo;
    }
    /// <summary>
    /// Найти слово которого нет в списке
    /// </summary>
    /// <param name="exceptWords"></param>
    /// <param name="words"></param>
    /// <returns></returns>
    private static WordLeo GetNewWord(List<QuestionLeo> exceptWords, List<WordLeo> words)
    {
        foreach (var item in words)
        {
            if (!exceptWords.Contains(new QuestionLeo(item)))
            {
                return item;
            }
        }
        return null;
    }

    /// <summary>
    /// заполнит варианты ответов
    /// </summary>
    /// <param name="questionLeo"></param>
    private void FillInAnswers(QuestionLeo questionLeo)
    {
        int[] numAnswers = { 0, 1, 2, 3, 4 };
        int indexOfQuestWord = URandom.Range(0, ANSWER_COUNT);

        Stack<WordLeo> answers = FillRandomStack(GameManager.WordManeger.GetAllGroupWords(), ANSWER_COUNT);
        questionLeo.answers = new List<WordLeo>(ANSWER_COUNT);
        foreach (var item in numAnswers)
        {
            if (item == indexOfQuestWord)
            {
                questionLeo.answers.Add(questionLeo.questWord);
                continue;
            }
            if (answers.Peek() == questionLeo.questWord)
                answers.Pop();
            questionLeo.answers.Add(answers.Pop());
        }
        questionLeo.answers = ShuffleList(questionLeo.answers);
    }

    /// <summary>
    /// перемешать слова
    /// </summary>
    /// <param name="words"></param>
    /// <returns></returns>
    private List<WordLeo> ShuffleList(List<WordLeo> words)
    {
        List<WordLeo> list = new List<WordLeo>(words);

        System.Random random = new System.Random();

        // для избежания зацикливания установлен максимум попыток
        //int maxAttempts = words.Count * 2;
        //int numberAttempts = 0;

        //foreach (var item in words)
        //{
        //    int randomIndex = 0; 
        //    do
        //    {
        //        numberAttempts++;
        //        if (numberAttempts > maxAttempts)
        //            goto ToEndLoop;

        //        randomIndex = random.Next(words.Count); 
        //    } while (list.Contains(words[randomIndex]));

        //    list[randomIndex] = item;
        //    continue;

        //    ToEndLoop:
        //    list.Add(item);
        //}

        for (int i = list.Count; i > 1; i--)
        {
            int j = random.Next(i);
            list.Add(list[j]);
            list.RemoveAt(j);
        }
        return list;
    }

    /// <summary>
    /// Заполнить стек случайным образом
    /// </summary>
    /// <param name="words"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    private Stack<WordLeo> FillRandomStack(List<WordLeo> words, int count)
    {
        Stack<WordLeo> stack = new Stack<WordLeo>();
        List<WordLeo> wordsTemp = new List<WordLeo>(words);
        System.Random random = new System.Random();
        while (stack.Count < count)
        {
            int randomIndex = random.Next(wordsTemp.Count);
            if (!stack.Contains(wordsTemp[randomIndex]))
            {
                stack.Push(wordsTemp[randomIndex]);
                wordsTemp.RemoveAt(randomIndex);
            }
        }
        return stack;
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

    WordLeo IWorkout.GetCurrentWord()
    {
        return questions[questionID].questWord;
    }
}