using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultPanel : MonoBehaviour
{

    [SerializeField]
    Text CaptionText = null;
    [SerializeField]
    Text LearnText = null;
    [SerializeField]
    Text EatingText = null;

    [SerializeField]
    Button[] buttons;



    const int BED_RESULT = 0;
    const int GOOD_RESULT = 9;
    const int BEST_RESULT = 10;

    // Use this for initialization
    void Start()
    {
        int score = GameManager.ScoreKeeper.ScoreValue;
        ShowCaption(score);
        ShowLearn(score);
        EatingText.text = "100%";

        InitButtons();
        GameManager.ScoreKeeper.ScoreValue = 0;
        print("lastWorkout = " + GameManager.LevelManeger.lastWorkout);
    }

    private void InitButtons()
    {
        buttons = transform.GetComponentsInChildren<Button>();
        foreach (var button in buttons)
        {
            switch (button.name)
            {
                case "NextWorkoutButton":
                    //item.onClick.AddListener();
                    break;
                case "ListWorkoutButton":
                    button.onClick.AddListener(() =>
                        GameManager.LevelManeger.LoadLevel("trainingСhoice")
                        );
                    break;
                case "ContinueWorkoutButton":
                    CheckContinueWorkout(button);
                    break;
            }
        }
    }

    private void CheckContinueWorkout(Button button)
    {
        int score = GameManager.ScoreKeeper.ScoreValue;
        if (GameManager.WordManeger.CountWordInGroup() - score > 0)
        {
            button.interactable = true;
            button.onClick.AddListener(() =>
            GameManager.Notifications.PostNotification(this, GAME_EVENTS.ContinueWorkout)
            );
            button.gameObject.SetActive(true);
        }
        else
        {
            button.interactable = false;
            button.gameObject.SetActive(false);
        }
    }

    private void ShowCaption(int score)
    {
        if (score == BED_RESULT)
        {
            CaptionText.text = "В этот раз не получилось, но продолжай тренироваться!";
        }
        else if (score < GOOD_RESULT)
        {
            CaptionText.text = "Неплохо, но есть над чем поработать.";
        }
        else if (score == GOOD_RESULT)
        {
            CaptionText.text = "Круто, отличный результат!";
        }
        else if (score == BEST_RESULT)
        {
            CaptionText.text = "Поздравляем, отличный результат!";
        }
    }

    private void ShowLearn(int score)
    {
        if (score == 0 || score > 4)
            LearnText.text = string.Format("{0} слов изучено, ", score);
        else if (score == 1)
            LearnText.text = string.Format("{0} слово изучено, ", score);
        else
            LearnText.text = string.Format("{0} слова изучено, ", score);

        int countAllWords = GameManager.WordManeger.CountWordInGroup();
        if (BEST_RESULT > countAllWords)
            LearnText.text += string.Format("{0} на изучении", countAllWords - score);
        else
            LearnText.text += string.Format("{0} на изучении", BEST_RESULT - score);
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Debug.Log("Input.GetKey");
            ShowCaption(GameManager.ScoreKeeper.ScoreValue);
            ShowLearn(GameManager.ScoreKeeper.ScoreValue);
        }
    }
}

/*
1:
        2:
        3:

  0 - В этот раз не получилось, но продолжай тренироваться! 
  1 - Неплохо, но есть над чем поработать
  9 - Круто, отличный результат! 
 10 - Поздравляем, отличный результат!
*/
