using System;
using LinguaLeo.Scripts.Helpers.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using LinguaLeo.Scripts.Helpers;
using LinguaLeo.Scripts.Managers;
using UnityEngine.Serialization;

namespace LinguaLeo.Scripts.Behaviour
{
    public class WordInfo : MonoBehaviour, IObserver
    {
        #region SerializeFields

        [FormerlySerializedAs("LevelText")]
        [SerializeField]
        private Text levelText = null;

        [FormerlySerializedAs("TimeRepeatText")]
        [SerializeField]
        private Text timeRepeatText = null;

        [FormerlySerializedAs("TimeReduceText")]
        [SerializeField]
        private Text timeReduceText = null;

        #endregion

        #region Private variables

        private Workout.Workout coreWorkout = null;

        #endregion

        #region Events

        void IObserver.OnNotify(object parametr, GameEvents notificationName)
        {
            switch (notificationName)
            {
                case GameEvents.UpdatedLicenseLevel:
                    UpdateInfoWord();
                    break;
            }
        }

        #endregion

        #region Unity events

        // Use this for initialization
        private void Start()
        {
            GameManager.Notifications.AddListener(this, GameEvents.BuildTask);
            GameManager.Notifications.AddListener(this, GameEvents.UpdatedLicenseLevel);

            ComponentsInit();

            coreWorkout = GameManager.WorkoutManager.GetWorkout();
            coreWorkout.DrawTask += UpdateInfoWord;
            UpdateInfoWord();
        }

        #endregion

        #region Private Methods

        private void ComponentsInit()
        {
            levelText = MyUtilities.FindComponentInGo<Text>("LevelText");
            timeRepeatText = MyUtilities.FindComponentInGo<Text>("TimeRepeatText");
            timeReduceText = MyUtilities.FindComponentInGo<Text>("TimeReduceText");
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

            levelText.text = word.progress.license.ToString();

            TimeSpan time = word.GetLicenseValidityTime();
            timeReduceText.text = MyUtilities.FormatTime(time);

            TimeSpan timeUnlock = word.GetLicenseUnlockForRepeat();
            timeRepeatText.text = MyUtilities.FormatTime(timeUnlock);
        }

        #endregion
    }
}
