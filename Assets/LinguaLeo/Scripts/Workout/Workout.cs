using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Workout : IWorkout
{
    public int maxQuestCount = 10;
    public const int ANSWER_COUNT = 5;

    public ButtonsHandler buttonsHandler;

    public List<QuestionLeo> tasks;
    private int questionID;

    private bool trainingСompleted;

    private List<WordLeo> untrainedWords;

    WorkoutNames workoutName;

    public event UnityAction DrawTask;

    void OnDrawTask()
    {
        if (DrawTask != null)
            DrawTask();
    }

    public WorkoutNames WorkoutName
    {
        get
        {
            return workoutName;
        }
    }

    public bool TaskExists()
    {
        return tasks != null && tasks.Count > 0;
    }

    // Use this for initialization
    public Workout(WorkoutNames WorkoutName, int questCount)
    {
        this.workoutName = WorkoutName;
        this.maxQuestCount = questCount;
    }

    public bool TrainingDone()
    {
        bool trainingDone = true;
        foreach (var task in tasks)
        {
            trainingDone = trainingDone && task.questWord.AllWorkoutDone();
        }
        return trainingDone;
    }

    public WordLeo GetCurrentWord()
    {
        return tasks[questionID].questWord;
    }
    public QuestionLeo GetCurrentQuest()
    {
        return tasks[questionID];
    }

    #region Handlers
    public void LoadQuestions()
    {
        tasks = LoadTasks();
    }
    public void SetNextQuestion()
    {
        buttonsHandler.SetNextQuestion(RunNextQuestion);
    }
    public void RunNextQuestion()
    {
        BuildTask(questionID + 1);
    }
    #endregion 
    public void SetSound(string file)
    {
        GameManager.AudioPlayer.SetSound(Utilities.ConverterUrlToName(file, false));
    }

    public void SetButtons(List<string> answers, string questWord)
    {
        buttonsHandler.FillingButtonsWithOptions(answers, questWord);
        buttonsHandler.FillingEnterButton(true);
    }

    public Workout GetCore()
    {
        return this;
    }

    public void BuildFirstTask()
    {
        BuildTask(0);
    }

    private List<QuestionLeo> LoadTasks()
    {
        List<QuestionLeo> questionsTemp = new List<QuestionLeo>(maxQuestCount);
        untrainedWords = GameManager.WordManeger.GetUntrainedGroupWords(workoutName);
        if (untrainedWords.Count == 0)
            return questionsTemp;

        untrainedWords = Utilities.ShuffleList(untrainedWords);
        untrainedWords = Utilities.SortWordsByProgress(untrainedWords);
        for (int i = 0; i < maxQuestCount; i++)
        {
            QuestionLeo question = GeneratorTask(i, questionsTemp);

            if (question == null)
                break;
            questionsTemp.Add(question);
        }
        return questionsTemp;
    }

    private void BuildTask(int current)
    {
        if (buttonsHandler)
            buttonsHandler.ClearTextInButtons();

        if (!AvaiableBuilding(current))
        {
            GameManager.Notifications.PostNotification(null, GAME_EVENTS.WordsEnded);
            return;
        }
        // Отрисовать GUI
        OnDrawTask();
    }

    private bool CheckTrainingСompleted()
    {
        int toNode = questionID + 1;
        if (tasks.Count <= toNode)
        {
            toNode = 0;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Построение задания доступно?
    /// </summary>
    /// <param name="current"></param>
    /// <returns></returns>
    private bool AvaiableBuilding(int current)
    {
        if (trainingСompleted || tasks.Count == 0)
        {
            return false;
        }
        questionID = FindNodeByID(current);
        if (questionID < 0)
        {
            Debug.LogError(this + "отсутствует или указан неверно идентификатор узла.");
            return false;
        }
        trainingСompleted = CheckTrainingСompleted();
        return true;
    }

    private QuestionLeo GeneratorTask(int id, List<QuestionLeo> exceptWords)
    {
        QuestionLeo questionLeo = new QuestionLeo();
        questionLeo.id = id;

        questionLeo.questWord = GetNewWord(exceptWords, untrainedWords);

        if (questionLeo.questWord == null)
        {
            Debug.LogWarning("Уникальных слов нет");
            return null;
        }

        questionLeo.FillInAnswers(ANSWER_COUNT);

        return questionLeo;
    }

    /// <summary>
    /// Найти слово которого нет в списке
    /// </summary>
    /// <param name="exceptWords"></param>
    /// <param name="words"></param>
    /// <returns></returns>
    private static WordLeo GetNewWord(List<QuestionLeo> exceptWords, List<WordLeo> words)
    {
        foreach (var item in words)
        {
            if (!exceptWords.Contains(new QuestionLeo(item)))
            {
                return item;
            }
        }
        return null;
    }

    /// <summary>
    /// Поиск вопроса по ID 
    /// </summary>
    /// <param name="i">ID для поиска</param>
    /// <returns></returns>
    int FindNodeByID(int i)
    {
        int j = 0;
        foreach (var quest in tasks)
        {
            if (quest.id == i)
                return j;
            j++;
        }

        return -1;
    }
}