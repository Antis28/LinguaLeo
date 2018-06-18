using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Xml;

[TestFixture]
public class WordCollectionsTest
{
    private WordCollection wordCollection;
    private string nameGroup = "Цитатник_7";
    private int countWordsInGroup = 49;
    string path = @"Data/Base/WordBase.xml";

    [OneTimeSetUp]
    public void PrepareBase()
    {
        wordCollection = WordCollection.BuildFromXml(path);
    }

    [Test]
    public void BuildFromXml()
    {
        Assert.IsNotNull(wordCollection);
        Assert.IsInstanceOf<WordCollection>(wordCollection);
    }

    [Test]
    public void BuildFromXml_XmlException()
    {
        string pathToBug = @"Data/Base/WordBase_BUG.xml";

        Assert.Throws<XmlException>(
            () => WordCollection.BuildFromXml(pathToBug)
            );
    }

    [Test]
    public void SaveToXml()
    {
        wordCollection.SaveToXml(path);
        FileAssert.Exists(path);

        wordCollection = WordCollection.BuildFromXml(path);
    }

    [Test]
    public void LoadGroup()
    {
        wordCollection.LoadGroup(nameGroup);
        Assert.IsNotNull(wordCollection.wordsFromGroup);
    }

    [Test]
    public void GetRandomWord()
    {
        WordLeo randomWord = wordCollection.GetRandomWord();
        Assert.IsInstanceOf<WordLeo>(randomWord);
    }

    [Test]
    public void GetRandomWords()
    {
        int countWords = 5;
        List<WordLeo> words = wordCollection.GetRandomWords(countWords);

        Assert.IsInstanceOf<List<WordLeo>>(words);
        Assert.AreEqual(words.Count, countWords);
    }

    [Test]
    public void GetRandomWordFromGroup()
    {
        wordCollection.LoadGroup(nameGroup);

        WordLeo word = wordCollection.GetRandomWordFromGroup();
        Assert.IsInstanceOf<WordLeo>(word);
    }

    [Test]
    public void GetRandomWordsFromGroup()
    {
        int countWords = 5;
        wordCollection.LoadGroup(nameGroup);
        
        List<WordLeo> words = wordCollection.GetRandomWordsFromGroup(countWords);
        Assert.IsInstanceOf<List<WordLeo>>(words);
    }

    [Test]
    public void GetUntrainedGroupWords()
    {
        wordCollection.LoadGroup(nameGroup);
        WorkoutNames workoutName = WorkoutNames.Audio;
        
        List<WordLeo> words = wordCollection.GetUntrainedGroupWords(workoutName);
        Assert.Fail();
    }

    [Test]
    public void GroupExist()
    {
        wordCollection.LoadGroup(nameGroup);
        bool isGroupExist = wordCollection.GroupExist();
        Assert.IsTrue(isGroupExist);
    }

    [Test]
    public void FilterGroup()
    {
        wordCollection.LoadGroup(nameGroup);
        List<string> listGroupName = wordCollection.FilterGroup();
        Assert.IsNotNull(listGroupName);
    }


    private WordCollection BuildMockCollection()
    {
        WordCollection mockWordCollection = new WordCollection();

        WordLeo word = new WordLeo()
        {
            wordValue = "metaphor",
            translations = "метафора",
            pictureURL = "http://contentcdn.lingualeo.com/uploads/picture/3250267.png",
            transcription = "ˈmɛtəfɔr",
            highlightedContext = "",
            soundURL = "https://audiocdn.lingualeo.com/v2/1/25559-631152008.mp3",
            clozefiedContext = "",
            groups = new[] { "Литература" }
        };

        word.progress = new WorkoutProgress();

        mockWordCollection.allWords.Add(word);
        return mockWordCollection;
    }
}
