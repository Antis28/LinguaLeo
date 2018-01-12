using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using URandom = UnityEngine.Random;


public class WordManeger : MonoBehaviour
{
    public Text wordText;
    public Image wordImage;

    public ButtonComponent[] buttons;

    public string folder = "Base"; // подпапка в Resources, для чтения

    [SerializeField]
    private string fileName;
    private string lastName;

    private List<QuestionLeo> nodes;
    //private Dialogue dialogue;
    //private Answer answer;

    private int buttonID;
    private int questionID;

    private const int TASK_COUNT = 10;
    private const int ANSWER_COUNT = 5;
    private const string DONT_KNOW = "Не знаю :(";
    private const string NEXT_WORD = "Следующее →";
    private bool exit;

    void Awake()
    {
        buttons = FindObjectsOfType<ButtonComponent>();
        Array.Sort(buttons, new MyComparer());
    }

    public void Start()
    {
        Load();
    }

    void SetQuestStatus(Button button, int i, string name) // событие, для управлением статуса
    {
        //button.onClick.AddListener(() => QuestStatus(t));
        print("событие, для управлением статуса");
    }

    void SetNextNode(Button button, int i) // событие, для перенаправления на другой узел диалога
    {
        button.onClick.AddListener(() => BuildTask(i));
        //print("событие, для перенаправления на другой узел диалога");
    }

    void SetShowResult(Button button, bool istrue) // событие, для перенаправления на другой узел диалога
    {
        button.onClick.AddListener(() => ShowResult(button, istrue));
    }

    private void ShowResult(Button button, bool istrue)
    {
        if (istrue)
        {
            SetColors(button, Color.green);
            GameManager.Notifications.PostNotification(this, GAME_EVENTS.CorrectAnswer);
           
        }
        if (!istrue)
        {
            GameManager.Notifications.PostNotification(this, GAME_EVENTS.NonCorrectAnswer);

            SetColors(button, Color.red);

            SetColors(FindTrueButton(), Color.green);

            
        }        
        ClearListeners();
        FillingEnterButton(false);
        SetNextQuestion();
    }

    private void SayWord(string file)
    {
        GameManager.AudioPlayer.SayWord(ConverterUrlToName(file));
    }

    private Button FindTrueButton()
    {
        foreach (var item in buttons)
        {
            bool trueWord = item.text.text == nodes[questionID].questWord.ruWord;
            if (trueWord)
            {
                return item.button;
            }
        }
        return null;
    }

    private void SetColors(Button button, Color color)
    {
        var colors = button.colors;
        colors.normalColor = color;
        colors.highlightedColor = color;
        button.colors = colors;
    }
    private void ResetColors()
    {
        foreach (var button in buttons)
        {
            SetColors(button.button, Color.white);
        }
    }

    void SetExitDialogue(Button button) // событие, для выхода из диалога
    {
        //button.onClick.AddListener(() => CloseWindow());
        print("присвоено событие, для выхода из диалога");
    }

    void Load()
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
        ClearDialogue();
        if (exit)
        {
            print("ScoreValue = " + ScoreKeeper.ScoreValue);
            GameManager.Notifications.PostNotification(this, GAME_EVENTS.Exit);
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
            exit = true;
        }

        // добавление слова для перевода
        wordText.text = nodes[questionID].questWord.engWord;

        FillingButtonsWithOptions(toNode);
        FillingEnterButton(true);

        ShowImage(nodes[questionID].questWord.imageURL);
        SayWord(nodes[questionID].questWord.audioURL);

        // выбор окна диалога как активного, чтобы снять выделение с кнопок диалога
        EventSystem.current.SetSelectedGameObject(this.gameObject);
    }

    private void FillingEnterButton(bool isFirst)
    {
        if (isFirst)
        {
            // Отдельная кнопка "не знаю"
            ButtonComponent extraB = buttons[buttonID];
            extraB.text.text = DONT_KNOW;
            extraB.button.GetComponentInChildren<Text>().color = Color.black;
            SetColors(extraB.button, Color.white);

            SetShowResult(buttons[buttonID].button, false);
        }
        else
        {
            // Отдельная кнопка "следующее"
            ButtonComponent extraB = buttons[buttonID];
            extraB.text.text = NEXT_WORD;
            extraB.button.GetComponentInChildren<Text>().color = Color.white;

            SetColors(extraB.button, new Color(68f/255, 145/255f, 207f/255));
            SetNextNode(buttons[buttonID].button, questionID + 1);
        }
    }

    /// <summary>
    /// добавление вариантов для перевода
    /// </summary>
    /// <param name="toNode"></param>
    private void FillingButtonsWithOptions(int toNode)
    {
        for (int i = 0; i < nodes[questionID].answers.Count; i++)
        {
            string ruWord = nodes[questionID].answers[i].ruWord;
            AddToList(toNode, ruWord);
        }
    }

    private void AddToList(int toNode, string text)
    {
        buttons[buttonID].text.text = text;

        bool isTrue = CheckAnswer(buttons[buttonID].button, nodes[questionID]);

        SetShowResult(buttons[buttonID].button, isTrue);

        buttonID++;
    }

    private bool CheckAnswer(Button button, QuestionLeo questionLeo)
    {
        return buttons[buttonID].text.text == questionLeo.questWord.ruWord;
    }

    void SetNextQuestion()
    {
        buttonID = 0;
        for (int i = 0; i < nodes[questionID].answers.Count; i++)
        {
            SetNextNode(buttons[buttonID].button, questionID + 1);
            buttonID++;
        }
    }

    private void ShowImage(string fileName)
    {
        string foloder = "!Pict";
        Sprite sprite = Resources.Load<Sprite>(foloder + "/" + ConverterUrlToName(fileName));
        wordImage.sprite = sprite;
        wordImage.preserveAspect = true;
    }
    private void HideImage()
    {
        wordImage.sprite = null;
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

    /// <summary>
    /// Очистить кнопки от текста
    /// </summary>
    void ClearDialogue()
    {
        HideImage();
        ResetColors();
        buttonID = 0;
        wordText.text = "";
        foreach (ButtonComponent b in buttons)
        {
            b.text.text = string.Empty;
            b.button.onClick.RemoveAllListeners();
        }
    }
    void ClearListeners()
    {
        foreach (ButtonComponent b in buttons)
        {
            b.button.onClick.RemoveAllListeners();
        }
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

    string ConverterUrlToName(string url)
    {
        //string url = "http://contentcdn.lingualeo.com/uploads/picture/3466359.png";
        //string url = "http://contentcdn.lingualeo.com/uploads/picture/96-631152008.mp3";
        string patern = @"(\d+.png$)|(\d+-\d+.mp3$)";
        Regex rg = new Regex(patern, RegexOptions.IgnoreCase);
        Match mat = rg.Match(url);

        return Path.GetFileNameWithoutExtension(mat.Value);
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