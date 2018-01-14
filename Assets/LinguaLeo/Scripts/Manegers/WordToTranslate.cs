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
    private Text questionText; // Поле для вопроса
    [SerializeField]
    private Text transcriptText; // Поле для транскрипции
    [SerializeField]
    private Image wordImage; // Картинка ассоциаци со словом

    public Slider scoreSlider;
    public Text scoreText;   
    
    private int answersCount;


    // Use this for initialization
    void Awake()
    {
        GameManager.Notifications.AddListener(this, GAME_EVENTS.BuildTask);
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
        GameManager.AudioPlayer.SayWord();
    }
}
