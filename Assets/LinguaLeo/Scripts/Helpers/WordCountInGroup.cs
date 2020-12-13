using LinguaLeo.Scripts.Helpers.Interfaces;
using LinguaLeo.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace LinguaLeo.Scripts.Helpers
{
    public class WordCountInGroup : MonoBehaviour, IObserver
    {
        #region SerializeFields

        [SerializeField]
        private WorkoutNames workoutName = WorkoutNames.WordTranslate;

        #endregion

        #region Private variables

        private Text countText;

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
        private void Start()
        {
            Transform CountTransform = transform.Find("CountText");
            if (CountTransform == null)
                return;
            countText = CountTransform.GetComponent<Text>();
            GameManager.Notifications.AddListener(this, GAME_EVENTS.LoadedVocabulary);
        }

        #endregion

        #region Private Methods

        private void ShowWordCount()
        {
            if (countText == null)
                return;
            countText.text = GameManager.WordManeger.GetUntrainedGroupWords(workoutName).Count.ToString();
        }

        // Update is called once per frame
        private void Update() { }

        #endregion
    }
}
