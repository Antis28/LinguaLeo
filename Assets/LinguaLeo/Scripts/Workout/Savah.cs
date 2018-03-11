using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using URandom = UnityEngine.Random;

public class Savah : MonoBehaviour, Observer, IWorkout
{
    [SerializeField]
    private WorkoutNames WorkoutName = WorkoutNames.WordTranslate;

    [SerializeField]
    private int questCount = 30;

    WorkoutNames IWorkout.WorkoutName
    {
        get
        {
            return WorkoutName;
        }
    }
    

    private Workout core;

    // Use this for initialization
    void Awake()
    {
        GameManager.Notifications.AddListener(this, GAME_EVENTS.ShowResult);
        GameManager.Notifications.AddListener(this, GAME_EVENTS.BuildTask);
        GameManager.Notifications.AddListener(this, GAME_EVENTS.LoadedVocabulary);
                
        core = new Workout(WorkoutName, questCount);
        core.DrawTask += Core_DrawTask;
    }

    private void Core_DrawTask()
    {
        
        //Выше заполнить GUI
        ResetSelection();
    }

    private void ResetSelection()
    {
        // выбор окна диалога как активного, чтобы снять выделение с кнопок диалога
        EventSystem.current.SetSelectedGameObject(this.gameObject);
        GameManager.Notifications.PostNotification(this, GAME_EVENTS.BuildTask);
    }

    void Observer.OnNotify(Component sender, GAME_EVENTS notificationName)
    {
        switch (notificationName)
        {
            case GAME_EVENTS.LoadedVocabulary:
                core.LoadVocabulary();
                //FindObjectOfType<DebugUI>().FillPanel(questions);
                break;            
            case GAME_EVENTS.ShowResult:
                core.SetNextQuestion();
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
}