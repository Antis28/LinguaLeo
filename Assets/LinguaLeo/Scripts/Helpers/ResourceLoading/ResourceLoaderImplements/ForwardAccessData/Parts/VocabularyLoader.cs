using System.IO;
using Helpers.Interfaces;
using Helpers.ResourceLoading.XmlImplementation;

namespace Helpers.ResourceLoading.ResourceLoaderImplements.ForwardAccessData.Parts
{
    public class VocabularyLoader
    {
        #region Private variables

        private readonly string vocabularyDirectory = "Base";
        private readonly string vocabularyFile = "WordBase.xml";

        private readonly IVocabularyManager<WordCollection> vocabularyManager;

        #endregion

        #region Public Methods

        public WordCollection GetVocabulary()
        {
            return LoadVocabularyFromXml();
        }

        public void SaveVocabulary(WordCollection vocabulary)
        {
            vocabularyManager.Save(vocabulary);
        }

        #endregion

        #region Private Methods

        private WordCollection LoadVocabularyFromXml()
        {
            return vocabularyManager.Load();
        }

        #endregion

        public VocabularyLoader(string pathToRootResources)
        {
            var vocabularyFullPath = Path.Combine(pathToRootResources, vocabularyDirectory, vocabularyFile);
            vocabularyManager = new VocabularyFromXml(vocabularyFullPath);
        }
    }
}
