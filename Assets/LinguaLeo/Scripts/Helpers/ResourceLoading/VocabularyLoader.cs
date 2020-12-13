using System.IO;
using LinguaLeo.Scripts.Helpers.Interfaces;
using LinguaLeo.Scripts.Helpers.ResourceLoading.XmlImplementation;

namespace LinguaLeo.Scripts.Helpers.ResourceLoading
{
    public class VocabularyLoader
    {
        private readonly string vocabularyDirectory = "Base";
        private readonly string vocabularyFile = "WordBase.xml";

        private readonly IVocabularyManager<WordCollection> vocabularyManager;

        public VocabularyLoader(string pathToRootResources)
        {
            var vocabularyFullPath = Path.Combine(pathToRootResources, vocabularyDirectory, vocabularyFile);
            vocabularyManager = new VocabularyFromXml(vocabularyFullPath);
        }

        public WordCollection GetVocabulary()
        {
            return LoadVocabularyFromXml();
        }

        public void SaveVocabulary(WordCollection vocabulary)
        {
            vocabularyManager.Save(vocabulary);
        }

        private WordCollection LoadVocabularyFromXml()
        {
            return vocabularyManager.Load();
        }
    }
}
