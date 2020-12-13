using LinguaLeo.Scripts.Helpers;
using LinguaLeo.Scripts.Manegers;
using UnityEngine;
using UnityEngine.UI;

namespace LinguaLeo.Scripts.Behaviour._02b_groupСhoice
{
    public class WordSetPanel : MonoBehaviour
    {
        #region SerializeFields

        [SerializeField]
        Image LogoImage = null;

        [SerializeField]
        Text CaptionText = null;

        [SerializeField]
        Text WordCountText = null;

        #endregion

        #region Private variables

        Button learnButton;
        Button viewButton;

        string groupName = string.Empty;

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
        void Start()
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
            return CaptionText.text;
        }

        public void Init(Sprite sprite, string caption, int count)
        {
            LogoImage.sprite = sprite;
            groupName = caption;
            CaptionText.text = groupName.Replace('_', ' ');
            WordCountText.text = count + " cлов";

            GetComponent<Transform>().localScale = Vector3.one;
            GetComponent<Transform>().localPosition = Vector3.zero;
        }

        public void PanelClick()
        {
            Debug.Log("LoadGroup = " + groupName);
            GameManager.WordManeger.LoadStartWordGroup(groupName);
        }

        public override string ToString()
        {
            return groupName;
        }

        #endregion
    }
}
