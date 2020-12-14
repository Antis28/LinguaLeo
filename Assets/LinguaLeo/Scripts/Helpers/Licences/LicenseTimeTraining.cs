namespace LinguaLeo.Scripts.Helpers.Licences
{
    /// <summary>
    /// Время через которое слово будет открыто для новой тренровки
    /// в минутах
    /// </summary>
    public class LicenseTimeTraining
    {
        #region Static Fields and Constants

        public static int
            level1 = CalculateTimeUnlock(60),                     // на 20 минут
            level2 = CalculateTimeUnlock(180),                    // на 1 час
            level3 = CalculateTimeUnlock(540),                    // на 3 часа
            level4 = CalculateTimeUnlock(LicenseTimeout.level4), // на 1 сутки
            level5 = CalculateTimeUnlock(LicenseTimeout.level5), // на 2 суток
            level6 = CalculateTimeUnlock(LicenseTimeout.level6), // на 3 суток
            level7 = CalculateTimeUnlock(LicenseTimeout.level7), // на 1 неделю
            level8 = CalculateTimeUnlock(LicenseTimeout.level8), // на 1 месяц
            level9 = CalculateTimeUnlock(LicenseTimeout.level9); // на 6 месяцев

        #endregion

        #region Private Methods

        private static int CalculateTimeUnlock(int fullTime)
        {
            int unlockTime = fullTime * 2 / 3;
            return unlockTime;
        }

        #endregion
    }
}
