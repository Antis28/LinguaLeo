using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

interface Observer
{
    void OnNotify(Component sender, GAME_EVENTS notificationName);
}

