﻿using System.Collections.Generic;
using System.Xml;
using LinguaLeo.Scripts.Helpers;
using NUnit.Framework;

namespace Editor.Tests
{
    /// Правила именования Unit тестов:
    /// {UnitOfWorkName}_{Scenario}_{ExpectedBehaviour}
    /// 
    /// UnitOfWorkName  - имя метода, группы методов или класса, который тестируется.
    /// Scenario        - условие под котором тестируется, такое как: 
    ///                   "неправильный логин", "правильный пароль";
    ///                   или состояние системы: "нет пользователей", "пользователь существует"
    /// ExpectedBehaviour - что ожидается от метода под этими условиями:
    ///                     вернуть значение как результат(правильное или exception), изменить состояние системы

    ///  [Category("Fast Tests")]
    ///  [Category("Slow Tests")]
    /// 
    /// [OneTimeSetUp] - запуск один раз перед всеми тестами
    /// 

    [TestFixture]
    public class WordCollectionsTest
    {
        private WordCollection _wordCollection;
        private const string NameGroup = "Цитатник_7";
        private readonly int _countWordsInGroup = 49;
        private const string Path = @"Data/Base/WordBase.xml";

        [OneTimeSetUp]
        public void PrepareBase()
        {
            _wordCollection = WordCollection.BuildFromXml(Path);
        }

        [Test]
        [Category("Slow Tests")]
        public void BuildFromXml_IsNull_ReturnWordCollection()
        {
            _wordCollection = WordCollection.BuildFromXml(Path);

            Assert.IsNotNull(_wordCollection);
            Assert.IsInstanceOf<WordCollection>(_wordCollection);
        }

        [Test]
        [Category("Fast Tests")]
        public void BuildFromXml_BadXML_throwXmlException()
        {
            string pathToBug = @"Data/Base/WordBase_BUG.xml";

            Assert.Throws<XmlException>(
                () => WordCollection.BuildFromXml(pathToBug)
            );
        }

        [Test]
        [Category("Slow Tests")]
        public void SaveToXml_ValidPath_PathExists()
        {
            _wordCollection.SaveToXml(Path);
            FileAssert.Exists(Path);
        }

        [Test]
        [Category("Fast Tests")]
        public void LoadGroup_ValidNameGroup_ReturnListWordLeo()
        {
            _wordCollection.LoadGroup(NameGroup);
            Assert.IsInstanceOf<List<WordLeo>>(_wordCollection.wordsFromGroup);
        }

        [Test]
        [Category("Fast Tests")]
        public void GetRandomWord_ValidState_RetunWordLeo()
        {
            WordLeo randomWord = _wordCollection.GetRandomWord();
            Assert.IsInstanceOf<WordLeo>(randomWord);
        }

        [Test]
        [Category("Fast Tests")]
        public void GetRandomWords_CountWordsFive_RetunListWithFiveWordLeo()
        {
            int countWords = 5;
            List<WordLeo> words = _wordCollection.GetRandomWords(countWords);

            Assert.IsInstanceOf<List<WordLeo>>(words);
            Assert.AreEqual(words.Count, countWords);
        }

        [Test]
        [Category("Fast Tests")]
        public void GetRandomWordFromGroup_ValidGroup_ReturnWordLeo()
        {
            _wordCollection.LoadGroup(NameGroup);

            WordLeo word = _wordCollection.GetRandomWordFromGroup();
            Assert.IsInstanceOf<WordLeo>(word);
        }

        [Test]
        [Category("Fast Tests")]
        public void GetRandomWordsFromGroup_ValidGroup_RetunListWithFiveWordLeo()
        {
            int countWords = 5;
            _wordCollection.LoadGroup(NameGroup);
        
            List<WordLeo> words = _wordCollection.GetRandomWordsFromGroup(countWords);
            Assert.IsInstanceOf<List<WordLeo>>(words);
        }

        [Test]
        [Category("Fast Tests")]
        public void GetUntrainedGroupWords_AudioWorkout_RetunListWordLeo()
        {
            _wordCollection.LoadGroup(NameGroup);
            WorkoutNames workoutName = WorkoutNames.Audio;
        
            List<WordLeo> words = _wordCollection.GetUntrainedGroupWords(workoutName);
            Assert.IsInstanceOf<List<WordLeo>>(words);
        }

        [Test]
        [Category("Fast Tests")]
        public void GroupExist_ValidNameGroup_ReturnTrue()
        {
            _wordCollection.LoadGroup(NameGroup);
            bool isGroupExist = _wordCollection.GroupExist();
            Assert.IsTrue(isGroupExist);
        }

        [Test]
        [Category("Fast Tests")]
        public void FilterGroup_WordsLoaded_ReturnListGroupName()
        {
            List<string> listGroupName = _wordCollection.FilterGroup();
            Assert.IsInstanceOf<List<string>>(listGroupName);
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
}
