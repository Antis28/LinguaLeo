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

        void IObserver.OnNotify(object parametr, GameEvents notificationName)
        {
            switch (notificationName)
            {
                case GameEvents.LoadedVocabulary:
                    ShowWordCount();
                    break;
            }
        }

        #endregion

        #region Unity events

        // Use this for initialization
        private void Start()
        {
            Transform countTransform = transform.Find("CountText");
            if (countTransform == null)
                return;
            countText = countTransform.GetComponent<Text>();
            GameManager.Notifications.AddListener(this, GameEvents.LoadedVocabulary);
        }

        #endregion

        #region Private Methods

        private void ShowWordCount()
        {
            if (countText == null)
                return;
            countText.text = GameManager.WordManager.GetUntrainedGroupWords(workoutName).Count.ToString();
        }

        // Update is called once per frame
        private void Update() { }

        #endregion
    }
}
