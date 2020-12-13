using LinguaLeo.Scripts.Helpers.Interfaces;
using LinguaLeo.Scripts.Manegers;
using UnityEngine;
using UnityEngine.UI;

namespace LinguaLeo.Scripts.Helpers
{
    public class WordCountInGroup : MonoBehaviour, IObserver
    {
        #region SerializeFields

        [SerializeField]
        WorkoutNames workoutName = WorkoutNames.WordTranslate;

        #endregion

        #region Private variables

        Text countText;

        #endregion

        #region Events

        void IObserver.OnNotify(object parametr, GAME_EVENTS notificationName)
        {
            switch (notificationName)
            {
                case GAME_EVENTS.LoadedVocabulary:
                    ShowWordCount();
                    break;
            }
        }

        #endregion

        #region Unity events

        // Use this for initialization
        void Start()
        {
            Transform CountTransform = transform.Find("CountText");
            if (CountTransform == null)
                return;
            countText = CountTransform.GetComponent<Text>();
            GameManager.Notifications.AddListener(this, GAME_EVENTS.LoadedVocabulary);
        }

        #endregion

        #region Private Methods

        void ShowWordCount()
        {
            if (countText == null)
                return;
            countText.text = GameManager.WordManeger.GetUntrainedGroupWords(workoutName).Count.ToString();
        }

        // Update is called once per frame
        void Update() { }

        #endregion
    }
}
