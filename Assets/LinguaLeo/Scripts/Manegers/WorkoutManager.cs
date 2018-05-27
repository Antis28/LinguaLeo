using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorkoutManager : MonoBehaviour, IObserver
{
    private int questMaxCount = 10;
    private int questsPassedCount = 0;
    private const int ANSWER_COUNT = 5;

    BrainStorm brainStorm = null;

    private static WordLeo currentWord = null;

    private LevelManeger levelManeger;
    private WorkoutNames currentWorkout;
    private WorkoutNames subWorkout;
    private Workout core;


    public int GetBrainTasks()
    {
        return 16;
    }

    public int GetCorrectAnswers()
    {
        return GameManager.ScoreKeeper.ScoreValue;
    }

    public int QuestCompletedCount
    {
        get
        {
            return questsPassedCount;
        }
    }

    // Use this for initialization
    void Start()
    {
        levelManeger = FindObjectOfType<LevelManeger>();

        GameManager.Notifications.AddListener(this, GAME_EVENTS.ButtonHandlerLoaded);
        GameManager.Notifications.AddListener(this, GAME_EVENTS.WordsEnded);

        GameManager.Notifications.AddListener(this, GAME_EVENTS.BuildTask);
        GameManager.Notifications.AddListener(this, GAME_EVENTS.CorrectAnswer);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ResetQuestCompletedCount()
    {
        questsPassedCount = 0;
    }

    public void RunWorkOut(WorkoutNames name)
    {
        currentWorkout = name;
        questsPassedCount = 0;
        questMaxCount = 10;

        if (name == WorkoutNames.brainStorm)
        {
            questMaxCount = 5;
            core = PrepareWorkout(WorkoutNames.brainStorm);
            brainStorm = new BrainStorm(core, levelManeger);
            return;
        }

        string sceneName = GetSceneName(name);

        if (sceneName != string.Empty)
        {
            levelManeger.LoadLevel(sceneName);
        }
    }

    public Workout GetWorkout()
    {
        if (brainStorm == null)
            return core;
        else
            return brainStorm.GetsSubCore();
    }

    void IObserver.OnNotify(object parametr, GAME_EVENTS notificationName)
    {
        switch (notificationName)
        {
            case GAME_EVENTS.ButtonHandlerLoaded:
                StartBehaviour();
                break;
            case GAME_EVENTS.WordsEnded:
                print("ScoreValue = " + GetCorrectAnswers());
                WordsEndedBehaviour();
                break;
            case GAME_EVENTS.CorrectAnswer:
                AddWorkoutProgress(currentWord, subWorkout);
                if (currentWord.AllWorkoutDone())
                    currentWord.AddLicenseLevel();
                break;
            case GAME_EVENTS.BuildTask:
                IWorkout workout = parametr as IWorkout;
                subWorkout = workout.WorkoutName;
                currentWord = workout.GetCurrentWord();
                break;
        }
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

    private void WordsEndedBehaviour()
    {
        switch (currentWorkout)
        {
            case WorkoutNames.WordTranslate:
            case WorkoutNames.TranslateWord:
            case WorkoutNames.Savanna:
            case WorkoutNames.Audio:
            case WorkoutNames.Puzzle:
            case WorkoutNames.reiteration:
                GameManager.LevelManeger.LoadWorkOut("result");
                break;
            case WorkoutNames.brainStorm:
                brainStorm.Run();
                break;
            default:
                break;
        }
    }

    private void StartBehaviour()
    {
        SceneManager.LoadSceneAsync("wordinfo", LoadSceneMode.Additive);

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
            case WorkoutNames.Savanna:
                core = PrepareWorkout(currentWorkout);
                CoreInitialization();
                break;
            case WorkoutNames.brainStorm:
                SceneManager.LoadSceneAsync("brainInfo", LoadSceneMode.Additive);
                brainStorm.CoreInitialization();
                break;
            case WorkoutNames.Puzzle:
                core = PrepareWorkout(currentWorkout);
                CoreInitialization();
                break;
            case WorkoutNames.reiteration:
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Подготавливает ядро для тренировки
    /// </summary>
    /// <param name="currentWorkout"></param>
    /// <returns></returns>
    private Workout PrepareWorkout(WorkoutNames currentWorkout)
    {
        Workout core = new Workout(currentWorkout, questMaxCount);
        core.LoadQuestions();
        if (!core.TaskExists())
        {
            Debug.LogError("Нет доступных слов для тренировки" + currentWorkout);
            return null;
        }
        questsPassedCount += core.tasks.Count;
        return core;
    }

    private string GetSceneName(WorkoutNames name)
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
                sceneName = "wordPuzzle";
                break;
            case WorkoutNames.reiteration:
                sceneName = string.Empty;
                break;
            case WorkoutNames.brainStorm:
                sceneName = string.Empty;
                break;
            case WorkoutNames.Savanna:
                sceneName = "savanna";
                break;
        }
        return sceneName;
    }

    private void AddWorkoutProgress(WordLeo word, WorkoutNames workout)
    {
        switch (workout)
        {
            case WorkoutNames.WordTranslate:
            case WorkoutNames.reiteration:
                word.LearnWordTranslate();
                break;
            case WorkoutNames.TranslateWord:
                word.LearnTranslateWord();
                break;
            case WorkoutNames.Audio:
                word.LearnAudio();
                break;
            case WorkoutNames.Puzzle:
                word.LearnPuzzle();
                break;
        }
    }

    internal BrainStorm BrainStorm
    {
        get
        {
            throw new System.NotImplementedException();
        }

        set
        {
        }
    }
}
