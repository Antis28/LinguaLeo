using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LicenseTimeTraining {

    public static int
     Level_1 = CalculateTimeUnlock(LicenseTimeout.Level_1),// на 20 минут
     Level_2 = CalculateTimeUnlock(LicenseTimeout.Level_2),// на 1 час
     Level_3 = CalculateTimeUnlock(LicenseTimeout.Level_3),// на 3 часа
     Level_4 = CalculateTimeUnlock(LicenseTimeout.Level_4),// на 1 сутки
     Level_5 = CalculateTimeUnlock(LicenseTimeout.Level_5),// на 2 суток
     Level_6 = CalculateTimeUnlock(LicenseTimeout.Level_6),// на 3 суток
     Level_7 = CalculateTimeUnlock(LicenseTimeout.Level_7),// на 1 неделю
     Level_8 = CalculateTimeUnlock(LicenseTimeout.Level_8),// на 1 месяц
     Level_9 = CalculateTimeUnlock(LicenseTimeout.Level_9);// на 6 месяцев

    private static int CalculateTimeUnlock(int fullTime)
    {
        int unlockTime = (fullTime / 3) * 2;
        return unlockTime;
    }
}
