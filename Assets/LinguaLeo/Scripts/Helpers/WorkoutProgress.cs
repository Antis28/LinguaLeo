using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

/// <summary>
/// Прогресс изучения слова
/// на разных тренировках
/// </summary>

public class WorkoutProgress
{
    [XmlElement("translate")]
    public bool word_translate;
    [XmlElement("reverse")]
    public bool translate_word;
    //savannah,
    [XmlElement("audio")]
    public bool audio_word;
    [XmlElement("puzzle")]
    public bool word_puzzle;
    //words_cards;
    //phrase_puzzle;

    public DateTime lastRepeat;
    public LicenseLevels license;

    public WorkoutProgress() { }

    public void ResetWorkouts()
    {
        word_translate = false;
        translate_word = false;
        audio_word = false;
        word_puzzle = false;
    }

    public bool AllWorkoutDone()
    {
        return word_translate;
       //return word_translate &&
       // translate_word &&
       // audio_word &&
       // word_puzzle;
    }
}
