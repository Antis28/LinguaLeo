using System;
using System.Xml.Serialization;
using JetBrains.Annotations;
using LinguaLeo.Scripts.Helpers.Licences;
using LinguaLeo.Scripts.Managers;

namespace LinguaLeo.Scripts.Helpers
{
    /// <summary>
    /// Mapping Class
    /// см. класс ExportWordLeo
    /// в проекте Обучение/CSVReader
    /// </summary>
    public class WordLeo : IEquatable<WordLeo>
    {
        #region Public variables

        public string wordValue;
        public string translations;
        [NotNull]
        public string pictureUrl;
        public string transcription;
        public string highlightedContext;
        [NotNull]
        public string soundUrl;
        public string clozefiedContext;
        public string[] groups;
        public WorkoutProgress progress;

        #endregion

        #region Public Methods

        public void AddLicenseLevel()
        {
            progress.lastRepeat = DateTime.Now;
            progress.license++;
            GameManager.Notifications.PostNotification(null, GAME_EVENTS.UpdatedLicenseLevel);
        }

        public bool AllWorkoutDone()
        {
            return progress.AllWorkoutDone();
        }

        public bool CanbeRepeated()
        {
            bool canbeRepeated = !progress.AllWorkoutDone();
            //canbeRepeated &= progress.license >= LicenseLevels.Level_1;
            return canbeRepeated;
        }

        public bool CanTraining(WorkoutNames workoutName)
        {
            return progress.CanTraining(workoutName);
        }

        public bool Equals(WordLeo other)
        {
            if (other == null)
                return false;

            return wordValue == other.wordValue;
        }

        public override bool Equals(Object obj)
        {
            if (obj == null)
                return false;

            WordLeo wordLeo = obj as WordLeo;
            if (wordLeo == null)
                return false;
            return Equals(wordLeo);
        }

        // ReSharper disable once NonReadonlyMemberInGetHashCode
        public override int GetHashCode()
        {
            return wordValue.GetHashCode();
        }

        public LicenseLevels GetLicense()
        {
            return progress.license;
        }

        /// <summary>
        /// Расчитывает количество минут до 
        /// разблокировки лицензии для повторения.
        /// </summary>
        /// <returns>Время</returns>
        public TimeSpan GetLicenseUnlockForRepeat()
        {
            LicenseLevels license = progress.license;
            if (license == LicenseLevels.Level_0 || !progress.AllWorkoutDone()) { return TimeSpan.Zero; }

            double minutes = GetLicenseTime();
            double minutesLeft = 0;

            switch (license)
            {
                case LicenseLevels.Level_1:
                    minutesLeft = LicenseTimeTraining.Level_1 - minutes;
                    break;
                case LicenseLevels.Level_2:
                    minutesLeft = LicenseTimeTraining.Level_2 - minutes;
                    break;
                case LicenseLevels.Level_3:
                    minutesLeft = LicenseTimeTraining.Level_3 - minutes;
                    break;
                case LicenseLevels.Level_4:
                    minutesLeft = LicenseTimeTraining.Level_4 - minutes;
                    break;
                case LicenseLevels.Level_5:
                    minutesLeft = LicenseTimeTraining.Level_5 - minutes;
                    break;
                case LicenseLevels.Level_6:
                    minutesLeft = LicenseTimeTraining.Level_6 - minutes;
                    break;
                case LicenseLevels.Level_7:
                    minutesLeft = LicenseTimeTraining.Level_7 - minutes;
                    break;
                case LicenseLevels.Level_8:
                    //лицензия на 1 месяц
                    minutesLeft = LicenseTimeTraining.Level_8 - minutes;
                    break;
                case LicenseLevels.Level_9:
                    //лицензия на 6 месяцев
                    minutesLeft = LicenseTimeTraining.Level_9 - minutes;
                    break;
            }

            if (minutesLeft < 0) { minutesLeft = 0; }

            return TimeSpan.FromMinutes(minutesLeft);
        }

