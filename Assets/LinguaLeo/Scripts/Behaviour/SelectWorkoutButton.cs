using LinguaLeo.Scripts.Helpers;
using LinguaLeo.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace LinguaLeo.Scripts.Behaviour
{
    public class SelectWorkoutButton : MonoBehaviour
    {
        #region SerializeFields

        [SerializeField]
        private WorkoutNames workoutName = WorkoutNames.WordTranslate;

        #endregion

        #region Unity events

        // Use this for initialization
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(
                () => GameManager.WorkoutManager.RunWorkOut(workoutName)
            );
        }

        #endregion
    }
}
