using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonsHandler : MonoBehaviour
{
    [SerializeField]
    private ButtonComponent[] buttons;
    [SerializeField]
    private Button RepeatWordButton = null;

    private int buttonID;

    private const string DONT_KNOW = "Не знаю :(";
    private const string NEXT_WORD = "Следующее →";
    private Button correctButton = null;

    void Start()
    {
        buttons = FindObjectsOfType<ButtonComponent>();
        System.Array.Sort(buttons, new MyComparer());

        if (RepeatWordButton)
            RepeatWordButton.onClick.AddListener(() => GameManager.AudioPlayer.SayWord());

        GameManager.Notifications.PostNotification(null, GAME_EVENTS.ButtonHandlerLoaded);
    }

    /// <summary>
    /// Очистить кнопки от текста
    /// </summary>
    public void ClearTextInButtons()
    {
        ResetColors();
        buttonID = 0;
        //TODO: Очистить текст вопроса ClearTextInQestion()
        foreach (ButtonComponent b in buttons)
        {
            b.text.text = string.Empty;
            b.button.onClick.RemoveAllListeners();
        }
    }


    /// <summary>
    /// Заполнение кнопок с ответами
    /// </summary>
    /// <param name="toNode"></param>
    public void FillingButtonsWithOptions(List<string> listWords, string questionWord)
    {
        ClearListeners();
        foreach (string word in listWords)
        {
            bool answerIsCorrect = word.Contains(questionWord);
            ButtonComponent bc = buttons[buttonID];

            if (answerIsCorrect)
                correctButton = bc.button;

            bc.text.text = word;
            JoinShowResult(bc.button, answerIsCorrect);
            buttonID++;
        }
    }


    /// <summary>
    /// действие для кнопки Enter
    /// </summary>
    /// <param name="isFirst"></param>
    public void FillingEnterButton(bool isFirst)
    {
        if (buttons.Length <= buttonID)
            return;

        ButtonComponent extraB = buttons[buttonID];
        if (isFirst)
        {
            EnterButtonToDontKnow(extraB);
        }
        else
        {
            EnterButtonToNext(extraB);
        }
    }

    private void EnterButtonToDontKnow(ButtonComponent extraB)
    {
        // Отдельная кнопка "не знаю"            
        extraB.text.text = DONT_KNOW;
        extraB.button.GetComponentInChildren<Text>().color = Color.black;
        SetColors(extraB.button, Color.white);

        JoinShowResult(buttons[buttonID].button, false);
    }

    private void EnterButtonToNext(ButtonComponent extraB)
    {
        // Отдельная кнопка "следующее"
        extraB.text.text = NEXT_WORD;
        extraB.button.GetComponentInChildren<Text>().color = Color.white;

        SetColors(extraB.button, new Color(68f / 255, 145 / 255f, 207f / 255));
    }

    void ClearListeners()
    {
        foreach (ButtonComponent b in buttons)
        {
            b.button.onClick.RemoveAllListeners();
        }
    }

    private void ResetColors()
    {
        foreach (var button in buttons)
        {
            SetColors(button.button, Color.white);
        }
    }
    private void SetColors(Button button, Color color)
    {
        var colors = button.colors;
        colors.normalColor = color;
        colors.highlightedColor = color;
        button.colors = colors;
    }

    /// <summary>
    /// Присоеденить на кнопку событие
    /// "ShowResult"
    /// </summary>
    /// <param name="button">Кнопка для события</param>
    /// <param name="istrue"></param>
    private void JoinShowResult(Button button, bool istrue)
    {
        button.onClick.AddListener(() => ShowResult(button, istrue));
    }

    /// <summary>
    /// Присоеденить на кнопку событие
    /// перехода к следующему слову
    /// </summary>
    /// <param name="button"></param>
    /// <param name="questionID">ID для следующей ноды</param>
    private void JoinNextNode(Button button, UnityAction action)//int questionID) 
    {
        //ToDo: событие, для перенаправления на другой узел диалога
        button.onClick.AddListener(action);
        //print("событие, для перенаправления на другой узел диалога");
    }

    public void SetNextQuestion(UnityAction action)
    {
        print("SetNextQuestion");
        ClearListeners();
        buttonID = 0;
        foreach (var button in buttons)
        {
            JoinNextNode(button.button, action);
            buttonID++;
        }
    }

    /// <summary>
    /// Показывает правильный ли ответ был нажат,
    /// если нет, то показывает правильный
    /// </summary>
    /// <param name="button"></param>
    /// <param name="istrue"></param>
    private void ShowResult(Button button, bool istrue)
    {
        if (istrue)
        {
            SetColors(button, Color.green);
            GameManager.Notifications.PostNotification(button, GAME_EVENTS.CorrectAnswer);
        }
        if (!istrue)
        {

            GameManager.Notifications.PostNotification(button, GAME_EVENTS.NonCorrectAnswer);

            SetColors(button, Color.red);
            //кнопка с правильным словом
            SetColors(correctButton, Color.green);
        }
        ClearListeners();
        FillingEnterButton(false);
        //установить следующий вопрос       
        GameManager.Notifications.PostNotification(null, GAME_EVENTS.ShowResult);
    }


}