        /// <summary>
        /// Минут до окончания лицензии
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetLicenseValidityTime()
        {
            LicenseLevels license = progress.license;
            double minutes = GetLicenseTime();
            double minutesLeft = 0;

            switch (license)
            {
                case LicenseLevels.Level_1:
                    minutesLeft = LicenseTimeout.Level_1 - minutes;
                    break;
                case LicenseLevels.Level_2:
                    minutesLeft = LicenseTimeout.Level_2 - minutes;
                    break;
                case LicenseLevels.Level_3:
                    minutesLeft = LicenseTimeout.Level_3 - minutes;
                    break;
                case LicenseLevels.Level_4:
                    minutesLeft = LicenseTimeout.Level_4 - minutes;
                    break;
                case LicenseLevels.Level_5:
                    minutesLeft = LicenseTimeout.Level_5 - minutes;
                    break;
                case LicenseLevels.Level_6:
                    minutesLeft = LicenseTimeout.Level_6 - minutes;
                    break;
                case LicenseLevels.Level_7:
                    minutesLeft = LicenseTimeout.Level_7 - minutes;
                    break;
                case LicenseLevels.Level_8:
                    //лицензия на 1 месяц
                    minutesLeft = LicenseTimeout.Level_8 - minutes;
                    break;
                case LicenseLevels.Level_9:
                    //лицензия на 6 месяцев
                    minutesLeft = LicenseTimeout.Level_9 - minutes;
                    break;
            }

            if (minutesLeft < 0) { minutesLeft = 0; }

            return TimeSpan.FromMinutes(minutesLeft);
        }

        /// <summary>
        /// показывает прогресс изучения слова
        /// </summary>
        public float GetProgressCount()
        {
            float progressCount = 0;

            if (progress.word_translate) { progressCount += 0.25f; }

            if (progress.translate_word) { progressCount += 0.25f; }

            if (progress.audio_word) { progressCount += 0.25f; }

            if (progress.word_puzzle) { progressCount += 0.25f; }

            return progressCount;
        }

        public void LearnAudio()
        {
            progress.audio_word = true;
        }

        public void LearnPuzzle()
        {
            progress.word_puzzle = true;
        }

        public void LearnTranslateWord()
        {
            progress.translate_word = true;
        }

        public void LearnWordTranslate()
        {
            progress.word_translate = true;
        }

        public bool LicenseExists()
        {
            return progress.license >= LicenseLevels.Level_1;
        }

        /// <summary>
        /// Проверка можно ли уже повторять слово.
        /// Если можно, то сбрасываются все тренировки.
        /// </summary>
        public void LicenseExpirationCheck()
        {
            LicenseLevels license = progress.license;
            if (license == LicenseLevels.Level_0 || !progress.AllWorkoutDone()) { return; }

            double minutes = GetLicenseTime();

            if (minutes > LicenseTimeout.Level_1)
                LicenseValidityCheck();

            switch (license)
            {
                case LicenseLevels.Level_1:
                    //лицензия на 20 минут
                    if (minutes > LicenseTimeTraining.Level_1)
                        UnlockWorkouts();
                    break;
                case LicenseLevels.Level_2:
                    //лицензия на 1 час
                    if (minutes > LicenseTimeTraining.Level_2)
                        UnlockWorkouts();
                    break;
                case LicenseLevels.Level_3:
                    //лицензия на  3 часа
                    if (minutes > LicenseTimeTraining.Level_3)
                        UnlockWorkouts();
                    break;
                case LicenseLevels.Level_4:
                    //лицензия на 1 сутки
                    if (minutes > LicenseTimeTraining.Level_4)
                        UnlockWorkouts();
                    break;
                case LicenseLevels.Level_5:
                    //лицензия на 2 суток
                    if (minutes > LicenseTimeTraining.Level_5)
                        UnlockWorkouts();
                    break;
                case LicenseLevels.Level_6:
                    //лицензия на 3 суток
                    if (minutes > LicenseTimeTraining.Level_6)
                        UnlockWorkouts();
                    break;
                case LicenseLevels.Level_7:
                    //лицензия на 1 неделю
                    if (minutes > LicenseTimeTraining.Level_7)
                        UnlockWorkouts();
                    break;
                case LicenseLevels.Level_8:
                    //лицензия на 1 месяц
                    if (minutes > LicenseTimeTraining.Level_8)
                        UnlockWorkouts();
                    break;
                case LicenseLevels.Level_9:
                    //лицензия на 6 месяцев
                    if (minutes > LicenseTimeTraining.Level_9)
                        UnlockWorkouts();
                    break;
            }
        }

