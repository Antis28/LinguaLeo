using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace LinguaLeo.Scripts.Helpers.ResourceLoading.ResourceLoaderImplements
{
    /// <summary>
    /// Загрузка с диска обычным IO.
    /// </summary>
    public class DataLoader : AbstractLoader
    {
        #region Private variables

        private readonly string folderXml = @"Data";

        #endregion

        #region Private Methods

        private void SelectPathByPlatform()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
#endif

#if UNITY_EDITOR || UNITY_STANDALONE
            pathToRootResources = Path.Combine(Application.dataPath, folderXml);
            pathToRootResources = folderXml;
#endif
        }

        #endregion

        public DataLoader()
        {
            SelectPathByPlatform();
            InitAllLoaders();
        }
    }
}
