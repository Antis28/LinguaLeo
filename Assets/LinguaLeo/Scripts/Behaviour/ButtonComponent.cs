// NULLcode Studio © 2016
// null-code.ru

using UnityEngine;
using UnityEngine.UI;

namespace LinguaLeo.Scripts.Behaviour
{
    public class ButtonComponent : MonoBehaviour
    {

        public Button button;
        public Text text;
        public RectTransform rect;

        public void Awake()
        {
            button = GetComponent<Button>();
            text = GetComponentInChildren<Text>();
            rect = GetComponent<RectTransform>();
        }
    }
}
