using LinguaLeo.Scripts.Helpers;
using LinguaLeo.Scripts.Manegers;
using UnityEngine;
using UnityEngine.UI;

namespace LinguaLeo.Scripts.Behaviour
{
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
}
