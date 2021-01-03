using System.Collections;
using LinguaLeo.Scripts.Behaviour;
using LinguaLeo.Scripts.Helpers.Interfaces;
using LinguaLeo.Scripts.Helpers.ResourceLoading.ResourceLoaderImplements;
using LinguaLeo.Scripts.Managers.Parts;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace LinguaLeo.Scripts.Managers
{
    /// <summary>
    /// Хранилище ссылок на синглтоны
    /// </summary>
// Component for sending and receiving  
    [RequireComponent(typeof(WordManager))]
    [RequireComponent(typeof(SceneLoader))]
    [RequireComponent(typeof(ScoreKeeper))]
    [RequireComponent(typeof(AudioPlayer))]
    [RequireComponent(typeof(NotificationsManager))]
    [RequireComponent(typeof(LicensesManager))]
    [RequireComponent(typeof(WorkoutManager))]
    public class GameManager : MonoBehaviour
    {
        #region Static Fields and Constants

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

        private static WordManager wordManager = null;

        private static WorkoutManager workoutManager = null;

        private static ExternalResourceManager resourcesLoader = null;

        #endregion

        #region Public variables

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

        public static WordManager WordManager
        {
            get
            {
                if (wordManager == null)
                {
                    CheckManyInstance<WordManager>();
                    wordManager = instance.GetComponent<WordManager>();
                }

                return wordManager;
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

        public static ExternalResourceManager ResourcesManager => resourcesLoader ?? (resourcesLoader = new ExternalResourceManager());

        #endregion

        #region Unity events

        private void Start()
        {
#if UNITY_ANDROID
            StartCoroutine(LoadYourAsyncScene("01a_Start_Android"));
#else
            StartCoroutine(LoadYourAsyncScene("01a_Start"));
#endif
        }

        #endregion

        #region Private Methods

        // Called before Start on object creation
        private void Awake()
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

        #endregion

        //public static Scene lastScene = SceneManager.GetActiveScene();
    }
}
