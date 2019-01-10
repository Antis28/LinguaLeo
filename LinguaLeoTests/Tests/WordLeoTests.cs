using System;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class WordLeoTests
    {
        [Test]
        public void GetLicenseValidityTime()
        {
            LicenseLevels level = LicenseLevels.Level_3;
            int levelTime = LicenseTimeout.Level_3;
            int timeExpiries = levelTime - 20;
            double expectedValue = levelTime - timeExpiries;

            WordLeo word = CreateWordLeo(level);
            word.progress.lastRepeat = DateTime.Now - TimeSpan.FromMinutes(timeExpiries);

            double resultValue = RoundTofloat(word.GetLicenseValidityTime().TotalMinutes);

            Assert.AreEqual(expectedValue, resultValue);
        }

        private static float RoundTofloat(double val)
        {
            return UnityEngine.Mathf.Round( (float)val );
        }

        //ToDO: добавить fake для DateTime
        [Test]
        public void GetLicenseExpiration_LevelBiggestZero_ReturnExpectedValue()
        {
            LicenseLevels level = LicenseLevels.Level_3;
            int levelTime = LicenseTimeTraining.Level_3;
            int timeExpiries = levelTime - 20;

            WordLeo word = CreateWordLeo(level, true);
            word.progress.lastRepeat = DateTime.Now - new TimeSpan(0, timeExpiries, 0);

            TimeSpan interval = DateTime.Now - word.progress.lastRepeat;
            double expectedValue = levelTime - interval.TotalMinutes;
            expectedValue = RoundTofloat(expectedValue);

            double resultValue = word.GetLicenseUnlockForRepeat().TotalMinutes;
            resultValue = RoundTofloat(resultValue);

            Assert.AreEqual(expectedValue, resultValue);
        }

        [Test]
        public void GetLicenseExpiration_Level0_ReturnZero()
        {
            double expectedValue = 0;

            WordLeo word = CreateWordLeo();
            double resultValue = word.GetLicenseUnlockForRepeat().TotalMinutes;

            Assert.AreEqual(expectedValue, resultValue);
        }

        [Test]
        [TestCase(WorkoutNames.Audio)]
        [TestCase(WorkoutNames.Puzzle)]
        [TestCase(WorkoutNames.Savanna)]
        [TestCase(WorkoutNames.TranslateWord)]
        [TestCase(WorkoutNames.WordTranslate)]
        [TestCase(WorkoutNames.brainStorm)]
        [TestCase(WorkoutNames.reiteration)]
        public void CanTraining_AllWorkoutFalse_ReturnTrue(WorkoutNames WorkoutName)
        {
            bool expectedValue = true;

            WordLeo word = CreateWordLeo(LicenseLevels.Level_3);
            bool resultValue = word.CanTraining(WorkoutName);

            Assert.AreEqual(expectedValue, resultValue);
        }

        [Test]
        public void CanbeRepeated_AllWorkoutFalse_ReturnTrue()
        {
            bool expectedValue = true;

            WordLeo word = CreateWordLeo(LicenseLevels.Level_3);
            bool resultValue = word.CanbeRepeated();

            Assert.AreEqual(expectedValue, resultValue);
        }

        [Test]
        [TestCase(LicenseLevels.Level_2)]
        [TestCase(LicenseLevels.Level_3)]
        [TestCase(LicenseLevels.Level_4)]
        [TestCase(LicenseLevels.Level_5)]
        [TestCase(LicenseLevels.Level_6)]
        [TestCase(LicenseLevels.Level_7)]
        [TestCase(LicenseLevels.Level_8)]
        [TestCase(LicenseLevels.Level_9)]
        public void LicenseExpirationCheck_LicenseNotExpiries_ExpectedProgressHasNotChanged(LicenseLevels expectedLevel)
        {
            bool currentProgress = true;
            bool expectedProgress = true;

            WordLeo word = CreateWordLeo(expectedLevel, currentProgress);
            word.progress.lastRepeat = DateTime.Now - new TimeSpan(0, 10, 0);
            word.LicenseExpirationCheck();

            Assert.AreEqual(word.progress.word_translate, expectedProgress);
            Assert.AreEqual(word.progress.translate_word, expectedProgress);
            Assert.AreEqual(word.progress.audio_word, expectedProgress);
            Assert.AreEqual(word.progress.word_puzzle, expectedProgress);
        }

        [Test]
        [TestCase(LicenseLevels.Level_2)]
        [TestCase(LicenseLevels.Level_3)]
        [TestCase(LicenseLevels.Level_4)]
        [TestCase(LicenseLevels.Level_5)]
        [TestCase(LicenseLevels.Level_6)]
        [TestCase(LicenseLevels.Level_7)]
        [TestCase(LicenseLevels.Level_8)]
        [TestCase(LicenseLevels.Level_9)]
        public void LicenseExpirationCheck_LicenseExpiries_ExpectedProgressHasReset(LicenseLevels expectedLevel)
        {
            bool currentProgress = true;
            bool expectedProgress = false;

            WordLeo word = CreateWordLeo(expectedLevel, currentProgress);
            word.progress.lastRepeat = DateTime.Now - new TimeSpan(0, LicenseTimeTraining.Level_9 + 1, 0);
            word.LicenseExpirationCheck();

            Assert.AreEqual(word.progress.word_translate, expectedProgress);
            Assert.AreEqual(word.progress.translate_word, expectedProgress);
            Assert.AreEqual(word.progress.audio_word, expectedProgress);
            Assert.AreEqual(word.progress.word_puzzle, expectedProgress);
        }

        [Test]
        public void GetLicense_StateLevel5_ReturnLevel5()
        {
            WordLeo word = CreateWordLeo(LicenseLevels.Level_5);
            var level = word.GetLicense();
            Assert.AreEqual(level, LicenseLevels.Level_5);
        }

        [Test] /// лицензия не истекла
        [TestCase(LicenseLevels.Level_2)]
        [TestCase(LicenseLevels.Level_3)]
        [TestCase(LicenseLevels.Level_4)]
        [TestCase(LicenseLevels.Level_5)]
        [TestCase(LicenseLevels.Level_6)]
        [TestCase(LicenseLevels.Level_7)]
        [TestCase(LicenseLevels.Level_8)]
        [TestCase(LicenseLevels.Level_9)]
        public void LicenseValidityCheck_LicenseNotExpiries_ExpectedLevelHasNotChanged(LicenseLevels expectedLevel)
        {
            WordLeo word = CreateWordLeo(expectedLevel);
            word.progress.lastRepeat = DateTime.Now - new TimeSpan(0, 1, 0);
            word.LicenseValidityCheck();
            Assert.AreEqual(expectedLevel, word.progress.license);
        }

        [Test] /// лицензия истекла
        [TestCase(LicenseLevels.Level_2)]
        [TestCase(LicenseLevels.Level_3)]
        [TestCase(LicenseLevels.Level_4)]
        [TestCase(LicenseLevels.Level_5)]
        [TestCase(LicenseLevels.Level_6)]
        [TestCase(LicenseLevels.Level_7)]
        [TestCase(LicenseLevels.Level_8)]
        [TestCase(LicenseLevels.Level_9)]
        public void LicenseValidityCheck_LicenseExpiries_ExpectedReducedLevelByOne(LicenseLevels actualLevel)
        {
            WordLeo word = CreateWordLeo(actualLevel);
            word.progress.lastRepeat = DateTime.Now - new TimeSpan(0, LicenseTimeout.Level_9 + 5, 0);
            word.LicenseValidityCheck();
            Assert.AreEqual(actualLevel - 1, word.progress.license);
        }

        [Test]
        public void ReduceLicense_LicenseLevelEqualsFive_StateLicenseLevelEqualsFour()
        {
            WordLeo word = CreateWordLeo(LicenseLevels.Level_5);
            LicenseLevels expextedLevel = LicenseLevels.Level_4;
            var expextedTime = DateTime.Now;

            word.ReduceLicense();
            var resultLevel = word.GetLicense();

            Assert.AreEqual(expextedLevel, resultLevel);
            Assert.AreEqual(expextedTime.Second, word.progress.lastRepeat.Second);
        }
        [Test]
        public void ReduceLicense_LicenseLevelEqualsZero_StateNotChange()
        {
            WordLeo word = CreateWordLeo(LicenseLevels.Level_0);
            LicenseLevels expextedLevel = LicenseLevels.Level_0;

            word.ReduceLicense();
            var resultLevel = word.GetLicense();

            Assert.AreEqual(resultLevel, expextedLevel);
        }

        [Test]
        public void ResetLicense_LicenseLevelBeggesZero_StateLicenseLevelEqualsZeroAndTimeEqualsNow()
        {
            WordLeo word = CreateWordLeo(LicenseLevels.Level_5);
            word.ResetLicense();
            var snapTime = DateTime.Now;

            Assert.AreEqual(word.GetLicense(), LicenseLevels.Level_0);
            Assert.AreEqual(word.progress.lastRepeat.Second, snapTime.Second);
        }

        [Test]
        public void ResetLicense_LicenseLevelEqualsZero_StateProgressEqualsFalse()
        {
            WordLeo word = CreateWordLeo(LicenseLevels.Level_0);
            word.ResetLicense();

            Assert.AreEqual(word.GetLicense(), LicenseLevels.Level_0);
        }

        [Test]
        public void UnlockWorkouts_WorkoutsProgressIsTrue_StateWorkoutsProgressIsFalse()
        {
            WordLeo word = CreateWordLeo(LicenseLevels.Level_5, true);

            word.UnlockWorkouts();

            Assert.AreEqual(word.progress.word_translate, false);
            Assert.AreEqual(word.progress.translate_word, false);
            Assert.AreEqual(word.progress.audio_word, false);
            Assert.AreEqual(word.progress.word_puzzle, false);
        }

        [Test]
        [TestCase(false)]
        [TestCase(true)]
        public void AllWorkoutDone(bool workoutIsDone)
        {
            WordLeo word = CreateWordLeo(LicenseLevels.Level_5, workoutIsDone);
            bool result = word.AllWorkoutDone();
            Assert.AreEqual(result, workoutIsDone);
        }


        //[Test]
        //public void GetProgressCount()
        //{
        //    Assert.Fail();
        //}

        //[Test]
        //public void LearnAudio()
        //{
        //    Assert.Fail();
        //}

        //[Test]
        //public void LearnPuzzle()
        //{
        //    Assert.Fail();
        //}

        //[Test]
        //public void LearnTranslateWord()
        //{
        //    Assert.Fail();
        //}

        //[Test]
        //public void LearnWordTranslate()
        //{
        //    Assert.Fail();
        //}

        //[Test]
        //public void LicenseExists()
        //{
        //    Assert.Fail();
        //}

        // Отложено до рефакторига
        //[Test]
        //public void AddLicenseLevel()
        //{
        //    WordLeo word = CreateWordLeo(LicenseLevels.Level_5);
        //    word.AddLicenseLevel();
        //    var snapTime = DateTime.Now;

        //    Assert.AreEqual(word.progress.license, LicenseLevels.Level_6);
        //    Assert.AreEqual(word.progress.lastRepeat.Second, snapTime.Second);
        //}


        #region Utils
        private static WordLeo CreateWordLeo(LicenseLevels level = LicenseLevels.Level_0,
                                            bool workoutsProgress = false)
        {
            WordLeo word;
            word = new WordLeo();
            word.progress = new WorkoutProgress();

            word.progress.license = level;

            word.progress.word_translate = workoutsProgress;
            word.progress.translate_word = workoutsProgress;
            word.progress.audio_word = workoutsProgress;
            word.progress.word_puzzle = workoutsProgress;

            return word;
        }
        #endregion
    }
}