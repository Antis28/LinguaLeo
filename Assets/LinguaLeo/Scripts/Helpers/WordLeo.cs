using System;
using System.Xml.Serialization;
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
        public string pictureUrl;
        public string transcription;
        public string highlightedContext;
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
            GameManager.Notifications.PostNotification(null, GameEvents.UpdatedLicenseLevel);
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
            if (license == LicenseLevels.Level0 || !progress.AllWorkoutDone()) { return TimeSpan.Zero; }

            double minutes = GetLicenseTime();
            double minutesLeft = 0;

            switch (license)
            {
                case LicenseLevels.Level1:
                    minutesLeft = LicenseTimeTraining.level1 - minutes;
                    break;
                case LicenseLevels.Level2:
                    minutesLeft = LicenseTimeTraining.level2 - minutes;
                    break;
                case LicenseLevels.Level3:
                    minutesLeft = LicenseTimeTraining.level3 - minutes;
                    break;
                case LicenseLevels.Level4:
                    minutesLeft = LicenseTimeTraining.level4 - minutes;
                    break;
                case LicenseLevels.Level5:
                    minutesLeft = LicenseTimeTraining.level5 - minutes;
                    break;
                case LicenseLevels.Level6:
                    minutesLeft = LicenseTimeTraining.level6 - minutes;
                    break;
                case LicenseLevels.Level7:
                    minutesLeft = LicenseTimeTraining.level7 - minutes;
                    break;
                case LicenseLevels.Level8:
                    //лицензия на 1 месяц
                    minutesLeft = LicenseTimeTraining.level8 - minutes;
                    break;
                case LicenseLevels.Level9:
                    //лицензия на 6 месяцев
                    minutesLeft = LicenseTimeTraining.level9 - minutes;
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
                case LicenseLevels.Level1:
                    minutesLeft = LicenseTimeout.level1 - minutes;
                    break;
                case LicenseLevels.Level2:
                    minutesLeft = LicenseTimeout.level2 - minutes;
                    break;
                case LicenseLevels.Level3:
                    minutesLeft = LicenseTimeout.level3 - minutes;
                    break;
                case LicenseLevels.Level4:
                    minutesLeft = LicenseTimeout.level4 - minutes;
                    break;
                case LicenseLevels.Level5:
                    minutesLeft = LicenseTimeout.level5 - minutes;
                    break;
                case LicenseLevels.Level6:
                    minutesLeft = LicenseTimeout.level6 - minutes;
                    break;
                case LicenseLevels.Level7:
                    minutesLeft = LicenseTimeout.level7 - minutes;
                    break;
                case LicenseLevels.Level8:
                    //лицензия на 1 месяц
                    minutesLeft = LicenseTimeout.level8 - minutes;
                    break;
                case LicenseLevels.Level9:
                    //лицензия на 6 месяцев
                    minutesLeft = LicenseTimeout.level9 - minutes;
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

            if (progress.wordTranslate) { progressCount += 0.25f; }

            if (progress.translateWord) { progressCount += 0.25f; }

            if (progress.audioWord) { progressCount += 0.25f; }

            if (progress.wordPuzzle) { progressCount += 0.25f; }

            return progressCount;
        }

        public void LearnAudio()
        {
            progress.audioWord = true;
        }

        public void LearnPuzzle()
        {
            progress.wordPuzzle = true;
        }

        public void LearnTranslateWord()
        {
            progress.translateWord = true;
        }

        public void LearnWordTranslate()
        {
            progress.wordTranslate = true;
        }

        public bool LicenseExists()
        {
            return progress.license >= LicenseLevels.Level1;
        }

        /// <summary>
        /// Проверка можно ли уже повторять слово.
        /// Если можно, то сбрасываются все тренировки.
        /// </summary>
        public void LicenseExpirationCheck()
        {
            LicenseLevels license = progress.license;
            if (license == LicenseLevels.Level0 || !progress.AllWorkoutDone()) { return; }

            double minutes = GetLicenseTime();

            if (minutes > LicenseTimeout.level1)
                LicenseValidityCheck();

            switch (license)
            {
                case LicenseLevels.Level1:
                    //лицензия на 20 минут
                    if (minutes > LicenseTimeTraining.level1)
                        UnlockWorkouts();
                    break;
                case LicenseLevels.Level2:
                    //лицензия на 1 час
                    if (minutes > LicenseTimeTraining.level2)
                        UnlockWorkouts();
                    break;
                case LicenseLevels.Level3:
                    //лицензия на  3 часа
                    if (minutes > LicenseTimeTraining.level3)
                        UnlockWorkouts();
                    break;
                case LicenseLevels.Level4:
                    //лицензия на 1 сутки
                    if (minutes > LicenseTimeTraining.level4)
                        UnlockWorkouts();
                    break;
                case LicenseLevels.Level5:
                    //лицензия на 2 суток
                    if (minutes > LicenseTimeTraining.level5)
                        UnlockWorkouts();
                    break;
                case LicenseLevels.Level6:
                    //лицензия на 3 суток
                    if (minutes > LicenseTimeTraining.level6)
                        UnlockWorkouts();
                    break;
                case LicenseLevels.Level7:
                    //лицензия на 1 неделю
                    if (minutes > LicenseTimeTraining.level7)
                        UnlockWorkouts();
                    break;
                case LicenseLevels.Level8:
                    //лицензия на 1 месяц
                    if (minutes > LicenseTimeTraining.level8)
                        UnlockWorkouts();
                    break;
                case LicenseLevels.Level9:
                    //лицензия на 6 месяцев
                    if (minutes > LicenseTimeTraining.level9)
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
                case LicenseLevels.Level2:
                    //лицензия на 1 час
                    if (minutes > LicenseTimeout.level2)
                        ReduceLicense();
                    break;
                case LicenseLevels.Level3:
                    //лицензия на  3 часа
                    if (minutes > LicenseTimeout.level3)
                        ReduceLicense();
                    break;
                case LicenseLevels.Level4:
                    //лицензия на 1 сутки
                    if (minutes > LicenseTimeout.level4)
                        ReduceLicense();
                    break;
                case LicenseLevels.Level5:
                    //лицензия на 2 суток
                    if (minutes > LicenseTimeout.level5)
                        ReduceLicense();
                    break;
                case LicenseLevels.Level6:
                    //лицензия на 3 суток
                    if (minutes > LicenseTimeout.level6)
                        ReduceLicense();
                    break;
                case LicenseLevels.Level7:
                    //лицензия на 1 неделю
                    if (minutes > LicenseTimeout.level7)
                        ReduceLicense();
                    break;
                case LicenseLevels.Level8:
                    //лицензия на 1 месяц
                    if (minutes > LicenseTimeout.level8)
                        ReduceLicense();
                    break;
                case LicenseLevels.Level9:
                    //лицензия на 6 месяцев
                    if (minutes > LicenseTimeout.level9)
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
            if (progress.license == LicenseLevels.Level0)
                return;

            progress.ResetAllWorkouts();
            progress.license--;
            progress.lastRepeat = DateTime.Now;
        }

        public void ResetLicense()
        {
            if (progress.license == LicenseLevels.Level0)
            {
                progress.ResetAllWorkouts();
                return;
            }

            progress.ResetAllWorkouts();
            progress.license = LicenseLevels.Level0;
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
