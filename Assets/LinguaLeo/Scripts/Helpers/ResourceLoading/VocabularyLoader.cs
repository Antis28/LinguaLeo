using System.IO;
using LinguaLeo.Scripts.Helpers.Interfaces;
using LinguaLeo.Scripts.Helpers.ResourceLoading.XmlImplementation;

namespace LinguaLeo.Scripts.Helpers.ResourceLoading
{
    public class VocabularyLoader
    {
        private readonly string vocalubaryDirectory = "Base";
        private readonly string vocalubaryFile = "WordBase.xml";

        private readonly string vocabularyDirectory;
        private IVocabularyManager vocabularyManager;

        public VocabularyLoader(string pathToRootResources)
        {
            this.vocabularyDirectory = Path.Combine(pathToRootResources, vocalubaryDirectory, vocalubaryFile);
        }

        public WordCollection GetVocabulary()
        {
            return LoadVocabularyFromXml(vocabularyDirectory);
        }
        
        public void SaveVocabulary(WordCollection vocabulary)
        {
            vocabularyManager.Save(vocabulary);
        }

        private WordCollection LoadVocabularyFromXml(string path)
        {
            vocabularyManager = new VocabularyFromXml(path);
            return vocabularyManager.Load();
        }
    }
}
