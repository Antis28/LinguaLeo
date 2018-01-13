using UnityEngine;

interface Observer
{
    void OnNotify(Component sender, GAME_EVENTS notificationName);
}

