using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LicenseTimeTraining {

    public static int
     Level_1 = CalculateTimeUnlock(20),// на 20 минут
     Level_2 = CalculateTimeUnlock(60),// на 1 час
     Level_3 = CalculateTimeUnlock(180),// на 3 часа
     Level_4 = CalculateTimeUnlock(LicenseTimeout.Level_4),// на 1 сутки
     Level_5 = CalculateTimeUnlock(LicenseTimeout.Level_5),// на 2 суток
     Level_6 = CalculateTimeUnlock(LicenseTimeout.Level_6),// на 3 суток
     Level_7 = CalculateTimeUnlock(LicenseTimeout.Level_7),// на 1 неделю
     Level_8 = CalculateTimeUnlock(LicenseTimeout.Level_8),// на 1 месяц
     Level_9 = CalculateTimeUnlock(LicenseTimeout.Level_9);// на 6 месяцев

    private static int CalculateTimeUnlock(int fullTime)
    {
        int unlockTime = fullTime * 2 / 3 ;
        return unlockTime;
    }
}
