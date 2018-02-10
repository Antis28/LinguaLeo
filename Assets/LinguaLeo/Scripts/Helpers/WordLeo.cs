using System;
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

    public void LicenseValidityCheck()
    {
        LicenseLevels license = progress.license;
        TimeSpan interval = DateTime.Now - progress.lastRepeat;
        switch (license)
        {
            case LicenseLevels.Level_0:
                progress.ResetWorkouts();
                break;
            case LicenseLevels.Level_1:
                //лицензия на 20 минут
                if (interval.Minutes > LicenseTimeout.Level_1)
                    ReduceLicense();
                break;
            case LicenseLevels.Level_2:
                //лицензия на 1 час
                if (interval.Minutes > LicenseTimeout.Level_2)
                    ReduceLicense();
                break;
            case LicenseLevels.Level_3:
                //лицензия на  3 часа
                if (interval.Minutes > LicenseTimeout.Level_3)
                    ReduceLicense();
                break;
            case LicenseLevels.Level_4:
                //лицензия на 1 сутки
                if (interval.Minutes > LicenseTimeout.Level_4)
                    ReduceLicense();
                break;
            case LicenseLevels.Level_5:
                //лицензия на 2 суток
                if (interval.Minutes > LicenseTimeout.Level_5)
                    ReduceLicense();
                break;
            case LicenseLevels.Level_6:
                //лицензия на 3 суток
                if (interval.Minutes > LicenseTimeout.Level_6)
                    ReduceLicense();
                break;
            case LicenseLevels.Level_7:
                //лицензия на 1 неделю
                if (interval.Minutes > LicenseTimeout.Level_7)
                    ReduceLicense();
                break;
            case LicenseLevels.Level_8:
                //лицензия на 1 месяц
                if (interval.Minutes > LicenseTimeout.Level_8)
                    ReduceLicense();
                break;
            case LicenseLevels.Level_9:
                //лицензия на 6 месяцев
                if (interval.Minutes > LicenseTimeout.Level_9)
                    ReduceLicense();
                break;
        }
    }

    public void CheckingTimeForTraining()
    {
        LicenseLevels license = progress.license;
        TimeSpan interval = DateTime.Now - progress.lastRepeat;

        if (interval.Minutes > LicenseTimeout.Level_1)
            LicenseValidityCheck();

        switch (license)
        {
            case LicenseLevels.Level_1:
                //лицензия на 20 минут
                if (interval.Minutes > LicenseTimeTraining.Level_1)
                    UnlockWorkouts();
                break;
            case LicenseLevels.Level_2:
                //лицензия на 1 час
                if (interval.Minutes > LicenseTimeTraining.Level_2)
                    UnlockWorkouts();
                break;
            case LicenseLevels.Level_3:
                //лицензия на  3 часа
                if (interval.Minutes > LicenseTimeTraining.Level_3)
                    UnlockWorkouts();
                break;
            case LicenseLevels.Level_4:
                //лицензия на 1 сутки
                if (interval.Minutes > LicenseTimeTraining.Level_4)
                    UnlockWorkouts();
                break;
            case LicenseLevels.Level_5:
                //лицензия на 2 суток
                if (interval.Minutes > LicenseTimeTraining.Level_5)
                    UnlockWorkouts();
                break;
            case LicenseLevels.Level_6:
                //лицензия на 3 суток
                if (interval.Minutes > LicenseTimeTraining.Level_6)
                    UnlockWorkouts();
                break;
            case LicenseLevels.Level_7:
                //лицензия на 1 неделю
                if (interval.Minutes > LicenseTimeTraining.Level_7)
                    UnlockWorkouts();
                break;
            case LicenseLevels.Level_8:
                //лицензия на 1 месяц
                if (interval.Minutes > LicenseTimeTraining.Level_8)
                    UnlockWorkouts();
                break;
            case LicenseLevels.Level_9:
                //лицензия на 6 месяцев
                if (interval.Minutes > LicenseTimeTraining.Level_9)
                    UnlockWorkouts();
                break;
        }
    }

    override
    public string ToString()
    {
        return wordValue;
    }


}