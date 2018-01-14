using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

using URandom = UnityEngine.Random;

public class Utilities
{

    public static string ConverterUrlToName(string url)
    {
        //string url = "http://contentcdn.lingualeo.com/uploads/picture/3466359.png";
        //string url = "http://contentcdn.lingualeo.com/uploads/picture/96-631152008.mp3";
        string patern = @"(\d+.png$)|(\d+-\d+.mp3$)";
        Regex rg = new Regex(patern, RegexOptions.IgnoreCase);
        Match mat = rg.Match(url);

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
