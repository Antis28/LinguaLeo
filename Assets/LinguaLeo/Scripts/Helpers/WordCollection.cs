using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;


[XmlRoot("Root")]
public class WordCollection
{
    [XmlArray("LeoWords")]
    [XmlArrayItem("word")]
    public List<WordLeo> allWords;

    [XmlIgnore]
    List<WordLeo> wordsFromGroup;

    private Random random = new Random();

    public WordLeo GetRandomWord()
    {
        int randomIndex = random.Next(allWords.Count);

        WordLeo word = allWords[randomIndex];
        return word;
    }

    public List<WordLeo> GetRandomWords(int Count)
    {
        List<WordLeo> words = new List<WordLeo>(Count);
        for (int i = 0; i < Count; i++)
        {
            words.Add(GetRandomWord());
        }
        return words;
    }
       
    /// <summary>
    /// Извлекает название всех групп
    /// </summary>
    /// <returns>список групп</returns>
    public List<string> FilterGroup()
    {
        List<string> groups = new List<string>();

        foreach (var word in allWords)
        {
            foreach (var group in word.groups)
            {
                if (!groups.Contains(group))
                    groups.Add(group);
            }
        }
        return groups;
    }

    public void LoadGroup(string groupName)
    {
        wordsFromGroup = new List<WordLeo>();
        foreach (var word in allWords)
        {
            if (word.groups.Contains(groupName))
                wordsFromGroup.Add(word);
        }
    }
}

