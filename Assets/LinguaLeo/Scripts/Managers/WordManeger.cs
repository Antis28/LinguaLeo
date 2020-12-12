using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using LinguaLeo.Adapters;
using LinguaLeo.Scripts.Helpers;
using LinguaLeo.Scripts.Helpers.Interfaces;
using LinguaLeo.Scripts.Manegers;
using UnityEngine;

namespace LinguaLeo.Scripts.Managers
{
    public class WordManeger : MonoBehaviour, IObserver
    {
        private readonly string folderXml = @"Data/Base";
        
        private static WordCollection vocabulary = null; // полный словарь
        private static List<string> wordGroups = null; // названия наборов слов

        private static List<WordLeo> currentWordGroups = null;

        private static string currentGroupName;
        private static List<WordGroup> groupNames;


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

        public int CountWordInGroup()
        {
            return currentWordGroups.Count;
        }

        public int CountUntrainWordInGroup()
        {
            var remainWord = vocabulary.SelectNotDoneWords();
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
        public void LoadStartWordGroup(string groupName)
        {
            currentGroupName = groupName;
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
            //    int count = GameManager.WordManeger.LoadVocabulary().wordsFromGroup.Count;

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

        void IObserver.OnNotify(object parametr, GAME_EVENTS notificationName)
        {

            switch (notificationName)
            {
                case GAME_EVENTS.WordsEnded:
                    /*
                    string path = folderXml + "/" + fileNameXml;
                    vocabulary.SaveToXml(path);
                    */
                    SaveVocabulary();
                    break;
                case GAME_EVENTS.QuitGame:
                    Settings.Instance.lastWordGroup = currentGroupName;
                    print("save settings");
                    Settings.SaveToXml();
                    break;
            }
        }

        void Start()
        {
            GameManager.Notifications.AddListener(this, GAME_EVENTS.WordsEnded);
            GameManager.Notifications.AddListener(this, GAME_EVENTS.QuitGame);

            LoadVocabulary();
            //CreateWordGroups();
            //ResetLicense();
        }

        private void LoadVocabulary()
        {
            if (vocabulary == null)
            {
                vocabulary = GameManager.ResourcesLoader.LoadVocabulary();
            }

            wordGroups = vocabulary.FilterGroup();

            // Здесь происходит загрузка стартового набора слов
            //vocabulary.LoadGroup(wordGroups[66]);
            //vocabulary.LoadGroup(wordGroups[23]);
            LoadStartWordGroup();

            SceneManagerAdapt.AddSceneLoaded(SceneManager_sceneLoaded);
            StartCoroutine(LoadedVocalubary());
        }

        private void SaveVocabulary()
        {
            GameManager.ResourcesLoader.SaveVocabulary(vocabulary);
        }

        private static void LoadStartWordGroup()
        {
            Settings.LoadFromXml();
            string lastWordGroup = Settings.Instance.lastWordGroup;
            if (lastWordGroup != null)
            {
                vocabulary.LoadGroup(lastWordGroup);
                print("LoadGroup = " + lastWordGroup);
            }
            else
            {
                vocabulary.LoadGroup(wordGroups[23]);
                print(wordGroups[23]);
            }
        }

        private void SceneManager_sceneLoaded()
        {
            StartCoroutine(LoadedVocalubary());
        }

        IEnumerator LoadedVocalubary()
        {
            yield return null;
            if (vocabulary != null)
                GameManager.Notifications.PostNotification(this, GAME_EVENTS.LoadedVocabulary);
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
}