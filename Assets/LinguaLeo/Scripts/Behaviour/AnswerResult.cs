using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerResult : MonoBehaviour, Observer
{
    [SerializeField]
    private Text ResultText = null;
    [SerializeField]
    private Text SampleText = null;

    private Color correctColor = new Color(59 / 255f,
                                                152 / 255f,
                                                57 / 255f);
    private Color wrongColor = new Color(157 / 255f,
                                                38 / 255f,
                                                29 / 255f);

    public void OnNotify(UnityEngine.Object parametr, GAME_EVENTS notificationName)
    {
        ButtonComponent button = null;
        if (parametr)
            button = ((Component)parametr).GetComponent<ButtonComponent>();
        if (ResultText == null)
            return;
        switch (notificationName)
        {
            case GAME_EVENTS.CorrectAnswer:
                ResultText.text = "Верный ответ.";
                ResultText.color = correctColor;
                break;
            case GAME_EVENTS.NonCorrectAnswer:
                ResultText.text = "Неверный ответ.";
                ResultText.color = wrongColor;
                FillSamleText(button);
                break;
            case GAME_EVENTS.BuildTask:
                ResultText.text = string.Empty;
                SampleText.text = string.Empty;
                break;
        }
    }

    private void FillSamleText(ButtonComponent button)
    {
        WordToTranslate wordToTranslate = FindObjectOfType<WordToTranslate>();
        if (!button || !wordToTranslate)
        {
            Debug.LogWarning("Не найден WordToTranslate");
            return;
        }

        QuestionLeo quest = wordToTranslate.GetCurrentQuest();
        WordLeo word = null;
        foreach (var item in quest.answers)
        {
            if (button.text.text == item.translations
                || button.text.text == item.wordValue)
            {
                word = item;
                break;
            }
        }
        if (word == null)
            return;
        if (button.text.text == word.translations)
            SampleText.text = word.translations + " → " + word.wordValue;
        else
            SampleText.text = word.wordValue + " → " + word.translations;
    }

    // Use this for initialization
    void Start()
    {
        GameManager.Notifications.AddListener(this, GAME_EVENTS.CorrectAnswer);
        GameManager.Notifications.AddListener(this, GAME_EVENTS.NonCorrectAnswer);
        GameManager.Notifications.AddListener(this, GAME_EVENTS.BuildTask);
    }
}
