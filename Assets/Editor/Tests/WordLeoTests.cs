using System;
using LinguaLeo.Scripts.Helpers;
using LinguaLeo.Scripts.Helpers.Licences;
using NUnit.Framework;

namespace Editor.Tests
{
    [TestFixture]
    public class WordLeoTests
    {
        #region Private variables

        private WordLeo word;

        #endregion

        #region Public Methods

        [Test]
        public void AddLicenseLevel()
        {
            Assert.Fail();
        }

        [Test]
        public void AllWorkoutDone()
        {
            Assert.Fail();
        }

        [Test]
        public void CanbeRepeated()
        {
            Assert.Fail();
        }

        [Test]
        public void CanTraining()
        {
            Assert.Fail();
        }

        [Test]
        public void GetLicense()
        {
            var level = word.GetLicense();
            Assert.IsInstanceOf<LicenseLevels>(level);
        }

        [Test]
        public void GetLicenseExpiration()
        {
            Assert.Fail();
        }

        [Test]
        public void GetLicenseValidityTime()
        {
            Assert.Fail();
        }

        [Test]
        public void GetProgressCount()
        {
            Assert.Fail();
        }

        [Test]
        public void LearnAudio()
        {
            Assert.Fail();
        }

        [Test]
        public void LearnPuzzle()
        {
            Assert.Fail();
        }

        [Test]
        public void LearnTranslateWord()
        {
            Assert.Fail();
        }

        [Test]
        public void LearnWordTranslate()
        {
            Assert.Fail();
        }

        [Test]
        public void LicenseExists()
        {
            Assert.Fail();
        }

        [Test]
        public void LicenseExpirationCheck()
        {
            Assert.Fail();
        }

        [Test]
        public void LicenseValidityCheck()
        {
            word.LicenseValidityCheck();
            Assert.Fail();
        }

        [OneTimeSetUp]
        public void Prepare()
        {
            word = new WordLeo();
            word.progress = new WorkoutProgress();
            word.progress.license = LicenseLevels.Level5;
        }

        [Test]
        public void ReduceLicense()
        {
            var beforeLevel = word.GetLicense();
            word.ReduceLicense();
            var afterLevel = word.GetLicense();
            Assert.AreNotEqual(beforeLevel, afterLevel);
        }

        [Test]
        public void ResetLicense()
        {
            word.ResetLicense();
            var snapTime = DateTime.Now;

            Assert.AreEqual(word.GetLicense(), LicenseLevels.Level0);
            Assert.AreEqual(word.progress.lastRepeat, snapTime);
        }

        [Test]
        public void UnlockWorkouts()
        {
            word.progress.wordTranslate = true;
            word.progress.translateWord = true;
            word.progress.audioWord = true;
            word.progress.wordPuzzle = true;

            word.UnlockWorkouts();

            Assert.AreEqual(word.progress.wordTranslate, false);
            Assert.AreEqual(word.progress.translateWord, false);
            Assert.AreEqual(word.progress.audioWord, false);
            Assert.AreEqual(word.progress.wordPuzzle, false);
        }

        #endregion
    }
}
