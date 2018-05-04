using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LicenseTimeout
{
    public static int
     Level_0 = 0,// нет лицензии
     Level_1 = 1440,// 20,// на 20 минут
     Level_2 = 1440,// 60,// на 1 час
     Level_3 = 1440,//180,// на 3 часа
     Level_4 = 1440,// на 1 сутки   // открывается через 16 часов
     Level_5 = 2880,// на 2 суток   // открывается через 32 часа
     Level_6 = 4320,// на 3 суток   // открывается через 1 сутки
     Level_7 = 10080,// на 1 неделю // открывается через 4,6 суток
     Level_8 = 40320,// на 1 месяц  // открывается через 20 суток
     Level_9 = 241920;// на 6 месяцев
}