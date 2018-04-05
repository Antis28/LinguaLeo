using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;


using URandom = UnityEngine.Random;


public class Utilities
{
    public static Sprite LoadSpriteFromFile(string path)
    {
        //string path = "Data/Covers" + "/" + pictureName + ".png";
        if (!File.Exists(path))
        {
            Debug.LogWarning("File not found\n" + path);
            path = "Data/Picture" + "/" + "image-not-found.png";
            if (!File.Exists(path))
                return null;
        }

        byte[] picture;

        using (FileStream stream = new FileStream(path, FileMode.Open))
        {
            picture = new byte[stream.Length];
            // считываем данные
            stream.Read(picture, 0, picture.Length);
        }

        Texture2D texture2D = new Texture2D(1, 1);
        texture2D.LoadImage(picture);

        Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(.5f, .5f));
        return sprite;
    }

    public static AudioClip LoadMusicFromFile(string path)
    {
        if (!File.Exists(path))
            return null;

        WWW www = new WWW("file://" + path);// задаем ссылку на файл
        //Debug.Log("file://" + path);

        AudioClip clip = www.GetAudioClip(false, false);//[/url] получаем по ссылке аудиоклип        
        return clip;
    }

    public static string ConverterUrlToName(string url, bool withExtension = true)
    {
        //string url = "http://contentcdn.lingualeo.com/uploads/picture/3466359.png";
        //string url = "http://contentcdn.lingualeo.com/uploads/picture/96-631152008.mp3";
        string patern = @"(\d+.png$)|(\d+-\d+.mp3$)";
        Regex rg = new Regex(patern, RegexOptions.IgnoreCase);
        Match mat = rg.Match(url);

        if (withExtension)
            return Path.GetFileName(mat.Value);

        return Path.GetFileNameWithoutExtension(mat.Value);

    }

    public int GetINT(string text)
    {
        int value;
        if (int.TryParse(text, out value))
        {
            return value;
        }
        return 0;
    }

    public bool GetBOOL(string text)
    {
        bool value;
        if (bool.TryParse(text, out value))
        {
            return value;
        }
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
    public static List<WordLeo> SortWordsByProgress(List<WordLeo> words)
    {
        var result = from word in words
                     orderby word.GetProgressCount() descending,
                            word.GetLicenseValidityTime(),
                            word.GetLicense() descending
                     select word;

        List<WordLeo> sortedWordGroups = result.ToList();
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
}

public class MyComparer : IComparer
{
    public int Compare(object x, object y)
    {
        ButtonComponent lVal = x as ButtonComponent;
        ButtonComponent rValt = y as ButtonComponent;

        return Compare(lVal.gameObject, rValt.gameObject);
    }

    // Call CaseInsensitiveComparer.Compare with the parameters reversed.
    public int Compare(GameObject x, GameObject y)
    {
        return (new CaseInsensitiveComparer()).Compare(x.name, y.name);
    }
}

class UniqRandom
{
    readonly int MAX_COUNT;
    List<int> lastIndex;

    UniqRandom(int max)
    {
        MAX_COUNT = max;
        lastIndex = new List<int>(MAX_COUNT);
    }

    int nextRandom()
    {
        int rndValue = -1;

        do
        {
            rndValue = URandom.Range(0, MAX_COUNT);
        } while (lastIndex.Contains(rndValue));

        return rndValue;
    }
}



