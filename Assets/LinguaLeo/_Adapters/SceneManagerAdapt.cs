using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneManagerAdapt
{

    public static AsyncOperation LoadSceneAsync(string sceneName, LoadSceneMode mode)
    {
        return SceneManager.LoadSceneAsync("wordinfo", LoadSceneMode.Additive);
    }

    public static void LoadScene(int sceneBuildIndex)
    {
        SceneManager.LoadScene(sceneBuildIndex);
    }

    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public static Scene GetActiveScene()
    {
        return SceneManager.GetActiveScene();
    }

    public static void AddSceneLoaded(UnityAction action)
    {
        SceneManager.sceneLoaded += (arg0, mode) => action();
    }
}
