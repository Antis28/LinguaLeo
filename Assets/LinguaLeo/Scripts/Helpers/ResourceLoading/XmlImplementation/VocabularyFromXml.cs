
using LinguaLeo.Scripts.Helpers.Interfaces;

namespace LinguaLeo.Scripts.Helpers.ResourceLoading.XmlImplementation
{
    /// <summary>
    /// Позволяет работать со словорем в XML реализации
    /// </summary>
    public class VocabularyFromXml : IVocabularyManager<WordCollection>
    {
      //  private readonly string folderXml = @"Data/Base";
      //  private readonly string fileNameXml = "WordBase.xml";
        private readonly string path;
        private readonly XmlSerialization<WordCollectionXml> xmlSerialization;

        public VocabularyFromXml(string path)
        {
            this.path = path;
            xmlSerialization = new XmlSerialization<WordCollectionXml>();
        }
        
        public WordCollection Load()
        {
           return  (WordCollection)xmlSerialization.Load(path);
        }

        public void Save(WordCollection saveObject)
        {
            xmlSerialization.Save(path, (WordCollectionXml)saveObject);
        }
        
       
        
        /*
        
        /// <summary>
        /// получить описание наборов слов
        /// </summary>
        /// <returns>описание наборов слов</returns>
        public List<WordGroup> GetGroupNames()
        {
            if (groupNames != null)
                return groupNames;

            string path = folderXml + "/" + "WordGroup.xml";
            if (!File.Exists(path))
            {
                Debug.LogError("File not found. Path: " + path);
                return null;
            }
            groupNames = DeserializeGroup(path);
            //SerializeGroup(GroupNames, path);
            return groupNames;
        }
*/
    }
}
