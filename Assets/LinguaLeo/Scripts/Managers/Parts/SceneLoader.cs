using LinguaLeo.Adapters;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR

#endif


namespace LinguaLeo.Scripts.Manegers.Parts
{
    public class SceneLoader : MonoBehaviour
    {
        public int lastWorkout = -1;

        public void QuitGame()
        {
            Application.Quit();

#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
        }

        public void LoadLevel(string name)
        {
            SceneManagerAdapt.LoadScene(name);
        }

        public void LoadResultWorkOut()
        {
            const string nameScene = "result";
            lastWorkout = SceneManagerAdapt.GetActiveScene().buildIndex;
            LoadLevel(nameScene);
        }

        public void LoadNextLevel()
        {
            //Application.loadedLevel
            int buildIndex = SceneManagerAdapt.GetActiveScene().buildIndex + 1;
            SceneManagerAdapt.LoadScene(buildIndex);
        }
    }
}
