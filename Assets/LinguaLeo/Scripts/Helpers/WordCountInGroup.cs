using LinguaLeo.Scripts.Helpers.Interfaces;
using LinguaLeo.Scripts.Manegers;
using UnityEngine;
using UnityEngine.UI;

namespace LinguaLeo.Scripts.Helpers
{
    public class WordCountInGroup : MonoBehaviour, IObserver
    {
        [SerializeField]
        WorkoutNames workoutName = WorkoutNames.WordTranslate;
        Text countText;

        // Use this for initialization
        void Start () {
            Transform CountTransform = transform.Find("CountText");
            if (CountTransform == null)
                return;
            countText = CountTransform.GetComponent<Text>();
            GameManager.Notifications.AddListener(this, GAME_EVENTS.LoadedVocabulary);
        }
	
        // Update is called once per frame
        void Update () {
		
        }

        void ShowWordCount()
        {
            if (countText == null)
                return;
            countText.text = GameManager.WordManeger.GetUntrainedGroupWords(workoutName).Count.ToString();
        }

        void IObserver.OnNotify(object parametr, GAME_EVENTS notificationName)
        {
            switch (notificationName)
            {
                case GAME_EVENTS.LoadedVocabulary:
                    ShowWordCount();
                    break;
            }
        }
    }
}
