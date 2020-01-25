
public interface IObserver
{
    void OnNotify(object parametr, GAME_EVENTS notificationName);
    int GetInstanceID();
}

