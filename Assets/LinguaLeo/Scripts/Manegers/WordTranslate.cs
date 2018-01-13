using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class WordTranslate : MonoBehaviour, Observer
{
    public Text questionText; // Поле для вопроса
    public Image wordImage; // Картинка ассоциаци со словом

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

    public void ClearTextInQestion()
    {
        questionText.text = "";
    }

    public void HideImage()
    {
        wordImage.sprite = null;
    }

    public void ShowImage(string fileName)
    {
        string foloder = "!Pict";
        Sprite sprite = Resources.Load<Sprite>(foloder + "/" + ConverterUrlToName(fileName));
        wordImage.sprite = sprite;
        wordImage.preserveAspect = true;
    }

    public void SayWord(string file)
    {
        GameManager.AudioPlayer.SayWord(ConverterUrlToName(file));
    }

    //ToDo: Вынести метод в отделюный класс
    private string ConverterUrlToName(string url)
    {
        //string url = "http://contentcdn.lingualeo.com/uploads/picture/3466359.png";
        //string url = "http://contentcdn.lingualeo.com/uploads/picture/96-631152008.mp3";
        string patern = @"(\d+.png$)|(\d+-\d+.mp3$)";
        Regex rg = new Regex(patern, RegexOptions.IgnoreCase);
        Match mat = rg.Match(url);

        return Path.GetFileNameWithoutExtension(mat.Value);
    }
}
