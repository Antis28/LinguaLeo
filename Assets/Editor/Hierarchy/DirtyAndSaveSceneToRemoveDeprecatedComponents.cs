using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Hierarchy {
    public class DirtyAndSaveSceneToRemoveDeprecatedComponents : MonoBehaviour
    {
        #region Private Methods

        [MenuItem("Cleanup/Cleanup Scenes with Deprecated Components")]
        private static void DoCleanup()
        {
            var sceneGUIDs = AssetDatabase.FindAssets("t:Scene");
            string[] scenePaths = sceneGUIDs.Select(i => AssetDatabase.GUIDToAssetPath(i)).ToArray();

            for (int i = 0; i < scenePaths.Length; i++)
            {
                var scene = EditorSceneManager.OpenScene(scenePaths[i], OpenSceneMode.Additive);

                EditorSceneManager.MarkSceneDirty(scene);
                EditorSceneManager.SaveScene(scene);

                if (scene != EditorSceneManager.GetActiveScene())
                    EditorSceneManager.UnloadSceneAsync(scene);
            }
        }

        #endregion
    }
}
