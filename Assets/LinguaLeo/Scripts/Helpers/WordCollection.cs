using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;


[XmlRoot("Root")]
public class WordCollection
{
    private const int MAX_CAPACITY = 4;
    private Random random = new Random();

    [XmlArray("LeoWords")]
    [XmlArrayItem("word")]
    public List<WordLeo> words;

    public WordLeo GetWord()
    {
        int randomIndex = random.Next(words.Count);

        WordLeo word = words[randomIndex];
        return word;
    }

    public List<WordLeo> GetWords()
    {
        List<WordLeo> words = new List<WordLeo>(MAX_CAPACITY);
        for( int i = 0; i < MAX_CAPACITY; i++ )
        {
            words.Add(GetWord());
        }
        return words;
    }
}

