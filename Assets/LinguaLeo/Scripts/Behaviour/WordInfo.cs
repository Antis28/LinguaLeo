using System;
using LinguaLeo.Scripts.Helpers.Interfaces;
using LinguaLeo.Scripts.Manegers;
using UnityEngine;
using UnityEngine.UI;
using LinguaLeo.Scripts.Helpers;

namespace LinguaLeo.Scripts.Behaviour
{
    public class WordInfo : MonoBehaviour, IObserver
    {
        [SerializeField]
        Text LevelText = null;
        [SerializeField]
        Text TimeRepeatText = null;
        [SerializeField]
        Text TimeReduceText = null;

        Workout.Workout coreWorkout = null;

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
            LevelText = MyUtilities.FindComponentInGO<Text>("LevelText");
            TimeRepeatText = MyUtilities.FindComponentInGO<Text>("TimeRepeatText");
            TimeReduceText = MyUtilities.FindComponentInGO<Text>("TimeReduceText");
        }

   

        private void UpdateInfoWord()
        {
            WordLeo word = coreWorkout.GetCurrentWord();

            LevelText.text = word.progress.license.ToString();

            TimeSpan time = word.GetLicenseValidityTime();
            TimeReduceText.text = MyUtilities.FormatTime(time);

            TimeSpan timeUnlock = word.GetLicenseUnlockForRepeat();
            TimeRepeatText.text = MyUtilities.FormatTime(timeUnlock);
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
}