        /// <summary>
        /// Проверка не истекла ли еще лицензия.
        /// Если истекла, то понижается уровень лицензии.
        /// </summary>
        public void LicenseValidityCheck()
        {
            LicenseLevels license = progress.license;
            TimeSpan interval = DateTime.Now - progress.lastRepeat;
            double minutes = interval.TotalMinutes;
            switch (license)
            {
                case LicenseLevels.Level_2:
                    //лицензия на 1 час
                    if (minutes > LicenseTimeout.Level_2)
                        ReduceLicense();
                    break;
                case LicenseLevels.Level_3:
                    //лицензия на  3 часа
                    if (minutes > LicenseTimeout.Level_3)
                        ReduceLicense();
                    break;
                case LicenseLevels.Level_4:
                    //лицензия на 1 сутки
                    if (minutes > LicenseTimeout.Level_4)
                        ReduceLicense();
                    break;
                case LicenseLevels.Level_5:
                    //лицензия на 2 суток
                    if (minutes > LicenseTimeout.Level_5)
                        ReduceLicense();
                    break;
                case LicenseLevels.Level_6:
                    //лицензия на 3 суток
                    if (minutes > LicenseTimeout.Level_6)
                        ReduceLicense();
                    break;
                case LicenseLevels.Level_7:
                    //лицензия на 1 неделю
                    if (minutes > LicenseTimeout.Level_7)
                        ReduceLicense();
                    break;
                case LicenseLevels.Level_8:
                    //лицензия на 1 месяц
                    if (minutes > LicenseTimeout.Level_8)
                        ReduceLicense();
                    break;
                case LicenseLevels.Level_9:
                    //лицензия на 6 месяцев
                    if (minutes > LicenseTimeout.Level_9)
                        ReduceLicense();
                    break;
            }
        }

        public static bool operator ==(WordLeo wordLeo1, WordLeo wordLeo2)
        {
            if (((object) wordLeo1) == null || ((object) wordLeo2) == null)
                return Object.Equals(wordLeo1, wordLeo2);

            return wordLeo1.Equals(wordLeo2);
        }

        public static bool operator !=(WordLeo wordLeo1, WordLeo wordLeo2)
        {
            if (((object) wordLeo1) == null || ((object) wordLeo2) == null)
                return !Object.Equals(wordLeo1, wordLeo2);

            return !(wordLeo1.Equals(wordLeo2));
        }

        /// <summary>
        /// Понизить лицензию
        /// </summary>
        public void ReduceLicense()
        {
            if (progress.license == LicenseLevels.Level_0)
                return;

            progress.ResetAllWorkouts();
            progress.license--;
            progress.lastRepeat = DateTime.Now;
        }

        public void ResetLicense()
        {
            if (progress.license == LicenseLevels.Level_0)
            {
                progress.ResetAllWorkouts();
                return;
            }

            progress.ResetAllWorkouts();
            progress.license = LicenseLevels.Level_0;
            progress.lastRepeat = DateTime.Now;
        }

        override
            public string ToString()
        {
            return wordValue + " - " + GetProgressCount() + "% " + GetProgressCount() + "мин. " + progress.license;
        }

        public void UnlockWorkouts()
        {
            progress.ResetAllWorkouts();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// минут с последнего повторения слова 
        /// </summary>
        /// <returns></returns>
        private double GetLicenseTime()
        {
            TimeSpan interval = DateTime.Now - progress.lastRepeat;
            return interval.TotalMinutes;
        }

        #endregion
    }
}
