using LinguaLeo.Scripts.Helpers;
using LinguaLeo.Scripts.Managers;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace LinguaLeo.Scripts.Behaviour._02b_groupChoice
{
    public class WordSetPanel : MonoBehaviour
    {
        #region SerializeFields

        [FormerlySerializedAs("LogoImage")]
        [SerializeField]
        private Image logoImage = null;

        [FormerlySerializedAs("CaptionText")]
        [SerializeField]
        private Text captionText = null;

        [FormerlySerializedAs("WordCountText")]
        [SerializeField]
        private Text wordCountText = null;

        #endregion

        #region Private variables

        private Button learnButton;
        private Button viewButton;

        private string groupName = string.Empty;

        #endregion

        #region Events

        // OnMouseDown is called when the user has pressed the mouse button while over the GUIElement or Collider
        public void OnMouseDown()
        {
            Debug.Log("OnMouseDown");
            PanelClick();
        }

        #endregion

        #region Unity events

        // Use this for initialization
        private void Start()
        {
            learnButton = transform.Find("LearnButton").GetComponent<Button>();
            viewButton = transform.Find("ViewButton").GetComponent<Button>();

            learnButton.onClick.AddListener(PanelClick);
            learnButton.onClick.AddListener(() => GameManager.SceneLoader.LoadLevel(SceneNames.trainingСhoice));

            viewButton.onClick.AddListener(PanelClick);
            viewButton.onClick.AddListener(() => GameManager.SceneLoader.LoadLevel(SceneNames.wordView));
        }

        #endregion

        #region Public Methods

        public string GetName()
        {
            return captionText.text;
        }

        public void Init(Sprite sprite, string caption, int count)
        {
            logoImage.sprite = sprite;
            groupName = caption;
            captionText.text = groupName.Replace('_', ' ');
            wordCountText.text = count + " cлов";

            GetComponent<Transform>().localScale = Vector3.one;
            GetComponent<Transform>().localPosition = Vector3.zero;
        }

        public void PanelClick()
        {
            Debug.Log("LoadGroup = " + groupName);
            GameManager.WordManager.LoadStartWordGroup(groupName);
        }

        public override string ToString()
        {
            return groupName;
        }

        #endregion
    }
}
