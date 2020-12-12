using System.IO;
using UnityEngine;

namespace LinguaLeo.Scripts.Helpers.ResourceLoading.ResourceLoaderImplements
{
    /// <summary>
    /// Загрузка с диска обычным IO.
    /// </summary>
    public class DataLoader : AbstractLoader, IResourcesLoader
    {
        private readonly string folderXml = @"Data";

        public DataLoader()
        {
            SelectPathByPlatform();
            InitAllLoaders();
        }

        private void SelectPathByPlatform()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
#endif

#if UNITY_EDITOR || UNITY_STANDALONE
            pathToRootResources = Path.Combine(Application.dataPath, folderXml);
            pathToRootResources = folderXml;
#endif
        }

        public WordCollection LoadVocabulary()
        {
            return vocabularyLoader.GetVocabulary();
        }

        public void SaveVocabulary(WordCollection vocabulary)
        {
            vocabularyLoader.SaveVocabulary(vocabulary);
        }
    }
}
