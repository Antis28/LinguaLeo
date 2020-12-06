﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace LinguaLeo.Scripts.Helpers.ResourceLoading
{
    public static class ResourcesLoader
    {
        private static string pathToRootResources = string.Empty;

        static ResourcesLoader() { SelectPathByPlatform(); }

        public static string PathToRootResources => pathToRootResources;

        private static void SelectPathByPlatform()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            //Android uses files inside a compressed APK/JAR file
            //To read streaming Assets on platforms like Android and WebGL
            //, where you cannot access streaming Asset files directly, use UnityWebRequest.
            //For an example, see Application.streamingAssetsPath. On many platforms, the streaming assets folder
            //location is read-only; you can not modify or write new files there at runtime.
            //Use Application.persistentDataPath for a folder location that is writable.
            pathToRootResources = "jar:file://" + Application.dataPath + "!/assets";
#endif

#if UNITY_EDITOR ||  UNITY_STANDALONE
            // Most platforms (Unity Editor, Windows, Linux players, PS4, Xbox One, Switch) use Application.dataPath + "/StreamingAssets"
            pathToRootResources = Path.Combine(Application.dataPath, "StreamingAssets");
#endif
            pathToRootResources = Path.Combine(pathToRootResources, "Data");
        }
        
        public static string ConverterUrlToName(string url, bool withExtension = true)
        {
            //string url = "http://contentcdn.lingualeo.com/uploads/picture/3466359.png";
            //string url = "http://contentcdn.lingualeo.com/uploads/picture/96-631152008.mp3";
            const string patern = @"(\d+.png$)|(\d+-\d+.mp3$)";
            var rg = new Regex(patern, RegexOptions.IgnoreCase);
            var mat = rg.Match(url);

            if (withExtension)
                return Path.GetFileName(mat.Value);

            return Path.GetFileNameWithoutExtension(mat.Value);
        }
    }

    public class MyUtilities
    {
        public static string GetFirstTranslate(WordLeo questWord) { return questWord.translations.Split(',')[0]; }
        public static string[] GetTranslates(WordLeo questWord) { return questWord.translations.Split(','); }

        public static string FormatTime(TimeSpan timeLeft)
        {
            string result = string.Empty;
            if (timeLeft.Days > 0)
                result += timeLeft.Days + "д.";
            if (timeLeft.Hours > 0)
                result += timeLeft.Hours + " ч.";
            if (timeLeft.Minutes > 0)
                result += timeLeft.Minutes + " м.";
            if (result == string.Empty)
                result = "0";
            return result;
        }

        public static string FormatTime(double timeLeft)
        {
            var span = TimeSpan.FromMinutes(timeLeft);
            return FormatTime(span);
        }

        public int GetINT(string text)
        {
            int value;
            if (int.TryParse(text, out value)) { return value; }

            return 0;
        }

        public bool GetBOOL(string text)
        {
            bool value;
            if (bool.TryParse(text, out value)) { return value; }

            return false;
        }

        /// <summary>
        /// Выбрать слова которые не полностью изучены
        /// </summary>
        /// <param name="wordsFromGroup"></param>
        /// <returns></returns>
        internal static List<WordLeo> SelectNotDoneWords(List<WordLeo> wordsFromGroup)
        {
            var remainList = from word in wordsFromGroup
                             where !word.AllWorkoutDone()
                             select word;
            return remainList.ToList();
        }

        /// <summary>
        /// перемешать слова
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        public static List<WordLeo> ShuffleList(List<WordLeo> words)
        {
            List<WordLeo> list = new List<WordLeo>(words);

            System.Random random = new System.Random();

            for (int i = list.Count; i > 1; i--)
            {
                int j = random.Next(i);
                list.Add(list[j]);
                list.RemoveAt(j);
            }

            return list;
        }

        /// <summary>
        /// перетосовать колоду
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cards"></param>
        public static void ShuffleDeck<T>(List<T> cards)
        {
            System.Random generator = new System.Random();
            int n = cards.Count;
            while (n > 1)
            {
                int rndValue = generator.Next(n);
                --n;
                T temp = cards[n];
                cards[n] = cards[rndValue];
                cards[rndValue] = temp;
            }
        }


        public static List<WordLeo> SortWordsByProgress(List<WordLeo> words)
        {
            var result = from word in words
                         orderby word.GetProgressCount() descending,
                             word.GetLicense() descending,
                             word.GetLicenseValidityTime()
                         select word;

            var sortedWordGroups = result.ToList();
            return sortedWordGroups;
        }

        /// <summary>
        /// Интерполяционный поиск
        ///  Возвращает индекс элемента со значением toFind или -1, 
        ///  если такого элемента не существует
        /// </summary>
        /// <param name="sortedArray"></param>
        /// <param name="toFind"></param>
        /// <returns></returns>
        public int InterpolationSearch(int[] sortedArray, int toFind)
        {
            int mid;
            int low = 0;
            int high = sortedArray.Length - 1;

            while (sortedArray[low] < toFind && sortedArray[high] > toFind)
            {
                mid = low + ((toFind - sortedArray[low]) * (high - low)) / (sortedArray[high] - sortedArray[low]);

                if (sortedArray[mid] < toFind)
                    low = mid + 1;
                else if (sortedArray[mid] > toFind)
                    high = mid - 1;
                else
                    return mid;
            }

            if (sortedArray[low] == toFind)
                return low;
            else if (sortedArray[high] == toFind)
                return high;
            else
                return -1; // Not found
        }

        public static T FindComponentInGO<T>(string nameGO)
        {
            GameObject go = GameObject.Find(nameGO);
            if (go)
                return go.GetComponent<T>();
            return default;
        }
    }
}
