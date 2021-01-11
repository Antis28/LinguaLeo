using System.Collections.Generic;
using Helpers;
using UnityEngine;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour
{
    #region SerializeFields

    [SerializeField]
    private Text[] words = null;

    #endregion

    #region Public Methods

    public void FillPanel(List<QuestionLeo> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] != null)
                words[i].text = list[i].questWord.wordValue;
        }
    }

    #endregion
}