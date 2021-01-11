#region

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Helpers;
using Helpers.Interfaces;
using LinguaLeo._Adapters;
using Managers.Parts;
using UnityEngine;

#endregion

namespace Managers
{
    public class WordManager : MonoBehaviour, IObserver
    {
        #region Public variables

        public bool VocabularyReady { get; private set; } = false;

        #endregion

        #region Private variables

        private WordCollection vocabulary = null; // полный словарь
        private GroupWords groupWords = null;

        #endregion

        #region Events

        void IObserver.OnNotify(object parameter, GAME_EVENTS notificationName)
        {
            switch (notificationName)
            {
                case GAME_EVENTS.WordsEnded:
                    SaveVocabulary();
                    break;
                case GAME_EVENTS.QuitGame:
                    Settings.Instance.lastWordGroup = groupWords.CurrentGroupName;
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

        public int CountUntrainedWordInGroup()
        {
            return groupWords.CountUntrainedWordInGroup();
        }

        public int CountWordInGroup()
        {
            return groupWords.CountWordInGroup();
        }

        public List<WordGroup> GetGroupNames()
        {
            return groupWords.GetGroupNames();
        }

        public List<WordLeo> GetAllWords()
        {
            return vocabulary.allWords;
        }

        public List<WordLeo> GetWordsWithLicense()
        {
            var allWords = GetAllWords();
            var wordsByLicense = new List<WordLeo>();

            foreach (var word in allWords)
            {
                word.LicenseExpirationCheck();
                var allWorkoutDone = word.AllWorkoutDone();
                var license = word.LicenseExists();

                //if (!word.CanbeRepeated())
                if (allWorkoutDone || !license)
                    continue;
                wordsByLicense.Add(word);
            }

            return wordsByLicense;
        }

        public List<WordLeo> GetAllGroupWords()
        {
            return groupWords.GetAllGroupWords();
        }

        public List<WordLeo> GetUntrainedGroupWords(WorkoutNames workoutName)
        {
            return groupWords.GetUntrainedGroupWords(workoutName);
        }


        public void ResetLicense()
        {
            foreach (var item in vocabulary.allWords) { item.ResetLicense(); }
        }

        public void LoadStartWordGroup(string groupName)
        {
            groupWords.LoadStartWordGroup(groupName);
        }

        #endregion

        #region Private Methods

        private List<WordGroup> DeserializeGroup(string FileName)
        {
            //string FileName = "WordGroup.xml";

            using (TextReader stream = new StreamReader(FileName, Encoding.UTF8)
            ) // (path, FileMode.Open, FileAccess.Read))
            {
                var Serializer = new XmlSerializer(typeof(List<WordGroup>));
                var result = Serializer.Deserialize(stream) as List<WordGroup>;
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
            while (vocabulary == null) { yield return null; }

            VocabularyReady = true;
            GameManager.Notifications.PostNotification(this, GAME_EVENTS.LoadedVocabulary);
        }


        private void LoadVocabulary()
        {
            if (vocabulary == null) { vocabulary = GameManager.ResourcesManager.GetVocabulary(); }

            groupWords = new GroupWords(vocabulary);
            SceneManagerAdapt.AddSceneLoaded(SceneManager_sceneLoaded);
            StartCoroutine(LoadedVocalubary());
        }

        private void SaveVocabulary()
        {
            GameManager.ResourcesManager.SaveVocabulary(vocabulary);
        }

        private void SceneManager_sceneLoaded()
        {
            StartCoroutine(LoadedVocalubary());
        }

        #endregion
    }
}
