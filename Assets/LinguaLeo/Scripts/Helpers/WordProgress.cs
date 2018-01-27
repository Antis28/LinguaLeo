using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// Прогресс изучения слова
/// на разных тренировках
/// </summary>

public class WordProgress
{
    [XmlAttribute]
    public string word;
    public bool word_translate,
     translate_word,
     //savannah,
     audio_word,
     word_puzzle;
     //words_cards;
     //phrase_puzzle;

    public WordProgress() { }


    // Use this for initialization
    public void Start()
    {

        List<WordProgress> progress;
        progress = new List<WordProgress>();
        progress.Add(new WordProgress() { word = "also",word_translate = true});
        progress.Add(new WordProgress() { word = "check",audio_word = true});
        progress.Add(new WordProgress() { word = "pick", word_puzzle = true});

        Serialize(progress);

    }

    private List<WordProgress> Deserialize()
    {
        string FileName = "Test.xml";
        FileStream stream = new FileStream(FileName, FileMode.Open);

        XmlSerializer Serializer = new XmlSerializer(typeof(List<WordProgress>));

        TextReader reader = new StreamReader(stream);
        List<WordProgress> result = Serializer.Deserialize(reader) as List<WordProgress>;
        reader.Close();
        return result;
    }

    private void Serialize(List<WordProgress> wp)
    {
        string FileName = "Test.xml";

        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<WordProgress>));
        FileStream Stream = new FileStream(FileName, FileMode.Create);
        xmlSerializer.Serialize(Stream, wp);
        Stream.Close();
        Debug.Log(FileName);
    }
}
