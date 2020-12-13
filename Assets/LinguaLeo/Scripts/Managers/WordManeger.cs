using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using LinguaLeo._Adapters;
using LinguaLeo.Scripts.Helpers;
using LinguaLeo.Scripts.Helpers.Interfaces;
using UnityEngine;

namespace LinguaLeo.Scripts.Managers
{
    public class WordManeger : MonoBehaviour, IObserver
    {
        #region Static Fields and Constants

        private static WordCollection vocabulary = null; // полный словарь
        private static List<string> wordGroups = null;   // названия наборов слов

        private static List<WordLeo> currentWordGroups = null;

        private static string currentGroupName;
        private static List<WordGroup> groupNames;

        #endregion

        #region Events

        void IObserver.OnNotify(object parametr, GAME_EVENTS notificationName)
        {
            switch (notificationName)
            {
                case GAME_EVENTS.WordsEnded:
                    SaveVocabulary();
                    break;
                case GAME_EVENTS.QuitGame:
                    Settings.Instance.lastWordGroup = currentGroupName;
                    print("save settings");
                    Settings.SaveToXml();
                    break;
            }
        }

        #endregion

        #region Unity events

        private void Start()
        {
            GameManager.Notifications.AddListener(this, GAME_EVENTS.WordsEnded);
            GameManager.Notifications.AddListener(this, GAME_EVENTS.QuitGame);

            LoadVocabulary();
            //CreateWordGroups();
            //ResetLicense();
        }

        #endregion

        #region Public Methods

        public int CountUntrainWordInGroup()
        {
            var remainWord = vocabulary.SelectNotDoneWords();
            return remainWord.Count;
        }

        public int CountWordInGroup()
        {
            return currentWordGroups.Count;
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

        /// <summary>
        /// получить все слова из набора
        /// </summary>
        /// <returns></returns>
        public List<WordLeo> GetAllGroupWords()
        {
            return vocabulary.wordsFromGroup;
        }


        public List<WordLeo> GetAllWords()
        {
            return vocabulary.allWords;
        }

        /// <summary>
        /// получить описание наборов слов
        /// </summary>
        /// <returns>описание наборов слов</returns>
        public List<WordGroup> GetGroupNames()
        {
            return groupNames ?? (groupNames = GameManager.ResourcesLoader.LoadWordGroup());
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

        /// <summary>
        /// Загружает набор слов из словаря
        /// </summary>
        /// <param name="groupName"></param>
        public void LoadStartWordGroup(string groupName)
        {
            currentGroupName = groupName;
            vocabulary.LoadGroup(groupName);
        }

        public void ResetLicense()
        {
            foreach (WordLeo item in vocabulary.allWords) { item.ResetLicense(); }
        }

        #endregion

        #region Private Methods

        private List<WordGroup> DeserializeGroup(string FileName)
        {
            //string FileName = "WordGroup.xml";

            using (TextReader stream = new StreamReader(FileName, Encoding.UTF8)
            ) // (path, FileMode.Open, FileAccess.Read))
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

        private IEnumerator LoadedVocalubary()
        {
            yield return null;
            if (vocabulary != null)
                GameManager.Notifications.PostNotification(this, GAME_EVENTS.LoadedVocabulary);
        }

        private static void LoadStartWordGroup()
        {
            Settings.LoadFromXml();
            string lastWordGroup = Settings.Instance.lastWordGroup;
            if (lastWordGroup != null)
            {
                vocabulary.LoadGroup(lastWordGroup);
                print("LoadGroup = " + lastWordGroup);
            } else
            {
                vocabulary.LoadGroup(wordGroups[23]);
                print(wordGroups[23]);
            }
        }

        private void LoadVocabulary()
        {
            if (vocabulary == null) { vocabulary = GameManager.ResourcesLoader.LoadVocabulary(); }

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

        private void SceneManager_sceneLoaded()
        {
            StartCoroutine(LoadedVocalubary());
        }

        private void SerializeGroup(List<WordGroup> list, string fileName = "WordGroup.xml")
        {
            using (TextWriter stream = new StreamWriter(fileName, false, Encoding.UTF8)
            ) // (path, FileMode.Open, FileAccess.Read))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<WordGroup>));
                xmlSerializer.Serialize(stream, list);
                stream.Close();
                Debug.Log("SerializeGroup");
            }
        }

        #endregion
    }
}
