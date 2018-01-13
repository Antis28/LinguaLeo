using System;
using System.Collections.Generic;
using System.Xml.Serialization;


[XmlRoot("Root")]
public class WordCollection
{
    [XmlArray("LeoWords")]
    [XmlArrayItem("word")]
    public List<WordLeo> words;

    private Random random = new Random();
        
    public WordLeo GetRandomWord()
    {
        int randomIndex = random.Next(words.Count);

        WordLeo word = words[randomIndex];
        return word;
    }

    public List<WordLeo> GetRandomWords(int Count)
    {
        List<WordLeo> words = new List<WordLeo>(Count);
        for( int i = 0; i < Count; i++ )
        {
            words.Add(GetRandomWord());
        }
        return words;
    }
}

