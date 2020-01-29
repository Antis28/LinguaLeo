using LinguaLeo.Scripts.Manegers;
using LinguaLeo.Scripts.Manegers.Parts;
using UnityEngine;
using UnityEngine.UI;

namespace LinguaLeo.Scripts.Behaviour
{
    public class LoadSceneButton : MonoBehaviour {
        [SerializeField]
        private string sceneName = string.Empty;

        private LevelManeger levelManeger;

        // Use this for initialization
        void Start () {
            levelManeger = FindObjectOfType<LevelManeger>();
            GetComponent<Button>().onClick.AddListener(() => GameManager.LevelManeger.LoadLevel(sceneName));
        
        }
    }
}
