using System.Collections;
using System.Collections.Generic;
using Helpers;
using Helpers.Interfaces;
using UnityEngine;

namespace Managers.Parts
{
    public class LicensesManager : MonoBehaviour, IObserver
    {
        #region Static Fields and Constants

        private const float CHECK_INTERVAL = 900f;

        #endregion

        #region Events

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

        #endregion

        #region Unity events

        private void Start()
        {
            //GameManager.Notifications.AddListener(this, GAME_EVENTS.LoadedVocabulary);
        }

        #endregion

        #region Private Methods

        // Use this for initialization
        private IEnumerator ChekLoop()
        {
            List<WordLeo> allWords = GameManager.WordManager.GetAllWords();

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

        #endregion
    }
}
