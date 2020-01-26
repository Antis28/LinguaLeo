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
        float score = GameManager.ScoreKeeper.GetCorrectAnswers();
        ShowCaption(score);
        ShowLearn(score);
        EatingText.text = "100%";

        InitButtons();
        GameManager.ScoreKeeper.ResetScore();
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
                        GameManager.LevelManeger.LoadLevel(SceneNames.trainingСhoice)
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
        float score = GameManager.ScoreKeeper.ScoreValue;
        int wordInGroupRemain = GameManager.WordManeger.CountUntrainWordInGroup();
        if (wordInGroupRemain > 0)
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

    private void ShowCaption(float score)
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

    private void ShowLearn(float score)
    {
        if (score < 0)
        {
            Debug.LogError("score = " + score);
            score = 0;
        }
        var resultText = GetStudiedWordsCount(score);
        resultText += GetWordsCountLeft();
        LearnText.text = resultText;
    }

    /// <summary>
    /// Получить количество изученных слов.
    /// </summary>
    /// <param name="score"></param>
    /// <returns></returns>
    private String GetStudiedWordsCount(float score)
    {
        if (score == 0 || score > 4)
            return string.Format("{0} слов изучено, ", score);
        else if (score == 1)
            return string.Format("{0} слово изучено, ", score);

        return string.Format("{0} слова изучено, ", score);
    }

    /// <summary>
    /// Получить количество оставшихся слов на изучении.
    /// </summary>
    /// <param name="score"></param>
    private String GetWordsCountLeft()
    {
        int wordsLeft = GameManager.WordManeger.CountUntrainWordInGroup();
        return string.Format("{0} на изучении", wordsLeft);
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
