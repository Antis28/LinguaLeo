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

    public List<WordLeo> GetUntrainedGroupWords()
    {
        List<WordLeo> untrainedWords = new List<WordLeo>();
        foreach (var item in wordsFromGroup)
        {
            CheckLicense(item);

            if (item.progress.word_translate != true)
                untrainedWords.Add(item);
        }
        return untrainedWords;
    }

    private static void CheckLicense(WordLeo word)
    {   
        WordLicenses license = word.progress.license;
        TimeSpan interval = DateTime.Now - word.progress.lastRepeat;
        switch (license)
        {
            case WordLicenses.Level_0:
                word.progress.Reset();
                break;
            case WordLicenses.Level_1:
                //лицензия на 20 минут
                if (interval.Minutes > 20)
                    word.ReduceLicense();                
                break;
            case WordLicenses.Level_2:
                //лицензия на 1 час
                if (interval.Minutes > 60)
                    word.ReduceLicense();
                break;
            case WordLicenses.Level_3:
                //лицензия на  3 часа
                if (interval.Hours > 180)
                    word.ReduceLicense();
                break;
            case WordLicenses.Level_4:
                //лицензия на 1 сутки
                if (interval.Days > 1)
                    word.ReduceLicense();
                break;
            case WordLicenses.Level_5:
                //лицензия на 2 суток
                if (interval.Days > 2)
                    word.ReduceLicense();
                break;
            case WordLicenses.Level_6:
                //лицензия на 3 суток
                if (interval.Days > 3)
                    word.ReduceLicense();
                break;
            case WordLicenses.Level_7:
                //лицензия на 1 неделю
                if (interval.Days > 7)
                    word.ReduceLicense();
                break;
            case WordLicenses.Level_8:
                //лицензия на 1 месяц
                if (interval.Days > 28)
                    word.ReduceLicense();
                break;
            case WordLicenses.Level_9:
                //лицензия на 6 месяцев
                if (interval.Days > 180)
                    word.ReduceLicense();
                break;
        }
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

