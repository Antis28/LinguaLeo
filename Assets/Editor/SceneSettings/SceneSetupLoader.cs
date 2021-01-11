using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace SceneSettings
{
	public class SceneSetupLoader : MonoBehaviour {
		static SceneSetupData data;
	
		[InitializeOnLoadMethod]
		public static void Load() {
			data = (SceneSetupData)AssetDatabase.LoadAssetAtPath("Assets/Setups/scene_setup.asset", typeof(SceneSetupData));
		}

		[MenuItem("Scenes/Scene Setup 1 &1")]
		static void SceneSetup1() {
			Load(1);
		}

		[MenuItem("Scenes/Scene Setup 2 &2")]
		static void SceneSetup2() {
			Load(2);
		}

		[MenuItem("Scenes/Scene Setup 3 &3")]
		static void SceneSetup3() {
			Load(3);
		}

		[MenuItem("Scenes/Scene Setup 4 &4")]
		static void SceneSetup4() {
			Load(4);
		}

		[MenuItem("Scenes/Scene Setup 5 &5")]
		static void SceneSetup5() {
			Load(5);
		}

		[MenuItem("Scenes/Scene Setup 6 &6")]
		static void SceneSetup6() {
			Load(6);
		}

		static void Load(int index){
			var config = data.setup.FirstOrDefault(d => d.index == index);
			if(config.setup == null){
				Debug.LogError(string.Format("Scene setup with index {0} not configured. Please add it to Assets/data/scene_setup", index));
			} else {
				EditorUtility.DisplayProgressBar("Loading scene setup", "Please wait a bit, work in progress", 0);

				var sceneSetups = config.setup.Select(vWrap => (UnityEditor.SceneManagement.SceneSetup) vWrap).ToArray();
				EditorSceneManager.RestoreSceneManagerSetup(sceneSetups);
				EditorUtility.DisplayProgressBar("Loading scene setup", "Please wait a bit, work in progress", 1);
				EditorUtility.ClearProgressBar();
			}		
		}
	}
}
