using System.Collections.Generic;
using LinguaLeo.Scripts.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace LinguaLeo.Scripts
{
    public class DebugUI : MonoBehaviour
    {
        [SerializeField]
        Text[] words = null;


        public void FillPanel(List<QuestionLeo> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] != null)
                    words[i].text = list[i].questWord.wordValue;
            }
        }
    }
}
