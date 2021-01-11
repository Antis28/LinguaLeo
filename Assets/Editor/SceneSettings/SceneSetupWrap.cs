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
            Debug.Log($"Загружено:  {vWrap.PathToScenes}{vWrap.Path.name}.unity");
            return new UnityEditor.SceneManagement.SceneSetup()
            {
                
                path = $"{vWrap.PathToScenes}{vWrap.Path.name}.unity",
                
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

        public string PathToScenes { get; set; }
    }
}
