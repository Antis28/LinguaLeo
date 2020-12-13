using UnityEngine;
using UnityEngine.UI;

namespace LinguaLeo.Scripts.Manegers
{
    public class ResolutionMeneger : MonoBehaviour
    {
        #region Static Fields and Constants

        const string SCREEN_WIDTH = "screen_width";
        const string SCREEN_HEIGHT = "screen_height";
        const string FULLSCREEN = "screen_isFull";

        #endregion

        #region Events

        public void OnValueChanged(int value)
        {
            switch (value)
            {
                case 0:
                    SetResolution(1280, 720, true);
                    break;
                case 1:
                    SetResolution(1920, 1080, true);
                    break;
                case 2:
                    SetResolution(1280, 1024, true);
                    break;
            }
        }

        #endregion

        #region Public Methods

        public void SetResolution(int width, int height, bool fullscreen = false)
        {
            PlayerPrefs.SetInt(SCREEN_WIDTH, width);
            PlayerPrefs.SetInt(SCREEN_HEIGHT, height);
            PlayerPrefs.SetString(FULLSCREEN, fullscreen.ToString());

            Screen.SetResolution(width, height, fullscreen);
            print("SetResolution");
        }

        #endregion

        #region Private Methods

        // Use this for initialization
        void Awake()
        {
            int width = PlayerPrefs.GetInt(SCREEN_WIDTH, 640);
            int height = PlayerPrefs.GetInt(SCREEN_HEIGHT, 480);
            bool fullScreen = bool.Parse(PlayerPrefs.GetString(FULLSCREEN, "false"));

            SetResolution(width, height, fullScreen);
            GetComponent<Dropdown>().onValueChanged.AddListener(OnValueChanged);
        }

        #endregion
    }
}
