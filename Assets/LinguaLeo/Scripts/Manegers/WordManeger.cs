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

    public string folder = "Base"; // подпапка в Resources, для чтения

    [SerializeField]
    private string fileName = string.Empty;

    public static WordCollection Vocabulary
    {
        get
        {
            if (vocabulary == null)
            {
                Debug.LogError("vocabulary == null");
            }
            return vocabulary;
        }

        set
        {
            vocabulary = value;
        }
    }

    public void Start()
    {
        LoadVocabulary();
    }

    

    void SetExitDialogue(Button button) // событие, для выхода из диалога
    {
        //button.onClick.AddListener(() => CloseWindow());
        print("присвоено событие, для выхода из диалога");
    }

    private void LoadVocabulary()
    {
        if (vocabulary == null)
        {
            TextAsset binary = Resources.Load<TextAsset>(folder + "/" + fileName);
            if (binary == null)
            {
                Debug.LogWarning("File not found");
                return;
            }
            //WordCollection words = LoadWords(binary.text);
            vocabulary = LoadFromXml(binary.text);
        }
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