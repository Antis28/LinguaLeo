using System.Collections.Generic;
using System.IO;
using Helpers.Interfaces;

namespace Helpers.ResourceLoading.XmlImplementation
{
    public class WordGroupFromXml : IVocabularyManager<List<WordGroup>>
    {
        #region Private variables

        private readonly string wordGroupFullPath;
        private readonly XmlSerialization<List<WordGroup>> xmlSerialization;

        private readonly string wordGroupDirectory = "Base";
        private readonly string wordGroupFile = "WordGroup.xml";

        #endregion

        #region Public Methods

        public List<WordGroup> Load()
        {
            return xmlSerialization.Load(wordGroupFullPath);
        }

        public void Save(List<WordGroup> saveObject)
        {
            xmlSerialization.Save(wordGroupFullPath, saveObject);
        }

        #endregion

        public WordGroupFromXml(string path)
        {
            xmlSerialization = new XmlSerialization<List<WordGroup>>();
            wordGroupFullPath = Path.Combine(path, wordGroupDirectory, wordGroupFile);
        }
    }
}
