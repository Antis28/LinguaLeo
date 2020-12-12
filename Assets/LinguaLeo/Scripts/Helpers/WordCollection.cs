using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using LinguaLeo.Scripts.Helpers.Interfaces;
using LinguaLeo.Scripts.Helpers.ResourceLoading.XmlImplementation;

namespace LinguaLeo.Scripts.Helpers
{
    public class WordCollection
    {
        public List<WordLeo> allWords; // полный словарь
        public List<WordLeo> wordsFromGroup; // словарь для набора слов

        private readonly Random random = new Random();

        public WordCollection()
        {
        }

        public void SaveToXml(string path)
        {
            if (!File.Exists(path))
                throw new Exception("File not founded");

            using (TextWriter stream = new StreamWriter(path, false, Encoding.UTF8))
            {
                //Now save game data
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(WordCollection));

                xmlSerializer.Serialize(stream, this);
                stream.Close();
            }
        }

        /// <summary>
        /// Загрука слов из набора слов
        /// </summary>
        /// <param name="groupName">название набора слов</param>
        public void LoadGroup(string groupName)
        {
            wordsFromGroup = new List<WordLeo>();
            var query = allWords.Where(word => word.groups.Contains(groupName));
            foreach (var word in query) { wordsFromGroup.Add(word); }
        }

        public WordLeo GetRandomWord()
        {
            int randomIndex = random.Next(allWords.Count);

            WordLeo word = allWords[randomIndex];
            return word;
        }

        public List<WordLeo> GetRandomWords(int Count)
        {
            List<WordLeo> words = new List<WordLeo>(Count);
            for (int i = 0; i < Count; i++) { words.Add(GetRandomWord()); }

            return words;
        }

        public WordLeo GetRandomWordFromGroup()
        {
            int randomIndex = random.Next(wordsFromGroup.Count);

            WordLeo word = wordsFromGroup[randomIndex];
            return word;
        }

        public List<WordLeo> GetRandomWordsFromGroup(int Count)
        {
            List<WordLeo> words = new List<WordLeo>(Count);
            for (int i = 0; i < Count; i++) { words.Add(GetRandomWordFromGroup()); }

            return words;
        }

        public List<WordLeo> GetUntrainedGroupWords(WorkoutNames workoutName)
        {
            List<WordLeo> untrainedWords = new List<WordLeo>();
            foreach (var item in wordsFromGroup)
            {
                item.LicenseExpirationCheck();

                if (item.CanTraining(workoutName))
                    untrainedWords.Add(item);
            }

            return untrainedWords;
        }

        public bool GroupExist() { return wordsFromGroup != null; }

        /// <summary>
        /// Извлекает название всех групп
        /// </summary>
        /// <returns>список групп</returns>
        public List<string> FilterGroup()
        {
            List<string> groups = new List<string>();

            foreach (var word in allWords)
            {
                foreach (var group in word.groups)
                {
                    if (!groups.Contains(group))
                        groups.Add(group);
                }
            }

            groups = groups.OrderBy((x) => x).ToList();
            return groups;
        }

        /// <summary>
        /// Выбрать слова которые не полностью изучены
        /// </summary>
        /// <param name="wordsFromGroup"></param>
        /// <returns></returns>
        internal List<WordLeo> SelectNotDoneWords()
        {
            var remainList = from word in wordsFromGroup
                             where !word.AllWorkoutDone()
                             select word;
            return remainList.ToList();
        }
    }
}
