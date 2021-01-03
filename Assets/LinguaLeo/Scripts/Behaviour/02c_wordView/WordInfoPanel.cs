using System;
using LinguaLeo.Scripts.Helpers;
using LinguaLeo.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace LinguaLeo.Scripts.Behaviour._02c_wordView
{
    public class WordInfoPanel : MonoBehaviour
    {
        #region SerializeFields

        [SerializeField]
        private Image wordImage = null;

        [SerializeField]
        private Text wordText = null;

        [SerializeField]
        private Text levelText = null;

        [SerializeField]
        private Text TimeReduceText = null;

        [SerializeField]
        private Text TimeUnlockText = null;

        #endregion

        #region Events

        // OnMouseDown is called when the user has pressed the mouse button while over the GUIElement or Collider
        public void OnMouseDown()
        {
            Debug.Log("OnMouseDown");
        }

        #endregion

        #region Unity events

        private void Start()
        {
            //  SceneLoader sceneLoader = FindObjectOfType<SceneLoader>();
        }

        #endregion

        #region Public Methods

        public string GetName()
        {
            return wordText.text;
        }

        public void Init(WordLeo word)
        {
            wordImage.sprite = GameManager.ResourcesManager.GetPicture(word.pictureUrl);
            wordImage.preserveAspect = true;

            levelText.text = word.progress.license.ToString();
            wordText.text = word.wordValue + " - " + word.translations;
            word.LicenseExpirationCheck();

            TimeSpan time = word.GetLicenseValidityTime();
            TimeReduceText.text = MyUtilities.FormatTime(time);

            TimeSpan timeUnlock = word.GetLicenseUnlockForRepeat();
            TimeUnlockText.text = MyUtilities.FormatTime(timeUnlock);

            GetComponent<Transform>().localScale = Vector3.one;
            GetComponent<Transform>().localPosition = Vector3.zero;
        }

        #endregion
    }
}
