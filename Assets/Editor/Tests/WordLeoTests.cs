using System;
using NUnit.Framework;

[TestFixture]
public class WordLeoTests
{
    private WordLeo _word;

    [OneTimeSetUp]
    public void Prepare()
    {
        _word = new WordLeo();
        _word.progress = new WorkoutProgress();
        _word.progress.license = LicenseLevels.Level_5;
    }

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
        var level = _word.GetLicense();
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
        _word.LicenseValidityCheck();
        Assert.Fail();
    }

    [Test]
    public void ReduceLicense()
    {
        var beforeLevel = _word.GetLicense();
        _word.ReduceLicense();
        var afterLevel = _word.GetLicense();
        Assert.AreNotEqual(beforeLevel, afterLevel);
    }

    [Test]
    public void ResetLicense()
    {
        _word.ResetLicense();
        var snapTime = DateTime.Now;

        Assert.AreEqual(_word.GetLicense(), LicenseLevels.Level_0);
        Assert.AreEqual(_word.progress.lastRepeat, snapTime);
    }

    [Test]
    public void UnlockWorkouts()
    {
        _word.progress.word_translate = true;
        _word.progress.translate_word = true;
        _word.progress.audio_word = true;
        _word.progress.word_puzzle = true;

        _word.UnlockWorkouts();

        Assert.AreEqual(_word.progress.word_translate, false);
        Assert.AreEqual(_word.progress.translate_word, false);
        Assert.AreEqual(_word.progress.audio_word, false);
        Assert.AreEqual(_word.progress.word_puzzle, false);
    }
}