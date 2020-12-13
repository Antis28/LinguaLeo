using LinguaLeo.Scripts.Manegers;
using LinguaLeo.Scripts.Manegers.Parts;
using UnityEngine;
using UnityEngine.UI;

namespace LinguaLeo.Scripts.Behaviour
{
    public class LoadSceneButton : MonoBehaviour
    {
        #region SerializeFields

        [SerializeField]
        private string sceneName = string.Empty;

        #endregion

        #region Unity events

        // Use this for initialization
        void Start()
        {
            GetComponent<Button>().onClick.AddListener(() => GameManager.SceneLoader.LoadLevel(sceneName));
        }

        #endregion
    }
}
