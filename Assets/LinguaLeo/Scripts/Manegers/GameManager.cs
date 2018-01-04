using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Component for sending and receiving  
[RequireComponent(typeof(NotificationsManager))]
public class GameManager : MonoBehaviour
{
    //Internal reference to single active instance of object - for singleton behaviour
    private static GameManager instance = null;
    //Internal reference to notifications object
    private static NotificationsManager notifications = null;
    //Internal reference to Saveload Game Manager
    private static LoadSaveManager statemanager = null;

    //Internal reference to notifications object
    private static AudioPlayer aPlayer = null;

    //--------------------------------------------------------------------------------------
    //C# property to retrieve currently active instance of object, if any
    public static GameManager Instance
    {
        get
        {
            if( instance == null )
                instance = new GameObject("GameManager").
                            AddComponent<GameManager>(); //create game manager object if required
            return instance;

        }
    }

    //C# property to retrieve notifications manager
    public static NotificationsManager Notifications
    {
        get
        {
            if (notifications == null)
                notifications = instance.GetComponent<NotificationsManager>();
            return notifications;
        }
    }

    //C# property to retrieve save/load manager
    public static LoadSaveManager StateManager
    {
        get
        {
            if (statemanager == null)
                statemanager = instance.GetComponent<LoadSaveManager>();
            return statemanager;

        }
    }

    //C# property to retrieve 
    public static AudioPlayer audioPlayer
    {
        get
        {
            if (aPlayer == null)
                aPlayer = instance.GetComponent<AudioPlayer>();
            return aPlayer;

        }
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
