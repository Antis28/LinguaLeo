﻿using System;
using System.Collections.Generic;
using System.Xml.Serialization;

/// <summary>
/// Mapping Class
/// см. класс ExportWordLeo
/// в проекте Обучение/CSVReader
/// </summary>

public class WordLeo
{
    [XmlAttribute]
    public string wordValue;

    [XmlAttribute]
    public string translations;

    [XmlAttribute]
    public string pictureURL;

    [XmlAttribute]
    public string transcription;

    [XmlAttribute]
    public string highlightedContext;

    [XmlAttribute]
    public string soundURL;

    [XmlAttribute]
    public string clozefiedContext;

    [XmlArray]
    [XmlArrayItem("group")]
    public string[] groups;

    [XmlElement]
    public WorkoutProgress progress;

    public WordLeo() { }

    /// <summary>
    /// Понизить лицензию
    /// </summary>
    /// <param name="word"></param>
    public void ReduceLicense()
    {
        if (progress.license == LicenseLevels.Level_0)
            return;

        progress.ResetWorkouts();
        progress.license--;
        progress.lastRepeat = DateTime.Now;
    }

    public void ResetLicense()
    {
        if (progress.license == LicenseLevels.Level_0)
        {
            progress.ResetWorkouts();
            return;
        }

        progress.ResetWorkouts();
        progress.license = LicenseLevels.Level_0;
        progress.lastRepeat = DateTime.Now;
    }

    public void UnlockWorkouts()
    {
        progress.ResetWorkouts();
    }

    /// <summary>
    /// Проверка не истекла ли еще лицензия.
    /// Если истекла, то понижается уровень лицензии.
    /// </summary>
    public void LicenseValidityCheck()
    {
        LicenseLevels license = progress.license;
        TimeSpan interval = DateTime.Now - progress.lastRepeat;
        double minutes = interval.TotalMinutes;
        switch (license)
        {
            case LicenseLevels.Level_0:
                progress.ResetWorkouts();
                break;
            case LicenseLevels.Level_1:
                //лицензия на 20 минут
                if (minutes > LicenseTimeout.Level_1)
                    ReduceLicense();
                break;
            case LicenseLevels.Level_2:
                //лицензия на 1 час
                if (minutes > LicenseTimeout.Level_2)
                    ReduceLicense();
                break;
            case LicenseLevels.Level_3:
                //лицензия на  3 часа
                if (minutes > LicenseTimeout.Level_3)
                    ReduceLicense();
                break;
            case LicenseLevels.Level_4:
                //лицензия на 1 сутки
                if (minutes > LicenseTimeout.Level_4)
                    ReduceLicense();
                break;
            case LicenseLevels.Level_5:
                //лицензия на 2 суток
                if (minutes > LicenseTimeout.Level_5)
                    ReduceLicense();
                break;
            case LicenseLevels.Level_6:
                //лицензия на 3 суток
                if (minutes > LicenseTimeout.Level_6)
                    ReduceLicense();
                break;
            case LicenseLevels.Level_7:
                //лицензия на 1 неделю
                if (minutes > LicenseTimeout.Level_7)
                    ReduceLicense();
                break;
            case LicenseLevels.Level_8:
                //лицензия на 1 месяц
                if (minutes > LicenseTimeout.Level_8)
                    ReduceLicense();
                break;
            case LicenseLevels.Level_9:
                //лицензия на 6 месяцев
                if (minutes > LicenseTimeout.Level_9)
                    ReduceLicense();
                break;
        }
    }

    /// <summary>
    /// Проверка можно ли уже повторять слово.
    /// Если можно, то сбрасываются все тренировки.
    /// </summary>
    public void CheckingTimeForTraining()
    {
        TimeSpan interval = DateTime.Now - progress.lastRepeat;
        double minutes = interval.TotalMinutes;

        if (minutes > LicenseTimeout.Level_1)
            LicenseValidityCheck();
        LicenseLevels license = progress.license;

        switch (license)
        {
            case LicenseLevels.Level_1:
                //лицензия на 20 минут
                if (minutes > LicenseTimeTraining.Level_1)
                    UnlockWorkouts();
                break;
            case LicenseLevels.Level_2:
                //лицензия на 1 час
                if (minutes > LicenseTimeTraining.Level_2)
                    UnlockWorkouts();
                break;
            case LicenseLevels.Level_3:
                //лицензия на  3 часа
                if (minutes > LicenseTimeTraining.Level_3)
                    UnlockWorkouts();
                break;
            case LicenseLevels.Level_4:
                //лицензия на 1 сутки
                if (minutes > LicenseTimeTraining.Level_4)
                    UnlockWorkouts();
                break;
            case LicenseLevels.Level_5:
                //лицензия на 2 суток
                if (minutes > LicenseTimeTraining.Level_5)
                    UnlockWorkouts();
                break;
            case LicenseLevels.Level_6:
                //лицензия на 3 суток
                if (minutes > LicenseTimeTraining.Level_6)
                    UnlockWorkouts();
                break;
            case LicenseLevels.Level_7:
                //лицензия на 1 неделю
                if (minutes > LicenseTimeTraining.Level_7)
                    UnlockWorkouts();
                break;
            case LicenseLevels.Level_8:
                //лицензия на 1 месяц
                if (minutes > LicenseTimeTraining.Level_8)
                    UnlockWorkouts();
                break;
            case LicenseLevels.Level_9:
                //лицензия на 6 месяцев
                if (minutes > LicenseTimeTraining.Level_9)
                    UnlockWorkouts();
                break;
        }
    }

    public bool CanbeRepeated()
    {
        bool canbeRepeated = !progress.AllWorkoutDone();
        canbeRepeated &= progress.license >= LicenseLevels.Level_1;
        return canbeRepeated;
    }

    override
    public string ToString()
    {
        return wordValue;
    }


}