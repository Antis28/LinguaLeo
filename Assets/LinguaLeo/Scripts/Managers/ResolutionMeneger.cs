using UnityEngine;
using UnityEngine.UI;

namespace LinguaLeo.Scripts.Managers
{
    public class ResolutionMeneger : MonoBehaviour
    {
        #region Static Fields and Constants

        private const string ScreenWidth = "screen_width";
        private const string ScreenHeight = "screen_height";
        private const string Fullscreen = "screen_isFull";

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
            PlayerPrefs.SetInt(ScreenWidth, width);
            PlayerPrefs.SetInt(ScreenHeight, height);
            PlayerPrefs.SetString(Fullscreen, fullscreen.ToString());

            Screen.SetResolution(width, height, fullscreen);
            print("SetResolution");
        }

        #endregion

        #region Private Methods

        // Use this for initialization
        private void Awake()
        {
            int width = PlayerPrefs.GetInt(ScreenWidth, 640);
            int height = PlayerPrefs.GetInt(ScreenHeight, 480);
            bool fullScreen = bool.Parse(PlayerPrefs.GetString(Fullscreen, "false"));

            SetResolution(width, height, fullScreen);
            GetComponent<Dropdown>().onValueChanged.AddListener(OnValueChanged);
        }

        #endregion
    }
}
