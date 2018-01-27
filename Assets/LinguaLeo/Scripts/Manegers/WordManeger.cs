using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WordManeger : MonoBehaviour
{
    private static WordCollection vocabulary = null; // полный словарь
    private static List<string> wordGroups = null; // названия наборов слов

    [SerializeField]
    private string folder = "Base"; // подпапка в Resources, для чтения

    [SerializeField]
    private string fileName = string.Empty;


    public WordCollection GetVocabulary()
    {
        return vocabulary;
    }

    public List<string> GetGroupNames()
    {
        return vocabulary.FilterGroup();
    }

    public void LoadGroup(string groupName)
    {
        vocabulary.LoadGroup(groupName);
    }

    void Start()
    {
        LoadVocabulary();
        CreateWordGroups();
    }

    private void LoadVocabulary()
    {
        if (vocabulary == null)
        {
            TextAsset binary = Resources.Load<TextAsset>(folder + "/" + fileName);
            if (binary == null)
            {
                Debug.LogError("File not found");
                return;
            }
            //WordCollection words = LoadWords(binary.text);
            vocabulary = LoadFromXml(binary.text);
        }

        wordGroups = vocabulary.FilterGroup();
        vocabulary.LoadGroup(wordGroups[66]);

        GameManager.Notifications.PostNotification(null, GAME_EVENTS.LoadedVocabulary);
    }

    private WordCollection LoadFromXml(string xmlString)
    {
        XmlSerializer Serializer = new XmlSerializer(typeof(WordCollection));

        TextReader reader = new StringReader(xmlString);
        WordCollection result = Serializer.Deserialize(reader) as WordCollection;
        reader.Close();
        return result;
    }

    public void CreateWordGroups()
    {
        List<string> groupNames = GetGroupNames();
        List<WordGroup> groups = new List<WordGroup>();
        foreach (string name in groupNames)
        {
            LoadGroup(name);
            int count = GameManager.WordManeger.GetVocabulary().wordsFromGroup.Count;
            List<WordProgress> wordProgress = new List<WordProgress>();
            foreach (WordLeo item in GameManager.WordManeger.GetVocabulary().wordsFromGroup)
            {
                wordProgress.Add(new WordProgress() { word = item.wordValue });
            }

            groups.Add(new WordGroup() {
                name = name,
                wordCount = count,
                progress = wordProgress,
                pictureName = "504"
            });
        }
        SerializeGroup(groups);
    }

    private List<WordGroup> DeserializeGroup()
    {
        string FileName = "WordGroup.xml";
        FileStream stream = new FileStream(FileName, FileMode.Open);

        XmlSerializer Serializer = new XmlSerializer(typeof(List<WordGroup>));

        TextReader reader = new StreamReader(stream);
        List<WordGroup> result = Serializer.Deserialize(reader) as List<WordGroup>;
        reader.Close();
        Debug.Log("DeserializeGroup");
        return result;
    }

    private void SerializeGroup(List<WordGroup> list)
    {
        string FileName = "WordGroup.xml";

        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<WordGroup>));
        FileStream Stream = new FileStream(FileName, FileMode.Create);
        xmlSerializer.Serialize(Stream, list);
        Stream.Close();
        Debug.Log("SerializeGroup");
    }
}