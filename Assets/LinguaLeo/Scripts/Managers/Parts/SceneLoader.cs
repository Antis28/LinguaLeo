using LinguaLeo.Adapters;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

#endif


namespace LinguaLeo.Scripts.Manegers.Parts
{
    public class SceneLoader : MonoBehaviour
    {
        #region Public variables

        public int lastWorkout = -1;

        #endregion

        #region Public Methods

        public void LoadLevel(string name)
        {
            SceneManagerAdapt.LoadScene(name);
        }

        public void LoadNextLevel()
        {
            //Application.loadedLevel
            int buildIndex = SceneManagerAdapt.GetActiveScene().buildIndex + 1;
            SceneManagerAdapt.LoadScene(buildIndex);
        }

        public void LoadResultWorkOut()
        {
            const string nameScene = "result";
            lastWorkout = SceneManagerAdapt.GetActiveScene().buildIndex;
            LoadLevel(nameScene);
        }

        public void QuitGame()
        {
            Application.Quit();

#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
        }

        #endregion
    }
}
