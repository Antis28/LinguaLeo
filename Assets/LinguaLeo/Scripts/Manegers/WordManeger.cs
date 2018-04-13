using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class WordManeger : MonoBehaviour, Observer
{
    private static WordCollection vocabulary = null; // полный словарь
    private static List<string> wordGroups = null; // названия наборов слов

    private static List<WordLeo> currentWordGroups = null;

    private static List<WordGroup> groupNames;

    private string folderXml = @"Data/Base";
    private string fileNameXml = "WordBase.xml";


    public List<WordLeo> GetAllWords()
    {
        return vocabulary.allWords;
    }

    /// <summary>
    /// получить нетринерованые слова из набора
    /// </summary>
    /// <returns>нетринерованые слова из набора</returns>
    public List<WordLeo> GetUntrainedGroupWords(WorkoutNames workoutName)
    {
        currentWordGroups = vocabulary.GetUntrainedGroupWords(workoutName);
        return currentWordGroups;
    }

    /// <summary>
    /// получить все слова из набора
    /// </summary>
    /// <returns></returns>
    public List<WordLeo> GetAllGroupWords()
    {
        return vocabulary.wordsFromGroup;
    }

    public List<WordLeo> GetWordsWithLicense()
    {
        List<WordLeo> allWords = GameManager.WordManeger.GetAllWords();
        List<WordLeo> wordsByLicense = new List<WordLeo>();

        foreach (var word in allWords)
        {
            word.LicenseExpirationCheck();
            bool AllWorkoutDone = word.AllWorkoutDone();
            bool license = word.LicenseExists();

            //if (!word.CanbeRepeated())
            if (AllWorkoutDone || !license)
                continue;
            wordsByLicense.Add(word);
        }
        return wordsByLicense;
    }

    internal int CountWordInGroup()
    {
        return currentWordGroups.Count;
    }

    internal int CountUntrainWordInGroup()
    {
        List<WordLeo> remainWord = Utilities.SelectNotDoneWords(
                                                    vocabulary.wordsFromGroup);
        return remainWord.Count;
    }

    /// <summary>
    /// получить описание наборов слов
    /// </summary>
    /// <returns>описание наборов слов</returns>
    public List<WordGroup> GetGroupNames()
    {
        if (groupNames != null)
            return groupNames;

        string path = folderXml + "/" + "WordGroup.xml";
        if (!File.Exists(path))
        {
            Debug.LogError("File not found. Path: " + path);
            return null;
        }
        groupNames = DeserializeGroup(path);
        //SerializeGroup(GroupNames, path);
        return groupNames;
    }

    /// <summary>
    /// Загружает набор слов из словаря
    /// </summary>
    /// <param name="groupName"></param>
    public void LoadGroup(string groupName)
    {
        vocabulary.LoadGroup(groupName);
    }

    /// <summary>
    /// Создать xml наборов слов
    /// </summary>
    public void CreateWordGroups()
    {
        throw new NotImplementedException();
        //List<string> groupNames = GetGroupNames();
        //List<WordGroup> groups = new List<WordGroup>();
        //foreach (string name in groupNames)
        //{
        //    LoadGroup(name);
        //    int count = GameManager.WordManeger.GetVocabulary().wordsFromGroup.Count;

        //    groups.Add(new WordGroup() {
        //        name = name,
        //        wordCount = count,
        //        pictureName = "504"
        //    });
        //}
        //SerializeGroup(groups);
    }

    public void ResetLicense()
    {
        foreach (WordLeo item in vocabulary.allWords)
        {
            item.ResetLicense();
        }
    }

    void Observer.OnNotify(object parametr, GAME_EVENTS notificationName)
    {

        switch (notificationName)
        {
            case GAME_EVENTS.WordsEnded:
                SaveToXml();
                break;
        }
    }

    void Start()
    {
        GameManager.Notifications.AddListener(this, GAME_EVENTS.WordsEnded);

        LoadVocabulary();
        //CreateWordGroups();
        //ResetLicense();
    }

    private void LoadVocabulary()
    {
        if (vocabulary == null)
        {
            vocabulary = LoadFromXml();
        }

        wordGroups = vocabulary.FilterGroup();
        //vocabulary.LoadGroup(wordGroups[66]);
        vocabulary.LoadGroup(wordGroups[23]);
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        StartCoroutine(LoadedVocalubary());
    }
    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        StartCoroutine(LoadedVocalubary());
    }

    IEnumerator LoadedVocalubary()
    {
        yield return null;
        if (vocabulary != null)
            GameManager.Notifications.PostNotification(this, GAME_EVENTS.LoadedVocabulary);
    }

    private WordCollection LoadFromXml()
    {
        string path = folderXml + "/" + fileNameXml;
        if (!File.Exists(path))
        {
            Debug.LogError("File not found");
            return null;
        }
        using (TextReader Stream = new StreamReader(path, Encoding.UTF8))// (path, FileMode.Open, FileAccess.Read))
        {
            XmlSerializer Serializer = new XmlSerializer(typeof(WordCollection));
            WordCollection result = Serializer.Deserialize(Stream) as WordCollection;
            Stream.Close();
            if (result == null)
                Debug.LogError("File not Serialize");
            return result;
        }
    }

    private WordCollection LoadFromXml(string xmlString)
    {
        XmlSerializer Serializer = new XmlSerializer(typeof(WordCollection));
        TextReader reader = new StringReader(xmlString);
        WordCollection result = Serializer.Deserialize(reader) as WordCollection;
        reader.Close();
        return result;
    }

    private void SaveToXml()
    {
        string path = folderXml + "/" + fileNameXml;
        if (!File.Exists(path))
        {
            Debug.LogError("File not found");
            return;
        }
        using (TextWriter stream = new StreamWriter(path, false, Encoding.UTF8))
        {

            //Now save game data
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(WordCollection));

            xmlSerializer.Serialize(stream, vocabulary);
            stream.Close();
        }
    }

    private List<WordGroup> DeserializeGroup(string FileName)
    {
        //string FileName = "WordGroup.xml";

        using (TextReader stream = new StreamReader(FileName, Encoding.UTF8))// (path, FileMode.Open, FileAccess.Read))
        {
            XmlSerializer Serializer = new XmlSerializer(typeof(List<WordGroup>));
            List<WordGroup> result = Serializer.Deserialize(stream) as List<WordGroup>;
            stream.Close();
            if (result == null)
                Debug.LogError("Do not Deserialize Group");
            else
                Debug.Log("Deserialize Group");
            return result;
        }
    }

    private void SerializeGroup(List<WordGroup> list, string fileName = "WordGroup.xml")
    {
        using (TextWriter stream = new StreamWriter(fileName, false, Encoding.UTF8))// (path, FileMode.Open, FileAccess.Read))
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<WordGroup>));
            xmlSerializer.Serialize(stream, list);
            stream.Close();
            Debug.Log("SerializeGroup");
        }
    }



}