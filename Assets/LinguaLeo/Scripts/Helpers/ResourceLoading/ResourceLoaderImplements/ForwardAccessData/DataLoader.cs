#region

using System.IO;
using Helpers.ResourceLoading.ResourceLoaderImplements.ForwardAccessData.Parts;
using UnityEngine;

#endregion

namespace Helpers.ResourceLoading.ResourceLoaderImplements.ForwardAccessData
{
    /// <summary>
    /// Загрузка с диска обычным IO.
    /// </summary>
    public class DataLoader : AbstractLoader
    {
        #region Private variables

        private readonly string folderXml = @"Data";

        #endregion

        #region Public Methods

        public DataLoader()
        {
            SelectPathByPlatform();
            InitAllLoaders();
        }

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
    }
}
