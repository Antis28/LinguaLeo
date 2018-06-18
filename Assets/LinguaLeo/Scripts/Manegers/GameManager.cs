using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif



using UnityEngine.UI;

//Component for sending and receiving  
[RequireComponent(typeof(WordManeger))]
[RequireComponent(typeof(LevelManeger))]
[RequireComponent(typeof(ScoreKeeper))]
[RequireComponent(typeof(AudioPlayer))]
[RequireComponent(typeof(NotificationsManager))]
[RequireComponent(typeof(LicensesManager))]
[RequireComponent(typeof(WorkoutManager))]

public class GameManager : MonoBehaviour
{
    //public static Scene lastScene = SceneManager.GetActiveScene();

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
    private static LevelManeger levelManeger = null;

    private static WordManeger wordManeger = null;

    private static WorkoutManager workoutManager = null;

    //--------------------------------------------------------------------------------------
    //C# property to retrieve currently active instance of object, if any
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                CheckManyInstance<GameManager>();
                instance = new GameObject("GameManager").
                            AddComponent<GameManager>(); //create game manager object if required
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

    //C# property to retrieve 
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

    public static LevelManeger LevelManeger
    {
        get
        {
            if (levelManeger == null)
            {
                CheckManyInstance<LevelManeger>();
                levelManeger = instance.GetComponent<LevelManeger>();
            }
            return levelManeger;

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

    private static void CheckManyInstance<T>()where T : Object
    {
#if UNITY_EDITOR
        var manyInstanceSingleton = FindObjectsOfType<T>();
        if (manyInstanceSingleton.Length > 1)
        {
            Debug.LogError("(manyInstanceSingleton)");
            foreach (var item in manyInstanceSingleton)
            {
                Debug.LogError(item.name);
            }
        }
#endif
    }

    // Called before Start on object creation
    void Awake()
    {
        //Check if there is an existing instance of this object
        if( (instance) && (instance.GetInstanceID() != GetInstanceID()) )
            DestroyImmediate(gameObject); //Delete duplicate
        else
        {
            instance = this; //Make this object the only instance
            DontDestroyOnLoad(gameObject); //Set as do not destroy
        }
    }
    void Start()
    {
        
    }
    
    //Restart Game
    public void RestartGame()
    {
        //Load first level        
        SceneManager.LoadScene(0);
    }
    //Exit Game
    public void ExitGame()
    {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
		    Application.Quit();
#endif
    }
    
}
