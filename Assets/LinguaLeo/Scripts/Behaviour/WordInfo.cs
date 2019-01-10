using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordInfo : MonoBehaviour, IObserver
{
    [SerializeField]
    Text LevelText = null;
    [SerializeField]
    Text TimeRepeatText = null;
    [SerializeField]
    Text TimeReduceText = null;

    Workout coreWorkout = null;

    // Use this for initialization
    void Start()
    {
        GameManager.Notifications.AddListener(this, GAME_EVENTS.BuildTask);
        GameManager.Notifications.AddListener(this, GAME_EVENTS.UpdatedLicenseLevel);

        ComponentsInit();

        coreWorkout = GameManager.WorkoutManager.GetWorkout();
        coreWorkout.DrawTask += UpdateInfoWord;
        UpdateInfoWord();
    }

    private void ComponentsInit()
    {
        LevelText = Utilities.FindComponentInGO<Text>("LevelText");
        TimeRepeatText = Utilities.FindComponentInGO<Text>("TimeRepeatText");
        TimeReduceText = Utilities.FindComponentInGO<Text>("TimeReduceText");
    }

   

    private void UpdateInfoWord()
    {
        WordLeo word = coreWorkout.GetCurrentWord();

        LevelText.text = word.progress.license.ToString();

        TimeSpan time = word.GetLicenseValidityTime();
        TimeReduceText.text = Utilities.FormatTime(time);

        TimeSpan timeUnlock = word.GetLicenseUnlockForRepeat();
        TimeRepeatText.text = Utilities.FormatTime(timeUnlock);
    }

    

    private string TimeSpanToString(TimeSpan time)
    {
        if (time.TotalDays > 1)
            return (int)time.TotalDays + " дн. " + (int)time.Hours + " ч.";
        if (time.TotalHours > 1)
            return (int)time.TotalHours + " ч. " + (int)time.Minutes + " мин.";

        return (int)time.TotalMinutes + " мин.";
    }

    void IObserver.OnNotify(object parametr, GAME_EVENTS notificationName)
    {
        switch (notificationName)
        {
            case GAME_EVENTS.UpdatedLicenseLevel:
                UpdateInfoWord();
                break;
        }
    }
}
