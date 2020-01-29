
using LinguaLeo.Scripts.Manegers;

namespace LinguaLeo.Scripts.Helpers.Interfaces
{
    public interface IObserver
    {
        void OnNotify(object parametr, GAME_EVENTS notificationName);
        int GetInstanceID();
    }
}

