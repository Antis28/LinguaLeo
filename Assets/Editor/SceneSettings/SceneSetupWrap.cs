// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using UnityEditor;
using UnityEngine;

namespace SceneSettings
{
    [Serializable]
    public class SceneSetupWrap
    {
        [SerializeField]
        private SceneAsset path;

        [SerializeField]
        private bool isLoaded;
        
        [SerializeField]
        private bool isActive;

        public static implicit operator UnityEditor.SceneManagement.SceneSetup(SceneSetupWrap vWrap)
        {
            Debug.Log($"Загружено: {AssetDatabase.GetAssetPath(vWrap.Path)}");
                          
            return new UnityEditor.SceneManagement.SceneSetup
            {
                
                path = AssetDatabase.GetAssetPath(vWrap.Path),
                
                isActive = vWrap.IsActive,
                isLoaded = vWrap.IsLoaded
            };
        }


        public bool IsActive
        {
            get => isActive;
            set => isActive = value;
        }

        public bool IsLoaded
        {
            get => isLoaded;
            set => isLoaded = value;
        }

        public SceneAsset Path
        {
            get => path;
            set => path = value;
        }

    }
}
