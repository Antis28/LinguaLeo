using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkoutManager : MonoBehaviour, Observer
{
    LevelManeger levelManeger;
    WorkoutNames currentWorkout;
    List<WordLeo> untrainedWords = null;
    Workout core;

    public int questCount = 10;
    public const int ANSWER_COUNT = 5;

    private int stage;

    // Use this for initialization
    void Start()
    {
        levelManeger = FindObjectOfType<LevelManeger>();

        GameManager.Notifications.AddListener(this, GAME_EVENTS.ButtonHandlerLoaded);
        GameManager.Notifications.AddListener(this, GAME_EVENTS.WordsEnded);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void RunWorkOut(WorkoutNames name)
    {
        currentWorkout = name;
        stage = -1;
        string sceneName = string.Empty;
        switch (name)
        {
            case WorkoutNames.WordTranslate:
                sceneName = "worldTranslate";
                break;
            case WorkoutNames.TranslateWord:
                sceneName = "translateWorld";
                break;
            case WorkoutNames.Audio:
                sceneName = string.Empty;
                break;
            case WorkoutNames.Puzzle:
                sceneName = string.Empty;
                break;
            case WorkoutNames.reiteration:
                sceneName = "reiteration";
                break;
            case WorkoutNames.brainStorm:
                RunBrainStorm();
                break;
        }
        if (sceneName != string.Empty)
            levelManeger.LoadLevel(sceneName);
    }

    public List<WordLeo> GetUntrainedWords()
    {
        return untrainedWords;
    }

    private void RunBrainStorm()
    {
        string sceneName = string.Empty;
        stage++;
        switch (stage)
        {
            case -1:
                throw new Exception();
            case 0:               
                currentWorkout = WorkoutNames.WordTranslate;
                sceneName = GetSceneName(currentWorkout);
                break;
            case 1:                
                CoreInitialization();
                break;
            case 2:
                currentWorkout = WorkoutNames.TranslateWord;
                sceneName = GetSceneName(currentWorkout);
                break;
            case 3:
                CoreInitialization();
                break;
            case 10:
                stage = -1;
                GameManager.LevelManeger.LoadWorkOut("result");
                break;
        }
        if (sceneName != string.Empty)
        {
            levelManeger.LoadLevel(sceneName);
        }
            
    }

    private void CoreInitialization()
    {
        core = new Workout(currentWorkout, questCount);
        core.LoadQuestions();
        GameManager.Notifications.PostNotification(core, GAME_EVENTS.CoreBuild);
        stage++;
    }

    void Observer.OnNotify(UnityEngine.Object parametr, GAME_EVENTS notificationName)
    {
        switch (notificationName)
        {
            case GAME_EVENTS.ButtonHandlerLoaded:
                RunBrainStorm();
                break;
            case GAME_EVENTS.WordsEnded:
                print("ScoreValue = " + GameManager.ScoreKeeper.ScoreValue);
                RunBrainStorm();
                break;
        }
    }
    public string GetSceneName(WorkoutNames name)
    {
        string sceneName = string.Empty;
        switch (name)
        {
            case WorkoutNames.WordTranslate:
                sceneName = "worldTranslate";
                break;
            case WorkoutNames.TranslateWord:
                sceneName = "translateWorld";
                break;
            case WorkoutNames.Audio:
                sceneName = "translateWorld";
                break;
            case WorkoutNames.Puzzle:
                sceneName = "translateWorld";
                break;
            case WorkoutNames.reiteration:
                sceneName = "translateWorld";
                break;
            case WorkoutNames.brainStorm:
                sceneName = "translateWorld";
                break;
            case WorkoutNames.savanna:
                sceneName = "translateWorld";
                break;
        }
        return sceneName;
    }
}
