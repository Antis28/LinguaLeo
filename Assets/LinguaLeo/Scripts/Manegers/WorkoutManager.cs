using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkoutManager : MonoBehaviour, Observer
{
    LevelManeger levelManeger;
    List<WordLeo> untrainedWords = null;
    WorkoutNames currentWorkout;
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
        questCount = 10;
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
                questCount = 5;
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
                core = PrepareWorkout(WorkoutNames.WordTranslate);
                sceneName = GetSceneName(currentWorkout);
                if (core != null)
                    break;
                stage++;
                RunBrainStorm();
                break;
            case 1:
                CoreInitialization();
                break;
            case 2:
                core = PrepareWorkout(WorkoutNames.TranslateWord);
                sceneName = GetSceneName(currentWorkout);
                if (core != null)
                    break;
                stage++;
                RunBrainStorm();
                break;
            case 3:
                CoreInitialization();
                break;
            case 4:
                stage = -1;
                GameManager.LevelManeger.LoadWorkOut("result");
                break;
        }
        if (sceneName != string.Empty)
        {
            levelManeger.LoadLevel(sceneName);
        }

    }
    /// <summary>
    /// Подготавливает ядро для тренировки
    /// </summary>
    /// <param name="currentWorkout"></param>
    /// <returns></returns>
    private Workout PrepareWorkout(WorkoutNames currentWorkout)
    {
        Workout core = new Workout(currentWorkout, questCount);
        core.LoadQuestions();
        if (!core.TaskExists())
        {
            Debug.LogError("Нет доступных слов для тренировки" + currentWorkout);
            GameManager.Notifications.PostNotification(null, GAME_EVENTS.NotUntrainedWords);
            return null;
        }
        return core;
    }

    private void CoreInitialization()
    {
        core.maxQuestCount = questCount;
        GameManager.Notifications.PostNotification(core, GAME_EVENTS.CoreBuild);
    }

    void Observer.OnNotify(object parametr, GAME_EVENTS notificationName)
    {
        switch (notificationName)
        {
            case GAME_EVENTS.ButtonHandlerLoaded:
                RunBrainStorm();
                break;
            case GAME_EVENTS.WordsEnded:
                print("ScoreValue = " + GameManager.ScoreKeeper.ScoreValue);

                if (currentWorkout == WorkoutNames.brainStorm)
                    RunBrainStorm();
                else
                    GameManager.LevelManeger.LoadWorkOut("result");
                break;
            case GAME_EVENTS.NotUntrainedWords:
                
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
