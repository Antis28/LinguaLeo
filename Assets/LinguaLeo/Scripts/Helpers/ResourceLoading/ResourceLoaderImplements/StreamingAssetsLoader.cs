using System.IO;
using UnityEngine;

namespace LinguaLeo.Scripts.Helpers.ResourceLoading.ResourceLoaderImplements
{
    public class StreamingAssetsLoader : AbstractLoader, IResourcesLoader
    {
        public StreamingAssetsLoader()
        {
            SelectPathByPlatform();
            InitAllLoaders();
        }

        private void SelectPathByPlatform()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            //Android uses files inside a compressed APK/JAR file
            //To read streaming Assets on platforms like Android and WebGL
            //, where you cannot access streaming Asset files directly, use UnityWebRequest.
            //For an example, see Application.streamingAssetsPath. On many platforms, the streaming assets folder
            //location is read-only; you can not modify or write new files there at runtime.
            //Use Application.persistentDataPath for a folder location that is writable.
            pathToRootResources = "jar:file://" + Application.dataPath + "!/assets";
#endif

#if UNITY_EDITOR || UNITY_STANDALONE
            // Most platforms (Unity Editor, Windows, Linux players, PS4, Xbox One, Switch) use Application.dataPath + "/StreamingAssets"
            pathToRootResources = Path.Combine(Application.dataPath, "StreamingAssets");
#endif
            pathToRootResources = Path.Combine(pathToRootResources, "Data");
        }

        public WordCollection LoadVocabulary() { throw new System.NotImplementedException(); }
        public void SaveVocabulary(WordCollection vocabulary) { throw new System.NotImplementedException(); }
    }
}
