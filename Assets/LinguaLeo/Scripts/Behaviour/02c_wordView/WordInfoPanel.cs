using System;
using LinguaLeo.Scripts.Helpers;
using LinguaLeo.Scripts.Managers;
using UnityEngine;
using UnityEngine.Serialization;
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

        [FormerlySerializedAs("TimeReduceText")]
        [SerializeField]
        private Text timeReduceText = null;

        [FormerlySerializedAs("TimeUnlockText")]
        [SerializeField]
        private Text timeUnlockText = null;

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
            wordImage.sprite = GameManager.ResourcesLoader.GetPicture(word.pictureUrl);
            wordImage.preserveAspect = true;

            levelText.text = word.progress.license.ToString();
            wordText.text = word.wordValue + " - " + word.translations;
            word.LicenseExpirationCheck();

            TimeSpan time = word.GetLicenseValidityTime();
            timeReduceText.text = MyUtilities.FormatTime(time);

            TimeSpan timeUnlock = word.GetLicenseUnlockForRepeat();
            timeUnlockText.text = MyUtilities.FormatTime(timeUnlock);

            GetComponent<Transform>().localScale = Vector3.one;
            GetComponent<Transform>().localPosition = Vector3.zero;
        }

        #endregion
    }
}
