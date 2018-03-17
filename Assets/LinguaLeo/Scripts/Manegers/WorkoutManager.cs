using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkoutManager : MonoBehaviour, Observer
{
    LevelManeger levelManeger;
    WorkoutNames currentWorkout;
    List<WordLeo> untrainedWords;

    private int stage;

    // Use this for initialization
    void Start()
    {
        levelManeger = FindObjectOfType<LevelManeger>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void RunWorkOut(WorkoutNames name)
    {
        currentWorkout = name;

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
        switch (stage)
        {
            case 0:
                
                stage++;
                break;
            case 10:
                GameManager.LevelManeger.LoadWorkOut("result");
                break;
                
        }
    }

    private void LoadTasks(int QUEST_COUNT)
    {
        List<QuestionLeo> questions = new List<QuestionLeo>(QUEST_COUNT);
        untrainedWords = GameManager.WordManeger.GetUntrainedGroupWords(currentWorkout);

        for (int i = 0; i < QUEST_COUNT; i++)
        {
            untrainedWords = Utilities.ShuffleList(untrainedWords);
            QuestionLeo question = GeneratorTask(i, questions);

            if (question == null)
                break;
            questions.Add(question);
        }
    }

    void Observer.OnNotify(Component sender, GAME_EVENTS notificationName)
    {
        switch (notificationName)
        {
            case GAME_EVENTS.WordsEnded:
                print("ScoreValue = " + GameManager.ScoreKeeper.ScoreValue);
                RunBrainStorm();
                break;
        }
    }
}
