using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

#if UNITY_EDITOR
namespace SceneSettings
{
	[CreateAssetMenu(fileName = "scene_setup", menuName = "Data/SceneSetup")]
	public class SceneSetupData : ScriptableObject {
	
		[System.Serializable]
		public struct SetupData {
			public int index;
			public string name;
			public SceneSetupWrap[] setup;
		}

		public List<SetupData> setup;
		
	}
}

#endif
