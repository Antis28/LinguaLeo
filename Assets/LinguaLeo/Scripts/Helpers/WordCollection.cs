using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;


[XmlRoot("Root")]
public class WordCollection
{
    [XmlArray("LeoWords")]
    [XmlArrayItem("word")]
    public List<WordLeo> allWords; // полный словарь

    [XmlIgnore]
    public List<WordLeo> wordsFromGroup; // словарь для набора слов

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

    public WordLeo GetRandomWordFromGroup()
    {
        int randomIndex = random.Next(wordsFromGroup.Count);

        WordLeo word = wordsFromGroup[randomIndex];
        return word;
    }

    public List<WordLeo> GetRandomWordsFromGroup(int Count)
    {
        List<WordLeo> words = new List<WordLeo>(Count);
        for (int i = 0; i < Count; i++)
        {
            words.Add(GetRandomWordFromGroup());
        }
        return words;
    }

    public List<WordLeo> GetUntrainedWordsGroup(int Count)
    {
        List<WordLeo> untrainedWords = new List<WordLeo>(Count);
        WordLeo word = null;
        // неповторяемые слова
        for (int index = 0, j = 0; index < Count; index++)
        {
            while (j < wordsFromGroup.Count)
            {
                word = wordsFromGroup[j];
                j++;
                if (word.progress.word_translate != true)
                    break;
            }

            if (word == null)
                break;
            untrainedWords.Add(word);
        }
        return untrainedWords;
    }

    public bool GroupExist()
    {
        return wordsFromGroup != null;
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
        groups = groups.OrderBy((x) => x).ToList();
        return groups;
    }

    /// <summary>
    /// Загрука слов из набора слов
    /// </summary>
    /// <param name="groupName">название набора слов</param>
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

