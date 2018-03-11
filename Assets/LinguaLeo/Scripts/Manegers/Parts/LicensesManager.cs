using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LicensesManager : MonoBehaviour,Observer
{

    const float CHECK_INTERVAL = 900f;

    void Start()
    {
        GameManager.Notifications.AddListener(this, GAME_EVENTS.LoadedVocabulary);
    }

    // Use this for initialization
    IEnumerator ChekLoop()
    {
        List<WordLeo> allWords = GameManager.WordManeger.GetAllWords();

        while (true)
        {
            foreach (var word in allWords)
            {
                word.CheckingTimeForTraining();
                yield return null;
            }

            yield return new WaitForSeconds(CHECK_INTERVAL);
        }
    }

    void Observer.OnNotify(Component sender, GAME_EVENTS notificationName)
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
