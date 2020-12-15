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
        public bool word_translate;

        [XmlElement("reverse")]
        public bool translate_word;

        //savannah,
        [XmlElement("audio")]
        public bool audio_word;

        [XmlElement("puzzle")]
        public bool word_puzzle;

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
            return word_translate &&
                   translate_word &&
                   audio_word &&
                   word_puzzle;
        }

        public bool CanTraining(WorkoutNames workoutName)
        {
            switch (workoutName)
            {
                case WorkoutNames.WordTranslate:
                    return !word_translate;
                case WorkoutNames.TranslateWord:
                    return !translate_word;
                case WorkoutNames.Audio:
                    return !audio_word;
                case WorkoutNames.Puzzle:
                    return !word_puzzle;

                case WorkoutNames.reiteration:
                    return !word_translate;
                case WorkoutNames.Savanna:
                    return !savanna;

                case WorkoutNames.brainStorm:
                    return !AllWorkoutDone();
                default:
                    Debug.LogError("Тренировка не найдена");
                    return false;
            }
        }

        public void ResetAllWorkouts()
        {
            word_translate = false;
            translate_word = false;
            audio_word = false;
            word_puzzle = false;
        }

        #endregion
    }
}
