using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using URandom = UnityEngine.Random;

public class Reiteration : MonoBehaviour, Observer, IWorkout
{
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
            return WorkoutNames.reiteration;
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
                HideImage();
                break;
            case GAME_EVENTS.LoadedVocabulary:
                LoadTasks();
                BuildTask(0);
                FindObjectOfType<DebugUI>().FillPanel(questions);
                break;
            case GAME_EVENTS.ShowResult:
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
        wordImage.sprite = null;
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
        untrainedWords = GameManager.WordManeger.GetWordsLicense();

        for (int i = 0; i < QUEST_COUNT; i++)
        {

            QuestionLeo question = GeneratorTask(i, questions);

            if (question == null)
                break;
            questions.Add(question);
        }
    }

    private void BuildTask(int current)
    {
        buttonsHandler.ClearTextInButtons();

        if (trainingСompleted || questions.Count == 0)
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

        int toNode = questionID + 1;
        if (questions.Count <= toNode)
        {
            toNode = 0;
            trainingСompleted = true;
        }

        QuestionLeo questionLeo = questions[questionID];

        // добавление слова для перевода
        string questionWord = questionLeo.questWord.wordValue;
        SetQuestion(questionWord);
        SetTranscript(questionLeo.questWord.transcription);

        //TODO: заполнять все кнопки одновременно
        buttonsHandler.FillingButtonsWithOptions(questionLeo.answers, questionWord);
        buttonsHandler.FillingEnterButton(true);

        SetImage(questionLeo.questWord.pictureURL);
        SetSound(questionLeo.questWord.soundURL);
        SetContext(questionLeo.questWord.highlightedContext);
        HideContext();

        // выбор окна диалога как активного, чтобы снять выделение с кнопок диалога
        EventSystem.current.SetSelectedGameObject(this.gameObject);
        GameManager.Notifications.PostNotification(this, GAME_EVENTS.BuildTask);
    }

    private QuestionLeo GeneratorTask(int id, List<QuestionLeo> exceptWords)
    {
        QuestionLeo questionLeo = new QuestionLeo();
        questionLeo.id = id;

        //if (words.GroupExist())
        untrainedWords = ShuffleList(untrainedWords);
        questionLeo.questWord = GetNewWord(exceptWords, untrainedWords);

        if (questionLeo.questWord == null)
        {
            Debug.LogWarning("Уникальных слов нет");
            return null;
        }

        FillAnswersForQuestion(questionLeo);

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
    /// заполнить варианты ответов
    /// </summary>
    /// <param name="questionLeo"></param>
    private void FillAnswersForQuestion(QuestionLeo questionLeo)
    {
        List<WordLeo> words = GameManager.WordManeger.GetWordsLicense();
        Stack<WordLeo> answers = null;

        if (words.Count <= ANSWER_COUNT * 2)
        {
            words = AddWordsForAnswers(words);
            //words = GameManager.WordManeger.GetAllWords();
        }

        answers = PrepareAnswers(words, ANSWER_COUNT);
        FillAnswers(questionLeo, answers);
        questionLeo.answers = ShuffleList(questionLeo.answers);
    }
    /// <summary>
    /// Добавляет слова к существующему списку слов
    /// до ANSWER_COUNT * 2
    /// </summary>
    /// <param name="words"></param>
    /// <returns></returns>
    private List<WordLeo> AddWordsForAnswers(List<WordLeo> words)
    {
        List<WordLeo> TempWords = new List<WordLeo>(ANSWER_COUNT * 2);
        if (words != null)
            TempWords.AddRange(words);
        List<WordLeo> allWords = GameManager.WordManeger.GetAllWords();

        //TODO: Заменить на случайный индекс
        allWords = ShuffleList(allWords);
        int index = 0;
        while (TempWords.Count < ANSWER_COUNT * 2)
        {
            TempWords.Add(allWords[index++]);
        }
        return TempWords;
    }

    private static void FillAnswers(QuestionLeo questionLeo, Stack<WordLeo> answers)
    {
        int[] numAnswers = { 0, 1, 2, 3, 4 };
        int indexOfQuestWord = URandom.Range(0, ANSWER_COUNT);
        questionLeo.answers = new List<WordLeo>(ANSWER_COUNT);

        foreach (var item in numAnswers)
        {
            // номер совпал с индексом для ответа
            if (item == indexOfQuestWord)
            {
                questionLeo.answers.Add(questionLeo.questWord);
                continue;
            }

            // пропустить повтор ответа для задания
            if (answers.Count != 0 && answers.Peek() == questionLeo.questWord)
                answers.Pop();

            //TODO: заполнять варианты ответов из общего словаря
            if (answers.Count != 0)
                questionLeo.answers.Add(answers.Pop());
        }
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
    private Stack<WordLeo> PrepareAnswers(List<WordLeo> words, int count)
    {
        Stack<WordLeo> stack = new Stack<WordLeo>();
        List<WordLeo> wordsTemp = new List<WordLeo>(words);

        System.Random random = new System.Random();
        while (stack.Count < count && stack.Count < wordsTemp.Count)
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