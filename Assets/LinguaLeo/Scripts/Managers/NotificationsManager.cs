using System.Collections.Generic;
using LinguaLeo._Adapters;
using LinguaLeo.Scripts.Helpers.Interfaces;
using UnityEngine;

namespace LinguaLeo.Scripts.Managers
{
    /// <summary>
    /// EVENTS MANAGER CLASS - for receiving notifications and notifying listeners -
    /// EVENTS MANAGER CLASS - для получения уведомлений и уведомления слушателей
    /// </summary>
    public class NotificationsManager : MonoBehaviour
    {
        #region Public variables

        public int CountListeners
        {
            get { return listeners.Count; }
        }

        #endregion

        #region Private variables

        private Dictionary<GameEvents, List<MonoBehaviour>> listeners =
            new Dictionary<GameEvents, List<MonoBehaviour>>();

        #endregion

        #region Public Methods

        /// <summary>
        /// Function to add a listener for an notification to the listeners list - 
        /// Добавляет слушателя для уведомления в список слушателей
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="notificationName"></param>
        public void AddListener(MonoBehaviour sender, GameEvents notificationName)
        {
            //Add listener to dictionary
            if (!listeners.ContainsKey(notificationName))
                listeners.Add(notificationName, new List<MonoBehaviour>());

            //Add object to listener list for this notification
            listeners[notificationName].Add(sender);
        }

        /// <summary>
        /// Function to clear all listeners -  Функция очистки всех прослушивателей
        /// </summary>
        public void ClearListeners()
        {
            listeners.Clear();
        }

        /// <summary>
        /// Function to post a notification to a listener -
        /// Функция отправки уведомлений слушателю
        /// </summary>
        public void PostNotification(object parametr, GameEvents notificationName)
        {
            //If no key in dictionary exists, then exit
            if (!listeners.ContainsKey(notificationName))
                return;
            //Else post notification to all matching listener‘s -  Уведомлять о новых сообщениях всем соответствующим слушателям
            foreach (IObserver listener in listeners[notificationName])
            {
                if (listener != null)
                    //listener.SendMessage(notificationName, sender, SendMessageOptions.DontRequireReceiver);
                    listener.OnNotify(parametr, notificationName);
                else
                    Debug.LogError("listener == null");
            }
        }

        /// <summary>
        /// Function to remove a listener for a notification -  
        /// Функция удаления слушателя для уведомления
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="notificationName"></param>
        public void RemoveListener(IObserver sender, GameEvents notificationName)
        {
            //If no key in dictionary exists, then exit
            if (!listeners.ContainsKey(notificationName))
                return;

            var listListeners = listeners[notificationName];
            int senderId = sender.GetInstanceID();

            //Cycle through listeners and identify component, and then remove 
            //Проведите цикл прослушивающих и идентифицируйте компонент, а затем удалите
            for (int i = listListeners.Count - 1; i >= 0; i--)
            {
                int currentId = listListeners[i].GetInstanceID();
                //Check instance ID - Проверить идентификатор экземпляра
                if (currentId == senderId)
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
            var tmpListeners = new Dictionary<GameEvents, List<MonoBehaviour>>();

            foreach (var item in listeners)
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
            listeners = tmpListeners;
        }

        #endregion

        #region Private Methods

        private void Awake()
        {
            SceneManagerAdapt.AddSceneLoaded(RemoveRedundancies);
        }

        #endregion
    }


    public enum GameEvents
    {
        LoadGameComplete,
        SaveGamePrepare,

        WorkoutLoaded,
        CorrectAnswer,
        NonCorrectAnswer,
        BuildTask,
        WordsEnded,
        ShowResult,
        LoadedVocabulary,
        ContinueWorkout,
        SetQuestWord,
        CoreBuild,
        ButtonHandlerLoaded,
        NotUntrainedWords,
        UpdatedLicenseLevel,
        QuitGame
    }
}
