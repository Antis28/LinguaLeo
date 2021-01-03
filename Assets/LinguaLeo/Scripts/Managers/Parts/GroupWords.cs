#region

using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using LinguaLeo.Scripts.Helpers;

#endregion

namespace LinguaLeo.Scripts.Managers.Parts
{
    public class GroupWords
    {
        #region Public variables

        public string CurrentGroupName => currentGroupName;

        #endregion

        #region Private variables

        private readonly WordCollection vocabulary = null; // полный словарь
        private string currentGroupName;
        private List<WordGroup> groupNames;
        private List<WordLeo> currentWordGroups = null;
        private readonly List<string> wordGroups = null; // названия наборов слов

        #endregion

        #region Public Methods

        public int CountUntrainedWordInGroup()
        {
            var remainWord = vocabulary.SelectNotDoneWords();
            return remainWord.Count;
        }

        public int CountWordInGroup()
        {
            return currentWordGroups.Count;
        }

        /// <summary>
        /// получить описание наборов слов
        /// </summary>
        /// <returns>описание наборов слов</returns>
        public List<WordGroup> GetGroupNames()
        {
            return groupNames ?? (groupNames = GameManager.ResourcesManager.GetWordGroup());
        }

        /// <summary>
        /// получить все слова из набора
        /// </summary>
        /// <returns></returns>
        public List<WordLeo> GetAllGroupWords()
        {
            return vocabulary.wordsFromGroup;
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
        /// Загружает набор слов из словаря
        /// </summary>
        /// <param name="groupName"></param>
        public void LoadStartWordGroup(string groupName)
        {
            currentGroupName = groupName;
            vocabulary.LoadGroup(groupName);
        }

        #endregion

        #region Private Methods

        private void LoadStartWordGroup()
        {
            Settings.LoadFromXml();
            var lastWordGroup = Settings.Instance.lastWordGroup;
            vocabulary.LoadGroup(lastWordGroup ?? wordGroups[23]);
        }

        private void SerializeGroup(List<WordGroup> list, string fileName = "WordGroup.xml")
        {
            using (TextWriter stream = new StreamWriter(fileName, false, Encoding.UTF8)
            ) // (path, FileMode.Open, FileAccess.Read))
            {
                var xmlSerializer = new XmlSerializer(typeof(List<WordGroup>));
                xmlSerializer.Serialize(stream, list);
                stream.Close();
            }
        }

        #endregion


        public GroupWords(WordCollection vocabulary)
        {
            this.vocabulary = vocabulary;
            wordGroups = this.vocabulary.FilterGroup();
            // Здесь происходит загрузка стартового набора слов
            //vocabulary.LoadGroup(wordGroups[66]);
            //vocabulary.LoadGroup(wordGroups[23]);
            LoadStartWordGroup();
        }
    }
}
