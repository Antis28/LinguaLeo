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
        if (progress.license == WordLicenses.Level_0)
            return;

        progress.Reset();
        progress.license--;
        progress.lastRepeat = DateTime.Now;
    }

    override
    public string ToString()
    {
        return wordValue;
    }


}