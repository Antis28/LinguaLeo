using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class WordManeger : MonoBehaviour, Observer
{
    private static WordCollection vocabulary = null; // полный словарь
    private static List<string> wordGroups = null; // названия наборов слов

    private static WordLeo currentWord = null;
    private static WorkoutNames currentWorkoutName;

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
    public List<WordLeo> GetUntrainedGroupWords()
    {
        return vocabulary.GetUntrainedGroupWords();
    }

    /// <summary>
    /// Возвращает все слова из набора
    /// </summary>
    /// <returns></returns>
    public List<WordLeo> GetAllGroupWords()
    {
        return vocabulary.wordsFromGroup;
    }

    /// <summary>
    /// получить описание наборов слов
    /// </summary>
    /// <returns>описание наборов слов</returns>
    public List<WordGroup> GetGroupNames()
    {
        //return vocabulary.FilterGroup();
        string path = folderXml + "/" + "WordGroup.xml";
        if (!File.Exists(path))
        {
            Debug.LogError("File not found");
            return null;
        }
        var groups = DeserializeGroup(path);
        //SerializeGroup(groups, path);
        return groups;
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

    IEnumerator Start()
    {
        GameManager.Notifications.AddListener(this, GAME_EVENTS.WordsEnded);
        GameManager.Notifications.AddListener(this, GAME_EVENTS.BuildTask);
        GameManager.Notifications.AddListener(this, GAME_EVENTS.CorrectAnswer);
        yield return new WaitForSeconds(0.1f);
        LoadVocabulary();
        //CreateWordGroups();
        //ResetWorkoutProgress();
    }

    private void LoadVocabulary()
    {
        if (vocabulary == null)
        {
            vocabulary = LoadFromXml();
        }

        wordGroups = vocabulary.FilterGroup();
        //vocabulary.LoadGroup(wordGroups[66]);
        vocabulary.LoadGroup(wordGroups[1]);
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        GameManager.Notifications.PostNotification(this, GAME_EVENTS.LoadedVocabulary);
    }
    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
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
        using (TextWriter stream = new StreamWriter(fileName,false, Encoding.UTF8))// (path, FileMode.Open, FileAccess.Read))
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<WordGroup>));
            xmlSerializer.Serialize(stream, list);
            stream.Close();
            Debug.Log("SerializeGroup");
        }
    }

    public void AddWorkoutProgress(WordLeo word, WorkoutNames workout)
    {
        switch (workout)
        {
            case WorkoutNames.translate:
                word.progress.word_translate = true;
                break;
            case WorkoutNames.reverse:
                word.progress.translate_word = true;
                break;
            case WorkoutNames.audio:
                word.progress.audio_word = true;
                break;
            case WorkoutNames.puzzle:
                word.progress.word_puzzle = true;
                break;
        }
    }

    public void ResetWorkoutProgress()
    {
        foreach (WordLeo item in vocabulary.allWords)
        {
            item.ResetLicense();
        }
    }

    private void AddWordLicenses(WordLeo currentWord)
    {
        currentWord.progress.lastRepeat = DateTime.Now;
        currentWord.progress.license++;
    }

    void Observer.OnNotify(Component sender, GAME_EVENTS notificationName)
    {

        switch (notificationName)
        {
            case GAME_EVENTS.WordsEnded:
                SaveToXml();
                break;
            case GAME_EVENTS.CorrectAnswer:
                AddWorkoutProgress(currentWord, currentWorkoutName);
                AddWordLicenses(currentWord);
                break;
            case GAME_EVENTS.BuildTask:
                IWorkout workout = sender as IWorkout;
                currentWorkoutName = workout.WorkoutName;
                currentWord = workout.GetCurrentWord();
                break;
        }
    }
}