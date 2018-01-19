using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class WordToTranslate : MonoBehaviour, Observer
{
    [SerializeField]
    private Text questionText = null; // Поле для вопроса

    [SerializeField]
    private Text transcriptText = null; // Поле для транскрипции

    [SerializeField]
    private Image wordImage = null; // Картинка ассоциаци со словом

    [SerializeField]
    private Toggle sayToggle = null; // checkbox для автопроизношения

    [SerializeField]
    private Slider scoreSlider = null; // Протренировано слов прогресс
    [SerializeField]
    private Text scoreText = null; // Число протренированых слов
    [SerializeField]
    private int answersCount; // Число протренированых слов

    [SerializeField]
    private Text contextText = null; //Поле для контекста
    private GameObject contextPanel; //Панель для контекста


    // Use this for initialization
    void Awake()
    {
        GameManager.Notifications.AddListener(this, GAME_EVENTS.BuildTask);
        contextPanel = contextText.transform.parent.gameObject;
    }

    void Observer.OnNotify(Component sender, GAME_EVENTS notificationName)
    {
        switch (notificationName)
        {
            case GAME_EVENTS.BuildTask:
                ProgeressUpdate();
                HideImage();
                break;
        }
    }

    private void ProgeressUpdate()
    {
        answersCount++;
        scoreText.text = answersCount + "/" + scoreSlider.maxValue;
        scoreSlider.value = answersCount;
    }

    public void SetQuestion(string quest)
    {
        questionText.text = quest;
    }
    public void SetTranscript(string transcript)
    {
        transcriptText.text = transcript;
    }

    public void ClearTextInQestion()
    {
        questionText.text = "";
    }

    public void HideImage()
    {
        wordImage.sprite = null;
    }

    public void SetImage(string fileName)
    {
        string foloder = "!Pict";
        Sprite sprite = Resources.Load<Sprite>(foloder + "/" + Utilities.ConverterUrlToName(fileName));
        wordImage.sprite = sprite;
        wordImage.preserveAspect = true;
    }

    public void SetSound(string file)
    {
        GameManager.AudioPlayer.SetSound(Utilities.ConverterUrlToName(file));
        if (sayToggle.isOn)
            GameManager.AudioPlayer.SayWord();
    }

    public void SetContext(string context)
    {
        if (context != string.Empty)
            contextText.text = context;
        else
            contextText.text = "(нет контекста)";
    }

    public void ShowContext()
    {
        contextPanel.SetActive(true);
    }
    public void HideContext()
    {
        contextPanel.SetActive(false);
    }
}
