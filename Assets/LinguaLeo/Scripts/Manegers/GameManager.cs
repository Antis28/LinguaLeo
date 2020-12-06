using System;
using System.Collections;
using System.Collections.Generic;
using LinguaLeo.Scripts.Behaviour;
using LinguaLeo.Scripts.Manegers.Parts;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace LinguaLeo.Scripts.Manegers
{
    /// <summary>
    /// Хранилище ссылок на синглтоны
    /// </summary>
// Component for sending and receiving  
    [RequireComponent(typeof(WordManeger))]
    [RequireComponent(typeof(SceneLoader))]
    [RequireComponent(typeof(ScoreKeeper))]
    [RequireComponent(typeof(AudioPlayer))]
    [RequireComponent(typeof(NotificationsManager))]
    [RequireComponent(typeof(LicensesManager))]
    [RequireComponent(typeof(WorkoutManager))]
    public class GameManager : MonoBehaviour
    {
        //public static Scene lastScene = SceneManager.GetActiveScene();

        #region Поля

        //Internal reference to single active instance of object - for singleton behaviour
        private static GameManager instance = null;

        //Internal reference to notifications object
        private static NotificationsManager notifications = null;

        //Internal reference to Saveload Game Manager
        private static LoadSaveManager statemanager = null;

        //Internal reference to notifications object
        private static AudioPlayer audioPlayer = null;

        //Internal reference to notifications object
        private static ScoreKeeper scoreKeeper = null;

        //Internal reference to notifications object
        private static SceneLoader sceneLoader = null;

        private static WordManeger wordManeger = null;

        private static WorkoutManager workoutManager = null;

        #endregion

        #region Свойства

        /// <summary>
        /// Экземпляр синглтона GameManager
        /// </summary>
        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    CheckManyInstance<GameManager>();
                    instance = new GameObject("GameManager")
                        .AddComponent<GameManager>(); //create game manager object if required
                }

                return instance;
            }
        }

        //C# property to retrieve notifications manager
        public static NotificationsManager Notifications
        {
            get
            {
                if (notifications == null)
                {
                    CheckManyInstance<NotificationsManager>();
                    notifications = instance.GetComponent<NotificationsManager>();
                }

                return notifications;
            }
        }

        //C# property to retrieve save/load manager
        public static LoadSaveManager StateManager
        {
            get
            {
                if (statemanager == null)
                {
                    CheckManyInstance<LoadSaveManager>();
                    statemanager = instance.GetComponent<LoadSaveManager>();
                }

                return statemanager;
            }
        }

        //C# property to retrieve 
        public static AudioPlayer AudioPlayer
        {
            get
            {
                if (audioPlayer == null)
                {
                    CheckManyInstance<AudioPlayer>();
                    audioPlayer = instance.GetComponent<AudioPlayer>();
                }

                return audioPlayer;
            }
        }

        #region C# property to retrieve

        public static ScoreKeeper ScoreKeeper
        {
            get
            {
                if (scoreKeeper == null)
                {
                    CheckManyInstance<ScoreKeeper>();
                    scoreKeeper = instance.GetComponent<ScoreKeeper>();
                }

                return scoreKeeper;
            }
        }

        public static SceneLoader SceneLoader
        {
            get
            {
                if (sceneLoader == null)
                {
                    CheckManyInstance<SceneLoader>();
                    sceneLoader = instance.GetComponent<SceneLoader>();
                }

                return sceneLoader;
            }
        }

        public static WordManeger WordManeger
        {
            get
            {
                if (wordManeger == null)
                {
                    CheckManyInstance<WordManeger>();
                    wordManeger = instance.GetComponent<WordManeger>();
                }

                return wordManeger;
            }
        }

        public static WorkoutManager WorkoutManager
        {
            get
            {
                if (workoutManager == null)
                {
                    CheckManyInstance<WorkoutManager>();
                    workoutManager = instance.GetComponent<WorkoutManager>();
                }

                return workoutManager;
            }
        }

        #endregion

        #endregion

        #region Другие методы

        private static void CheckManyInstance<T>() where T : Object
        {
#if UNITY_EDITOR
            var manyInstanceSingleton = FindObjectsOfType<T>();
            if (manyInstanceSingleton.Length > 1)
            {
                Debug.LogError("(manyInstanceSingleton)");
                foreach (var item in manyInstanceSingleton) { Debug.LogError(item.name); }
            }
#endif
        }

        #endregion

        #region Жизненый цикл

        // Called before Start on object creation
        void Awake()
        {
            //Check if there is an existing instance of this object
            if ((instance) && (instance.GetInstanceID() != GetInstanceID()))
                DestroyImmediate(gameObject); //Delete duplicate
            else
            {
                instance = this;               //Make this object the only instance
                DontDestroyOnLoad(gameObject); //Set as do not destroy
            }
        }

        private void Start()
        {
#if UNITY_ANDROID
            StartCoroutine(LoadYourAsyncScene("01a_Start_Android"));
#else
            StartCoroutine(LoadYourAsyncScene("01a_Start"));
#endif
        }

        #endregion

        private IEnumerator LoadYourAsyncScene(string sceneName)
        {
            // The Application loads the Scene in the background as the current Scene runs.
            // This is particularly good for creating loading screens.
            // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
            // a sceneBuildIndex of 1 as shown in Build Settings.

            var asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone) { yield return null; }
        }
    }
}
