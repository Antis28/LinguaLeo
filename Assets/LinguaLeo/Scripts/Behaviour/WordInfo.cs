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
        #region SerializeFields

        [SerializeField]
        private Text LevelText = null;

        [SerializeField]
        private Text TimeRepeatText = null;

        [SerializeField]
        private Text TimeReduceText = null;

        #endregion

        #region Private variables

        private Workout.Workout coreWorkout = null;

        #endregion

        #region Events

        void IObserver.OnNotify(object parametr, GAME_EVENTS notificationName)
        {
            switch (notificationName)
            {
                case GAME_EVENTS.UpdatedLicenseLevel:
                    UpdateInfoWord();
                    break;
            }
        }

        #endregion

        #region Unity events

        // Use this for initialization
        private void Start()
        {
            GameManager.Notifications.AddListener(this, GAME_EVENTS.BuildTask);
            GameManager.Notifications.AddListener(this, GAME_EVENTS.UpdatedLicenseLevel);

            ComponentsInit();

            coreWorkout = GameManager.WorkoutManager.GetWorkout();
            coreWorkout.DrawTask += UpdateInfoWord;
            UpdateInfoWord();
        }

        #endregion

        #region Private Methods

        private void ComponentsInit()
        {
            LevelText = MyUtilities.FindComponentInGO<Text>("LevelText");
            TimeRepeatText = MyUtilities.FindComponentInGO<Text>("TimeRepeatText");
            TimeReduceText = MyUtilities.FindComponentInGO<Text>("TimeReduceText");
        }


        private string TimeSpanToString(TimeSpan time)
        {
            if (time.TotalDays > 1)
                return (int) time.TotalDays + " дн. " + time.Hours + " ч.";
            if (time.TotalHours > 1)
                return (int) time.TotalHours + " ч. " + time.Minutes + " мин.";

            return (int) time.TotalMinutes + " мин.";
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

        #endregion
    }
}
