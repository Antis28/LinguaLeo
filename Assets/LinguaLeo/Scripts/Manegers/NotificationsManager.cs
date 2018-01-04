using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// EVENTS MANAGER CLASS - for receiving notifications and notifying listeners -
/// EVENTS MANAGER CLASS - для получения уведомлений и уведомления слушателей
/// </summary>
public class NotificationsManager : MonoBehaviour
{

    private Dictionary<string, List<Component>> _listeners =
                                new Dictionary<string, List<Component>>();
    public int CountListeners
    {
        get { return _listeners.Count; }
    }

    /// <summary>
    /// Function to add a listener for an notification to the listeners list - 
    /// Добавляет слушателя для уведомления в список слушателей
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="notificationName"></param>
    public void AddListener(Component sender, string notificationName)
    {
        //Add listener to dictionary
        if (!_listeners.ContainsKey(notificationName))
            _listeners.Add(notificationName, new List<Component>());

        //Add object to listener list for this notification
        _listeners[notificationName].Add(sender);

    }

    /// <summary>
    /// Function to post a notification to a listener -
    /// Функция отправки уведомлений слушателю
    /// </summary>
    public void PostNotification(Component sender, string notificationName)
    {
        //If no key in dictionary exists, then exit
        if (!_listeners.ContainsKey(notificationName))
            return;
        //Else post notification to all matching listener‘s -  Уведомлять о новых сообщениях всем соответствующим слушателям
        foreach (Component listener in _listeners[notificationName])
        {
            if (listener)
                listener.SendMessage(notificationName, sender, SendMessageOptions.DontRequireReceiver);
        }
    }

    /// <summary>
    /// Function to remove a listener for a notification -  
    /// Функция удаления слушателя для уведомления
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="notificationName"></param>
    public void RemoveListener(Component sender, string notificationName)
    {
        //If no key in dictionary exists, then exit
        if (!_listeners.ContainsKey(notificationName))
            return;

        List<Component> listListeners = _listeners[notificationName];
        int senderID = sender.GetInstanceID();

        //Cycle through listeners and identify component, and then remove 
        //Проведите цикл прослушивания и идентифицируйте компонент, а затем удалите
        for (int i = listListeners.Count - 1; i >= 0; i--)
        {
            int currentID = listListeners[i].GetInstanceID();
            //Check instance ID - Проверить идентификатор экземпляра
            if (currentID == senderID)
                //Matched. Remove from list -  Совпало. Убрать из списка
                listListeners.RemoveAt(i);
        }
    }

    /// <summary>
    /// Function to remove redundant listeners - deleted and removed listeners - 
    /// Функция удаления избыточных прослушивателей - удаление удаленных слушателей
    /// </summary>
    public void RemoveRedundancies()
    {
        Dictionary<string, List<Component>> tmpListeners =
                               new Dictionary<string, List<Component>>();

        foreach (KeyValuePair<string, List<Component>> item in _listeners)
        {
            // Cycle through all listener objects in list, remove null objects 
            // Циклический просмотр всех объектов-слушателей в списке, удаление нулевых объектов
            for (int i = item.Value.Count - 1; i >= 0; i--)
            {
                // If null, then remove item
                if (item.Value[i] == null)
                    item.Value.RemoveAt(i);
            }
            // If items remain in list for this notification, then add this to tmp dictionary -  
            // Если элементы остались в списке для этого уведомления, добавить их в словарь tmp
            if (item.Value.Count > 0)
                tmpListeners.Add(item.Key, item.Value);
        }
        //Replace listeners object with new, optimized dictionary
        _listeners = tmpListeners;
    }

    /// <summary>
    /// Function to clear all listeners -  Функция очистки всех прослушивателей
    /// </summary>
    public void ClearListeners()
    {
        _listeners.Clear();
    }

    public void Awake()
    {
        SceneManager.sceneLoaded += (s, mode) => RemoveRedundancies();
    }
}


public enum GAME_EVENTS
{
    AmmoCollected,
    AmmoExpired,
    AmmoChange,
    EnemyDestroyed,
    InputChanged,
    AllCashCollected,
    CashCollected,
    LoadGameComplete,
    PlayerHealthChange,
    PlayerDie,
    PowerupCollected,
    SaveGamePrepare,
    WeaponChange,
}
