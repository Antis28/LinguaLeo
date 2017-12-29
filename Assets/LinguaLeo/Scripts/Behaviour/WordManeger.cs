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
    public ButtonComponent[] buttonText;

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
        buttonText = FindObjectsOfType<ButtonComponent>();
    }

    public void Start()
    {
        Load();
    }

    void SetQuestStatus( Button button, int i, string name ) // событие, для управлением статуса
    {
        //button.onClick.AddListener(() => QuestStatus(t));
        print("событие, для управлением статуса");
    }

    void SetNextNode( Button button, int i ) // событие, для перенаправления на другой узел диалога
    {
        button.onClick.AddListener(() => BuildTask(i));
        print("событие, для перенаправления на другой узел диалога");
    }

    void SetExitDialogue( Button button ) // событие, для выхода из диалога
    {
        //button.onClick.AddListener(() => CloseWindow());
        print("присвоено событие, для выхода из диалога");

    }

    void QuestStatus( string s ) // меняем статус слова
    {
        print("меняем статус слова");
    }

    int FindNodeByID( int i )
    {
        int j = 0;
        foreach( var quest in nodes )
        {
            if( quest.id == i )
                return j;
            j++;
        }

        return -1;
    }

    void BuildTask( int current )
    {
        ClearDialogue();
        if( exit )
            return;

        int j = FindNodeByID(current);
        // добавление слова для перевола
        wordText.text = nodes[j].questWord;
        if( j < 0 )
        {
            Debug.LogError(this + " в диалоге [" + fileName + ".xml] отсутствует или указан неверно идентификатор узла.");
            return;
        }

        // добавление текста NPC
        wordText.text = nodes[j].questWord;
        int toNode = j + 1;
        if( TASK_COUNT == toNode )
        {
            toNode = 0;
            exit = true;
        }

        for( int i = 0; i < nodes[j].answers.Count; i++ )
        {
            AddToList(toNode, nodes[j].answers[i].ruWord, nodes[j].answers[i].isTrue);
        }


        // выбор окна диалога как активного, чтобы снять выделение с кнопок диалога
        EventSystem.current.SetSelectedGameObject(this.gameObject);
    }

    void BuildDialogue( int current )
    {
        //ClearDialogue();

        //int j = FindNodeByID(current);

        //if( j < 0 )
        //{
        //    Debug.LogError(this + " в диалоге [" + fileName + ".xml] отсутствует или указан неверно идентификатор узла.");
        //    return;
        //}

        //// добавление текста NPC
        //wordText.text = node[j].questWord;

        //for( int i = 0; i < node[j].answer.Count; i++ )
        //{

        //    AddToList(node[j].answer[i].exit, node[j].answer[i].toNode, node[j].answer[i].text, node[j].answer[i].questStatus, node[j].answer[i].questName, true); // текст игрока

        //}
    }

    void AddToList( int toNode, string text, bool isTrue )
    {
        buttonText[id].text.text = text;

        SetNextNode(buttonText[id].button, toNode);
        id++;
    }

    void Load()
    {
        if( lastName == fileName ) // проверка, чтобы не загружать уже загруженный файл
        {
            BuildTask(0);
            return;
        }

        TextAsset binary = Resources.Load<TextAsset>(folder + "/" + fileName);
        if( binary == null )
        {
            Debug.LogWarning("File not found");
            return;
        }

        XmlSerializer Serializer = new XmlSerializer(typeof(WordCollection));

        TextReader reader = new StringReader(binary.text);
        var result = Serializer.Deserialize(reader);
        reader.Close();

        WordCollection words = result as WordCollection;

        nodes = new List<QuestionLeo>(TASK_COUNT);
        for( int i = 0; i < TASK_COUNT; i++ )
        {
            // Слово для перевода
            WordLeo word = words.GetWord();
            QuestionLeo ql = GeneratorTask(i, word, words.GetWords());
            nodes.Add(ql);
        }


        //GeneratorDialogue1();
        BuildTask(0);
    }

    private QuestionLeo GeneratorTask( int id, WordLeo word, List<WordLeo> alternativeWord )
    {
        QuestionLeo questionLeo = new QuestionLeo();
        questionLeo.answers = new List<AnswerLeo>();

        questionLeo.questWord = word.engWord;
        questionLeo.id = id;

        List<AnswerLeo> sequential = CreateAnswers(alternativeWord);
        sequential.Add(CreateAnswer(word, true));

        int index;
        AnswerLeo answerLeo;
        List<int> lastIndex = new List<int>(ANSWER_COUNT);

        for( int i = 0; i < ANSWER_COUNT; i++ )
        {
            int rndValue = -1;
            do
            {
                rndValue = URandom.Range(0, ANSWER_COUNT);
            } while( lastIndex.Contains(rndValue) );
            lastIndex.Add(rndValue);

            questionLeo.answers.Add(sequential[rndValue]);
        }

        return questionLeo;
    }

    private AnswerLeo CreateAnswer( WordLeo item, bool isTrue = false )
    {
        AnswerLeo al = new AnswerLeo();
        al.ruWord = item.ruWord;
        al.engWord = item.engWord;
        al.isTrue = isTrue;
        return al;
    }

    private List<AnswerLeo> CreateAnswers( List<WordLeo> items )
    {
        List<AnswerLeo> list = new List<AnswerLeo>();
        foreach( var item in items )
        {
            list.Add(CreateAnswer(item));
        }
        return list;
    }

    void ClearDialogue()
    {
        id = 0;
        wordText.text = "";
        foreach( ButtonComponent b in buttonText )
        {
            b.text.text = string.Empty;
            b.rect.sizeDelta = new Vector2(b.rect.sizeDelta.x, 0);
            b.rect.anchoredPosition = new Vector2(b.rect.anchoredPosition.x, 0);
            b.button.onClick.RemoveAllListeners();
        }
    }

    int GetINT( string text )
    {
        int value;
        if( int.TryParse(text, out value) )
        {
            return value;
        }
        return 0;
    }

    bool GetBOOL( string text )
    {
        bool value;
        if( bool.TryParse(text, out value) )
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

    UniqRandom( int max )
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
        } while( lastIndex.Contains(rndValue) );

        return rndValue;
    }
}
