using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using URandom = UnityEngine.Random;


public class WordManeger : MonoBehaviour, Observer
{
    private ButtonsHandler buttonsHandler; 
    private WordTranslate wordTranslate; 

    public string folder = "Base"; // подпапка в Resources, для чтения

    [SerializeField]
    private string fileName = string.Empty;
    //private string lastName;

    private List<QuestionLeo> nodes;
    //private Dialogue dialogue;
    //private Answer answer;

    private int questionID;

    private const int TASK_COUNT = 10;
    private const int ANSWER_COUNT = 5;
    private bool wordsEnded;
    

    public void Start()
    {
        buttonsHandler = FindObjectOfType<ButtonsHandler>();
        wordTranslate = FindObjectOfType<WordTranslate>();
        GameManager.Notifications.AddListener(this, GAME_EVENTS.ShowResult);
        Initialization();
    }
   
    

    void SetExitDialogue(Button button) // событие, для выхода из диалога
    {
        //button.onClick.AddListener(() => CloseWindow());
        print("присвоено событие, для выхода из диалога");
    }

    void Initialization()
    {
        TextAsset binary = Resources.Load<TextAsset>(folder + "/" + fileName);
        if (binary == null)
        {
            Debug.LogWarning("File not found");
            return;
        }
        nodes = new List<QuestionLeo>(TASK_COUNT);
        WordCollection words = LoadWords(binary.text);
        LoadTasks(words);
        BuildTask(0);
    }

    private void LoadTasks(WordCollection words)
    {
        for (int i = 0; i < TASK_COUNT; i++)
        {
            QuestionLeo question = GeneratorTask(i, words);
            nodes.Add(question);
        }
    }

    private WordCollection LoadWords(string xmlString)
    {
        XmlSerializer Serializer = new XmlSerializer(typeof(WordCollection));

        TextReader reader = new StringReader(xmlString);
        WordCollection result = Serializer.Deserialize(reader) as WordCollection;
        reader.Close();
        return result;
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

    private void BuildTask(int current)
    {
        buttonsHandler.ClearTextInButtons();
        if (wordsEnded)
        {
            GameManager.Notifications.PostNotification(this, GAME_EVENTS.WordsEnded);
            return;
        }

        questionID = FindNodeByID(current);
        if (questionID < 0)
        {
            Debug.LogError(this + " в диалоге [" + fileName + ".xml] отсутствует или указан неверно идентификатор узла.");
            return;
        }

        GameManager.Notifications.PostNotification(this, GAME_EVENTS.BuildTask);
        int toNode = questionID + 1;
        if (TASK_COUNT == toNode)
        {
            toNode = 0;
            wordsEnded = true;
        }

        // добавление слова для перевода
        string questionWord = nodes[questionID].questWord.engWord;
        wordTranslate.SetQuestion(questionWord);
        
        //TODO: заполнять все кнопки одновременно
        buttonsHandler.FillingButtonsWithOptions(nodes[questionID].answers, questionWord);
        buttonsHandler.FillingEnterButton(true);

        wordTranslate.ShowImage(nodes[questionID].questWord.imageURL);
        wordTranslate.SayWord(nodes[questionID].questWord.audioURL);

        // выбор окна диалога как активного, чтобы снять выделение с кнопок диалога
        EventSystem.current.SetSelectedGameObject(this.gameObject);
    }

    /// <summary>
    /// Поиск ноды по ID 
    /// </summary>
    /// <param name="i">ID для поиска</param>
    /// <returns></returns>
    int FindNodeByID(int i)
    {
        int j = 0;
        foreach (var quest in nodes)
        {
            if (quest.id == i)
                return j;
            j++;
        }

        return -1;
    }
    

        int GetINT(string text)
    {
        int value;
        if (int.TryParse(text, out value))
        {
            return value;
        }
        return 0;
    }

    bool GetBOOL(string text)
    {
        bool value;
        if (bool.TryParse(text, out value))
        {
            return value;
        }
        return false;
    }

    //ToDo: Вынести метод в отделюный класс
    string ConverterUrlToName(string url)
    {
        //string url = "http://contentcdn.lingualeo.com/uploads/picture/3466359.png";
        //string url = "http://contentcdn.lingualeo.com/uploads/picture/96-631152008.mp3";
        string patern = @"(\d+.png$)|(\d+-\d+.mp3$)";
        Regex rg = new Regex(patern, RegexOptions.IgnoreCase);
        Match mat = rg.Match(url);

        return Path.GetFileNameWithoutExtension(mat.Value);
    }

    void Observer.OnNotify(Component sender, GAME_EVENTS notificationName)
    {
        switch (notificationName)
        {
            case GAME_EVENTS.ShowResult:
                buttonsHandler.SetNextQuestion(nodes[questionID].answers,
                                                () => BuildTask(questionID + 1));
                break;
        }
    }
}

class UniqRandom
{
    readonly int MAX_COUNT;
    List<int> lastIndex;

    UniqRandom(int max)
    {
        MAX_COUNT = max;
        lastIndex = new List<int>(MAX_COUNT);
    }

    int nextRandom()
    {
        int rndValue = -1;

        do
        {
            rndValue = URandom.Range(0, MAX_COUNT);
        } while (lastIndex.Contains(rndValue));

        return rndValue;
    }
}

public class MyComparer : IComparer
{
    public int Compare(object x, object y)
    {
        ButtonComponent lVal = x as ButtonComponent;
        ButtonComponent rValt = y as ButtonComponent;

        return Compare(lVal.gameObject, rValt.gameObject);
    }

    // Call CaseInsensitiveComparer.Compare with the parameters reversed.
    public int Compare(GameObject x, GameObject y)
    {
        return (new CaseInsensitiveComparer()).Compare(x.name, y.name);
    }
}