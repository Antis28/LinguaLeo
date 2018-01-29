using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class WordManeger : MonoBehaviour
{
    private static WordCollection vocabulary = null; // полный словарь
    private static List<string> wordGroups = null; // названия наборов слов

    [SerializeField]
    private string folder = @"Data/Base"; // подпапка в Resources, для чтения

    [SerializeField]
    private string fileName = string.Empty;


    public WordCollection GetVocabulary()
    {
        return vocabulary;
    }

    public List<WordGroup> GetGroupNames()
    {
        //return vocabulary.FilterGroup();
        string path = folder + "/" + "WordGroup.xml";
        if (!File.Exists(path))
        {
            Debug.LogError("File not found");
            return null;
        }
       return  DeserializeGroup(path);
    }

    public void LoadGroup(string groupName)
    {
        vocabulary.LoadGroup(groupName);
    }

    void Start()
    {
        LoadVocabulary();
        //CreateWordGroups();
    }

    private void LoadVocabulary()
    {
        if (vocabulary == null)
        {
            vocabulary = LoadFromXml();
        }

        wordGroups = vocabulary.FilterGroup();
        vocabulary.LoadGroup(wordGroups[66]);

        GameManager.Notifications.PostNotification(null, GAME_EVENTS.LoadedVocabulary);
    }

    private WordCollection LoadFromXml()
    {
        string path = folder + "/" + fileName;
        if (!File.Exists(path))
        {
            Debug.LogError("File not found");
            return null;
        }
        XmlSerializer Serializer = new XmlSerializer(typeof(WordCollection));
        FileStream Stream = new FileStream(path, FileMode.Open);
        WordCollection result = Serializer.Deserialize(Stream) as WordCollection;
        Stream.Close();
        return result;
    }

    private WordCollection LoadFromXml(string xmlString)
    {
        XmlSerializer Serializer = new XmlSerializer(typeof(WordCollection));

        TextReader reader = new StringReader(xmlString);
        WordCollection result = Serializer.Deserialize(reader) as WordCollection;
        reader.Close();
        return result;
    }

    private void SaveToXml(string FileName)
    {
        
        //AssetDatabase.GetAssetPath()
        //Now save game data
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(WordCollection));
        FileStream Stream = new FileStream(FileName, FileMode.Create);
        xmlSerializer.Serialize(Stream, vocabulary);
        Stream.Close();
    }

    public void CreateWordGroups()
    {
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

    private List<WordGroup> DeserializeGroup(string FileName)
    {
        //string FileName = "WordGroup.xml";
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