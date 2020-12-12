using LinguaLeo.Scripts.Helpers;
using LinguaLeo.Scripts.Helpers.Interfaces;
using LinguaLeo.Scripts.Helpers.ResourceLoading;
using LinguaLeo.Scripts.Manegers;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace LinguaLeo.Scripts.Workout
{
    /// <summary>
    /// Является базой для тренировок.
    /// Содержит общие поля и методы для них.
    /// </summary>
    public class AbstractWorkout : MonoBehaviour, IWorkout
    {
        protected Workout core;

        [FormerlySerializedAs("WorkoutName")]
        [SerializeField]
        protected WorkoutNames workoutName = WorkoutNames.WordTranslate;

        [SerializeField]
        protected Text questionText = null; // Поле для вопроса

        [SerializeField]
        protected Image wordImage = null; // Картинка ассоциаци со словом

        WorkoutNames IWorkout.WorkoutName => workoutName;

        protected void SetImage(string fileName)
        {
            var sprite = GameManager.ResourcesLoader.GetPicture(fileName);
            wordImage.sprite = sprite;
            wordImage.preserveAspect = true;
        }

        protected void HideImage() { wordImage.enabled = false; }
        protected void ShowImage() { wordImage.enabled = true; }

        WordLeo IWorkout.GetCurrentWord() { return core.GetCurrentWord(); }

        Workout IWorkout.GetCore() { return core; }
    }
}
