using LinguaLeo.Scripts.Managers;
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
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(() => GameManager.SceneLoader.LoadLevel(sceneName));
        }

        #endregion
    }
}
