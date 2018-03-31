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
        GameManager.Notifications.AddListener(this, GAME_EVENTS.NotUntrainedWords);
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

        if (name == WorkoutNames.brainStorm)
        {
            questCount = 2;
            RunBrainStorm();
            return;
        }

        string sceneName = GetSceneName(name);

        if (sceneName != string.Empty)
            levelManeger.LoadLevel(sceneName);
    }

    public List<WordLeo> GetUntrainedWords()
    {
        return untrainedWords;
    }

    private void RunBrainStorm(bool isEnd = false)
    {
        string sceneName = string.Empty;
        WorkoutNames nextWorkout = WorkoutNames.TranslateWord;
        stage++;
        if (isEnd)
            stage = 100;
        switch (stage)
        {
            case -1:
                throw new Exception();
            case 0:
                nextWorkout = WorkoutNames.WordTranslate;
                core = PrepareWorkout(nextWorkout);
                if (core == null)
                {
                    stage++;
                    RunBrainStorm();
                    break;
                }
                sceneName = GetSceneName(nextWorkout);
                break;
            case 1:
                nextWorkout = WorkoutNames.TranslateWord;
                core = PrepareWorkout(nextWorkout);
                if (core == null)
                {
                    stage++;
                    RunBrainStorm();
                    break;
                }
                sceneName = GetSceneName(nextWorkout);
                break;
            case 2:
                nextWorkout = WorkoutNames.Audio;
                core = PrepareWorkout(nextWorkout);
                if (core == null)
                {
                    stage++;
                    RunBrainStorm();
                    break;
                }
                sceneName = GetSceneName(nextWorkout);
                break;
            case 3:
                nextWorkout = WorkoutNames.WordTranslate;
                core = PrepareWorkout(nextWorkout);
                if (core == null)
                {
                    stage++;
                    RunBrainStorm();
                    break;
                }
                sceneName = GetSceneName(nextWorkout);
                break;
            case 4:
                nextWorkout = WorkoutNames.Audio;
                core = PrepareWorkout(nextWorkout);
                if (core == null)
                {
                    stage++;
                    RunBrainStorm();
                    break;
                }
                sceneName = GetSceneName(nextWorkout);
                break;
            case 5:
            case 6:
                //Завершает тренировку на следующей итерации
                stage = 99;
                break;
            case 100:
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
            return null;
        }
        return core;
    }

    private void CoreInitialization()
    {
        if (core != null)
            GameManager.Notifications.PostNotification(core, GAME_EVENTS.CoreBuild);
        else
        {
            Debug.LogError("core == null");
            GameManager.Notifications.PostNotification(core, GAME_EVENTS.NotUntrainedWords);
        }
    }

    void Observer.OnNotify(object parametr, GAME_EVENTS notificationName)
    {
        switch (notificationName)
        {
            case GAME_EVENTS.ButtonHandlerLoaded:
                StartBehaviour();
                break;
            case GAME_EVENTS.WordsEnded:
                print("ScoreValue = " + GameManager.ScoreKeeper.ScoreValue);
                WordsEndedBehaviour();
                break;
            case GAME_EVENTS.NotUntrainedWords:
                if (currentWorkout == WorkoutNames.brainStorm)
                    RunBrainStorm(false);
                break;
        }
    }

    private void WordsEndedBehaviour()
    {
        switch (currentWorkout)
        {
            case WorkoutNames.WordTranslate:
            case WorkoutNames.TranslateWord:
            case WorkoutNames.savanna:
            case WorkoutNames.Audio:
            case WorkoutNames.Puzzle:
                GameManager.LevelManeger.LoadWorkOut("result");
                break;
            case WorkoutNames.reiteration:
                break;
            case WorkoutNames.brainStorm:
                RunBrainStorm();
                break;
            default:
                break;
        }
    }

    private void StartBehaviour()
    {
        switch (currentWorkout)
        {
            case WorkoutNames.WordTranslate:
            case WorkoutNames.TranslateWord:
                core = PrepareWorkout(currentWorkout);
                CoreInitialization();
                break;
            case WorkoutNames.Audio:
                core = PrepareWorkout(currentWorkout);
                CoreInitialization();
                break;
            case WorkoutNames.Puzzle:
                break;
            case WorkoutNames.reiteration:
                break;
            case WorkoutNames.brainStorm:
                CoreInitialization();
                break;
            case WorkoutNames.savanna:
                break;
            default:
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
                sceneName = "audioTest";
                break;
            case WorkoutNames.Puzzle:
                sceneName = string.Empty;
                break;
            case WorkoutNames.reiteration:
                sceneName = string.Empty;
                break;
            case WorkoutNames.brainStorm:
                sceneName = string.Empty;
                break;
            case WorkoutNames.savanna:
                sceneName = string.Empty;
                break;
        }
        return sceneName;
    }
}
