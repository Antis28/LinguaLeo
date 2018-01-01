using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using URandom = UnityEngine.Random;

public class WordManeger : MonoBehaviour
{

    public Text wordText;
    public ButtonComponent[] buttons;

    public string folder = "Base"; // подпапка в Resources, для чтения

    [SerializeField]
    private string fileName;
    private string lastName;

    private List<QuestionLeo> nodes;
    //private Dialogue dialogue;
    //private Answer answer;

    private int id;
    private const int TASK_COUNT = 10;
    private const int ANSWER_COUNT = 5;
    private bool exit;

    void Awake()
    {
        buttons = FindObjectsOfType<ButtonComponent>();
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
        print("событие, для перенаправления на другой узел диалога");
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
        questionLeo.questWord.isTrue = true;

        return questionLeo;
    }

    private void BuildTask(int current)
    {
        ClearDialogue();
        if (exit)
            return;

        int j = FindNodeByID(current);

        if (j < 0)
        {
            Debug.LogError(this + " в диалоге [" + fileName + ".xml] отсутствует или указан неверно идентификатор узла.");
            return;
        }

        int toNode = j + 1;
        if (TASK_COUNT == toNode)
        {
            toNode = 0;
            exit = true;
        }

        // добавление слова для перевода
        wordText.text = nodes[j].questWord.engWord;
        // добавление вариантов для перевода
        for (int i = 0; i < nodes[j].answers.Count; i++)
        {
            AddToList(toNode, nodes[j].answers[i].ruWord, nodes[j].answers[i].isTrue);
        }


        // выбор окна диалога как активного, чтобы снять выделение с кнопок диалога
        EventSystem.current.SetSelectedGameObject(this.gameObject);
    }

    private void AddToList(int toNode, string text, bool isTrue)
    {
        buttons[id].text.text = text;

        SetNextNode(buttons[id].button, toNode);
        id++;
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
        id = 0;
        wordText.text = "";
        foreach (ButtonComponent b in buttons)
        {
            b.text.text = string.Empty;
            b.rect.sizeDelta = new Vector2(b.rect.sizeDelta.x, 0);
            b.rect.anchoredPosition = new Vector2(b.rect.anchoredPosition.x, 0);
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
