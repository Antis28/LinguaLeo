using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class BrainStorm : IObserver
{
    private LevelManeger levelManeger;
    private Workout core;
    private Workout subCore;
    private int stage;
    private WorkoutNames subWorkout;

    public BrainStorm(Workout brainStormCore, LevelManeger levelManeger)
    {
        core = brainStormCore;
        this.levelManeger = levelManeger;

        ResetStage();
        Run();

        GameManager.Notifications.AddListener(null, GAME_EVENTS.NotUntrainedWords);
    }
    /// <summary>
    /// Поведение тренировки мозгового штурма
    /// </summary>
    public void Run()
    {
        string sceneName = string.Empty;
        stage++;
        switch (stage)
        {
            case 0:
                throw new Exception();
            case 1:
                sceneName = PrepareWordTranslate();
                break;
            case 2:
                sceneName = PrepareAudioTest();
                break;
            case 3:
                sceneName = PrepareTranslateWord();
                break;
            case 4:
                sceneName = PrepareWordPuzzle();
                break;
            case 5:
                if (!core.TrainingDone())
                {
                    stage = 0;
                    Run();
                    return;
                }
                TerminateBrainStorm(sceneName);
                break;
            case 100:
                ShowResult();
                break;
        }
        if (sceneName != string.Empty)
        {
            levelManeger.LoadLevel(sceneName);
        }

    }

    public void CoreInitialization()
    {
        if (subCore != null)
        {
            GameManager.Notifications.PostNotification(subCore, GAME_EVENTS.CoreBuild);
        }
        else
        {
            Debug.LogError("core == null");
            GameManager.Notifications.PostNotification(subCore, GAME_EVENTS.NotUntrainedWords);
        }
    }

    private void ShowResult()
    {
        ResetStage();
        GameManager.LevelManeger.LoadWorkOut("result");
    }

    public void ResetStage()
    {
        stage = 0;
    }

    private Workout FilterCore(WorkoutNames currentWorkout)
    {
        Workout newCore = new Workout(currentWorkout, core.maxQuestCount);
        List<QuestionLeo> newTasks = new List<QuestionLeo>();
        int ID = 0;
        foreach (var task in core.tasks)
        {
            if (task.questWord.CanTraining(currentWorkout))
            {
                task.id = ID++;
                newTasks.Add(task);
            }
        }
        newCore.tasks = newTasks;
        return newCore;
    }

    /// <summary>
    /// //Завершает тренировку
    /// </summary>
    /// <param name="sceneName"></param>
    private void TerminateBrainStorm(string sceneName)
    {
        if (sceneName != string.Empty)
        {
            //Завершает тренировку на следующей итерации
            stage = 99;
        }
        else
        {
            //Завершает тренировку            
            ShowResult();
        }
    }

    private string InitSubCore()
    {
        subCore = FilterCore(subWorkout);

        if (CoreValid(subCore))
            return GetSceneName(subWorkout);
        else
            return string.Empty;
    }

    private bool CoreValid(Workout workout)
    {
        if (workout == null)
        {
            //перейти к следующей тренировке
            Run();
            return false;
        }
        return true;
    }

    private string PrepareWordTranslate()
    {
        subWorkout = WorkoutNames.WordTranslate;
        return InitSubCore();
    }

    private string PrepareTranslateWord()
    {
        subWorkout = WorkoutNames.TranslateWord;
        return InitSubCore();
    }

    private string PrepareAudioTest()
    {
        subWorkout = WorkoutNames.Audio;
        return InitSubCore();
    }

    private string PrepareWordPuzzle()
    {
        subWorkout = WorkoutNames.Puzzle;
        return InitSubCore();
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

    void IObserver.OnNotify(object parametr, GAME_EVENTS notificationName)
    {
        switch (notificationName)
        {
            case GAME_EVENTS.NotUntrainedWords:                
                    Run();
                break;
        }
    }
}

