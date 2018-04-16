using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using URandom = UnityEngine.Random;

public class Savah : MonoBehaviour, IObserver, IWorkout
{
    [SerializeField]
    private WorkoutNames WorkoutName = WorkoutNames.WordTranslate;

    //[SerializeField]
    //private int questCount = 30;

    [SerializeField]
    private Text questionText = null;

    WorkoutNames IWorkout.WorkoutName
    {
        get
        {
            return WorkoutName;
        }
    }


    private Workout core;

    // Use this for initialization
    void Start()
    {
        GameManager.Notifications.AddListener(this, GAME_EVENTS.CoreBuild);
        GameManager.Notifications.AddListener(this, GAME_EVENTS.ShowResult);
    }

    private void Core_DrawTask()
    {

        QuestionLeo questionLeo = core.GetCurrentQuest();

        // добавление слова для перевода
        SetQuestion(questionLeo.questWord.wordValue);
        SetButtons(questionLeo, questionLeo.questWord);

        //Выше заполнить GUI
        ResetSelection();
    }

    private void ResetSelection()
    {
        // выбор окна диалога как активного, чтобы снять выделение с кнопок диалога
        EventSystem.current.SetSelectedGameObject(this.gameObject);
        GameManager.Notifications.PostNotification(this, GAME_EVENTS.BuildTask);
    }

    void IObserver.OnNotify(object parametr, GAME_EVENTS notificationName)
    {
        switch (notificationName)
        {
            case GAME_EVENTS.CoreBuild:
                core = parametr as Workout;
                core.buttonsHandler = GameObject.FindObjectOfType<ButtonsHandler>();
                core.DrawTask += Core_DrawTask;
                core.BuildTask(0);
                //FindObjectOfType<DebugUI>().FillPanel(questions);
                break;
            case GAME_EVENTS.ShowResult:
                core.RunNextQuestion();
                break;
        }
    }

    public QuestionLeo GetCurrentQuest()
    {
        return core.GetCurrentQuest();
    }

    WordLeo IWorkout.GetCurrentWord()
    {
        return core.GetCurrentWord();
    }

    private void SetQuestion(string quest)
    {
        questionText.text = quest;
    }
    /// <summary>
    /// Заполнить кнопки вариантами ответов
    /// </summary>
    /// <param name="questionLeo"></param>
    /// <param name="questionWord"></param>
    private void SetButtons(QuestionLeo questionLeo, WordLeo questionWord)
    {
        List<string> answers = new List<string>(Workout.ANSWER_COUNT);

        foreach (WordLeo item in questionLeo.answers)
            answers.Add(item.translations);

        core.SetButtons(answers, questionWord.translations);

    }

    public Workout GetCore()
    {
        return core.GetCore();
    }
}