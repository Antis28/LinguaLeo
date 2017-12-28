using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WordManeger : MonoBehaviour
{

    public Text wordText;
    public ButtonComponent[] buttonText;

    public string folder = "Russian"; // подпапка в Resources, для чтения

    private string fileName, lastName;
    private List<Dialogue> node;
    private Dialogue dialogue;
    private Answer answer;

    private int id;

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
        button.onClick.AddListener(() => BuildDialogue(i));
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
        foreach( Dialogue d in node )
        {
            if( d.id == i )
                return j;
            j++;
        }

        return -1;
    }

    void BuildDialogue( int current )
    {
        ClearDialogue();

        int j = FindNodeByID(current);

        if( j < 0 )
        {
            Debug.LogError(this + " в диалоге [" + fileName + ".xml] отсутствует или указан неверно идентификатор узла.");
            return;
        }

        // добавление текста NPC
        wordText.text = node[j].npcText;

        for( int i = 0; i < node[j].answer.Count; i++ )
        {

            AddToList(node[j].answer[i].exit, node[j].answer[i].toNode, node[j].answer[i].text, node[j].answer[i].questStatus, node[j].answer[i].questName, true); // текст игрока

        }

        EventSystem.current.SetSelectedGameObject(this.gameObject); // выбор окна диалога как активного, чтобы снять выделение с кнопок диалога

    }

    void AddToList( bool exit, int toNode, string text, int questStatus, string questName, bool isActive )
    {
        buttonText[id].text.text = text;
        buttonText[id].button.interactable = isActive;

        SetNextNode(buttonText[id].button, toNode);

        if( questStatus != 0 )
            SetQuestStatus(buttonText[id].button, questStatus, questName);

        id++;
    }

    void Load()
    {
        //if( lastName == fileName ) // проверка, чтобы не загружать уже загруженный файл
        //{
        //    BuildDialogue(0);
        //    return;
        //}

        node = new List<Dialogue>();

        TextAsset binary = Resources.Load<TextAsset>(folder + "/" + fileName);



        // Слово для перевода
        GeneratorDialogue1();
        GeneratorDialogue2();

        BuildDialogue(0);
    }

    private void GeneratorDialogue1()
    {
        dialogue = new Dialogue();
        dialogue.answer = new List<Answer>();

        dialogue.npcText = "sun";
        dialogue.id = 0;

        GenerateAnswer("qqqqqqqqqqqq", 1);
        dialogue.answer.Add(answer);
        GenerateAnswer("wwwwwwwwwwww", 1);
        dialogue.answer.Add(answer);
        GenerateAnswer("eeeeeeeeeee", 1);
        dialogue.answer.Add(answer);
        GenerateAnswer("rrrrrrrrrr", 1);
        dialogue.answer.Add(answer);
        GenerateAnswer("ttttttttttt", 1);
        dialogue.answer.Add(answer);

        node.Add(dialogue);
    }

    private void GeneratorDialogue2()
    {
        dialogue = new Dialogue();
        dialogue.answer = new List<Answer>();

        dialogue.npcText = "Star";
        dialogue.id = 1;

        GenerateAnswer("zzzzzzzz");
        dialogue.answer.Add(answer);
        GenerateAnswer("xxxxxxxxx");
        dialogue.answer.Add(answer);
        GenerateAnswer("ccccccccccc");
        dialogue.answer.Add(answer);
        GenerateAnswer("vvvvvvvvvvv");
        dialogue.answer.Add(answer);
        GenerateAnswer("bbbbbbbbbbbb");
        dialogue.answer.Add(answer);

        node.Add(dialogue);
    }

    private void GenerateAnswer( string text, int toNode = 0 )
    {
        answer = new Answer();
        answer.text = text;
        answer.toNode = toNode;
        answer.exit = false;
        answer.questStatus = 2;
        answer.questValue = 3;
        answer.questValueGreater = 0;
        answer.questName = "TestQuest";
    }

    void ClearDialogue()
    {
        id = 0;

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
