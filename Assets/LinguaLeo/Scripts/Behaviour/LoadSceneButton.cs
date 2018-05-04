using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
