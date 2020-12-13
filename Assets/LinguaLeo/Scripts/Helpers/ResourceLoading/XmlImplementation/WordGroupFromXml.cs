using System.Collections.Generic;
using System.IO;
using LinguaLeo.Scripts.Helpers.Interfaces;

namespace LinguaLeo.Scripts.Helpers.ResourceLoading.XmlImplementation
{
    public class WordGroupFromXml: IVocabularyManager<List<WordGroup>>
    {
        private readonly string wordGroupFullPath;
        private readonly XmlSerialization<List<WordGroup>> xmlSerialization;

        private readonly string wordGroupDirectory = "Base";
        private readonly string wordGroupFile = "WordGroup.xml";
        
        public WordGroupFromXml(string path)
        {
            xmlSerialization = new XmlSerialization<List<WordGroup>>();
            wordGroupFullPath = Path.Combine(path, wordGroupDirectory, wordGroupFile);
        }
        
        public List<WordGroup> Load()
        {
            return xmlSerialization.Load(wordGroupFullPath);
        }

        public void Save(List<WordGroup> saveObject)
        {
            xmlSerialization.Save(wordGroupFullPath, saveObject);
        }
    }
}
