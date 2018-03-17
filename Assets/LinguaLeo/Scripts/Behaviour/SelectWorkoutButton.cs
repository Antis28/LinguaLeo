using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectWorkoutButton : MonoBehaviour
{    
    [SerializeField]
    private WorkoutNames workoutName = WorkoutNames.WordTranslate;

    // Use this for initialization
    void Start()
    {
            GetComponent<Button>().onClick.AddListener(
                () => GameManager.WorkoutManager.RunWorkOut(workoutName)
                );
    }
}
