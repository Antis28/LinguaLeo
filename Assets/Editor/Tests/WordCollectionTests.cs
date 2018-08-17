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
    private WordCollection _wordCollection;
    private const string NameGroup = "Цитатник_7";
    private int _countWordsInGroup = 49;
    private const string Path = @"Data/Base/WordBase.xml";

    [OneTimeSetUp]
    public void PrepareBase()
    {
        _wordCollection = WordCollection.BuildFromXml(Path);
    }

    [Test]
    public void BuildFromXml()
    {
        Assert.IsNotNull(_wordCollection);
        Assert.IsInstanceOf<WordCollection>(_wordCollection);
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
        _wordCollection.SaveToXml(Path);
        FileAssert.Exists(Path);

        _wordCollection = WordCollection.BuildFromXml(Path);
    }

    [Test]
    public void LoadGroup()
    {
        _wordCollection.LoadGroup(NameGroup);
        Assert.IsNotNull(_wordCollection.wordsFromGroup);
    }

    [Test]
    public void GetRandomWord()
    {
        WordLeo randomWord = _wordCollection.GetRandomWord();
        Assert.IsInstanceOf<WordLeo>(randomWord);
    }

    [Test]
    public void GetRandomWords()
    {
        int countWords = 5;
        List<WordLeo> words = _wordCollection.GetRandomWords(countWords);

        Assert.IsInstanceOf<List<WordLeo>>(words);
        Assert.AreEqual(words.Count, countWords);
    }

    [Test]
    public void GetRandomWordFromGroup()
    {
        _wordCollection.LoadGroup(NameGroup);

        WordLeo word = _wordCollection.GetRandomWordFromGroup();
        Assert.IsInstanceOf<WordLeo>(word);
    }

    [Test]
    public void GetRandomWordsFromGroup()
    {
        int countWords = 5;
        _wordCollection.LoadGroup(NameGroup);
        
        List<WordLeo> words = _wordCollection.GetRandomWordsFromGroup(countWords);
        Assert.IsInstanceOf<List<WordLeo>>(words);
    }

    [Test]
    public void GetUntrainedGroupWords()
    {
        _wordCollection.LoadGroup(NameGroup);
        WorkoutNames workoutName = WorkoutNames.Audio;
        
        List<WordLeo> words = _wordCollection.GetUntrainedGroupWords(workoutName);
        Assert.IsInstanceOf<List<WordLeo>>(words);
    }

    [Test]
    public void GroupExist()
    {
        _wordCollection.LoadGroup(NameGroup);
        bool isGroupExist = _wordCollection.GroupExist();
        Assert.IsTrue(isGroupExist);
    }

    [Test]
    public void FilterGroup()
    {
        _wordCollection.LoadGroup(NameGroup);
        List<string> listGroupName = _wordCollection.FilterGroup();
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
