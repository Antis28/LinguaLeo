using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Workout : UnityEngine.Object, IWorkout
{
    public int questCount = 10;
    public const int ANSWER_COUNT = 5;

    private List<QuestionLeo> questions;
    private int questionID;

    private bool trainingСompleted;

    private ButtonsHandler buttonsHandler;

    private List<WordLeo> untrainedWords;

    WorkoutNames workoutName;

    public event UnityAction DrawTask;

    public WorkoutNames WorkoutName
    {
        get
        {
            return workoutName;
        }
    }

    // Use this for initialization
    public Workout(WorkoutNames WorkoutName, int questCount)
    {
        this.workoutName = WorkoutName;
        this.buttonsHandler = GameObject.FindObjectOfType<ButtonsHandler>();
        this.questCount = questCount;
    }

    public WordLeo GetCurrentWord()
    {
        return questions[questionID].questWord;
    }
    public QuestionLeo GetCurrentQuest()
    {
        return questions[questionID];
    }

    #region Handlers
    public void LoadQuestions()
    {
        LoadTasks();
        GameObject.FindObjectOfType<DebugUI>().FillPanel(questions);
    }
    public void SetNextQuestion()
    {
        buttonsHandler.SetNextQuestion(() => BuildTask(questionID + 1));
    }
    #endregion 
    public void SetSound(string file)
    {
        GameManager.AudioPlayer.SetSound(Utilities.ConverterUrlToName(file, false));
    }

    private void LoadTasks()
    {
        questions = new List<QuestionLeo>(questCount);
        untrainedWords = GameManager.WordManeger.GetUntrainedGroupWords(workoutName);

        for (int i = 0; i < questCount; i++)
        {
            untrainedWords = Utilities.ShuffleList(untrainedWords);
            QuestionLeo question = GeneratorTask(i, questions);

            if (question == null)
                break;
            questions.Add(question);
        }
    }

    public void BuildTask(int current)
    {
        buttonsHandler.ClearTextInButtons();

        if (!AvaiableBuilding(current))
        {
            GameManager.Notifications.PostNotification(null, GAME_EVENTS.WordsEnded);
            return;
        }
        // Отрисовать GUI
        DrawTask();
    }

    private bool CheckTrainingСompleted()
    {
        int toNode = questionID + 1;
        if (questions.Count <= toNode)
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
        if (trainingСompleted || questions.Count == 0)
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
        foreach (var quest in questions)
        {
            if (quest.id == i)
                return j;
            j++;
        }

        return -1;
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
}