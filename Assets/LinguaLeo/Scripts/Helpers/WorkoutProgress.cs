using System;
using System.Xml.Serialization;
using LinguaLeo.Scripts.Helpers.Licences;
using UnityEngine;

namespace LinguaLeo.Scripts.Helpers
{
    /// <summary>
    /// Прогресс изучения слова
    /// на разных тренировках
    /// </summary>
    public class WorkoutProgress
    {
        #region Public variables

        [XmlElement("translate")]
        public bool wordTranslate;

        [XmlElement("reverse")]
        public bool translateWord;

        //savannah,
        [XmlElement("audio")]
        public bool audioWord;

        [XmlElement("puzzle")]
        public bool wordPuzzle;

        [XmlElement("savanna")]
        public bool savanna;
        //words_cards;
        //phrase_puzzle;

        public DateTime lastRepeat;
        public LicenseLevels license;

        #endregion

        #region Public Methods

        /// <summary>
        /// Все тренировки пройдены
        /// </summary>
        /// <returns></returns>
        public bool AllWorkoutDone()
        {
            return wordTranslate &&
                   translateWord &&
                   audioWord &&
                   wordPuzzle;
        }

        public bool CanTraining(WorkoutNames workoutName)
        {
            switch (workoutName)
            {
                case WorkoutNames.WordTranslate:
                    return !wordTranslate;
                case WorkoutNames.TranslateWord:
                    return !translateWord;
                case WorkoutNames.Audio:
                    return !audioWord;
                case WorkoutNames.Puzzle:
                    return !wordPuzzle;

                case WorkoutNames.Reiteration:
                    return !wordTranslate;
                case WorkoutNames.Savanna:
                    return !savanna;

                case WorkoutNames.BrainStorm:
                    return !AllWorkoutDone();
                default:
                    Debug.LogError("Тренировка не найдена");
                    return false;
            }
        }

        public void ResetAllWorkouts()
        {
            wordTranslate = false;
            translateWord = false;
            audioWord = false;
            wordPuzzle = false;
        }

        #endregion
    }
}
