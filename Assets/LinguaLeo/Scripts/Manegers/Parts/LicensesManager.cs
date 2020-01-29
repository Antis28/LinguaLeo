using System.Collections;
using System.Collections.Generic;
using LinguaLeo.Scripts.Helpers;
using LinguaLeo.Scripts.Helpers.Interfaces;
using UnityEngine;

namespace LinguaLeo.Scripts.Manegers.Parts
{
    public class LicensesManager : MonoBehaviour,IObserver
    {

        const float CHECK_INTERVAL = 900f;

        void Start()
        {
            //GameManager.Notifications.AddListener(this, GAME_EVENTS.LoadedVocabulary);
        }

        // Use this for initialization
        IEnumerator ChekLoop()
        {
            List<WordLeo> allWords = GameManager.WordManeger.GetAllWords();

            while (true)
            {
                foreach (var word in allWords)
                {
                    word.LicenseExpirationCheck();
                    yield return null;
                }

                yield return new WaitForSeconds(CHECK_INTERVAL);
            }
        }

        void IObserver.OnNotify(object parametr, GAME_EVENTS notificationName)
        {
            switch (notificationName)
            {            
                case GAME_EVENTS.LoadedVocabulary:
                    StopCoroutine(ChekLoop());
                    //StartCoroutine(ChekLoop());
                    break;
            }
        }
    }
}
