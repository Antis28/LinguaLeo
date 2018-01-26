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
}