using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using LinguaLeo.Scripts.Helpers.Interfaces;

namespace LinguaLeo.Scripts.Helpers.ResourceLoading.XmlImplementation
{
    /// <summary>
    /// Позволяет работать со словорем в XML реализации
    /// </summary>
    public class VocabularyFromXml : IVocabularyManager
    {
      //  private readonly string folderXml = @"Data/Base";
      //  private readonly string fileNameXml = "WordBase.xml";
        private readonly string path;

        public VocabularyFromXml(string path) { this.path = path; }
        
        public WordCollection Load()
        {
           return  (WordCollection)LoadFromXml<WordCollectionXml>(path);
        }

        public void Save(WordCollection vocabulary)
        {
            SaveToXml(path, (WordCollectionXml)vocabulary);
        }
        
        private void SaveToXml<T>(string path,  T vocabulary) where T: class
        { 
            if (!File.Exists(path))
                throw new FileNotFoundException("path: " + path);

            using (TextWriter stream = new StreamWriter(path, false, Encoding.UTF8))
            {

                //Now save game data
                var xmlSerializer = new XmlSerializer(typeof(T));

                xmlSerializer.Serialize(stream, vocabulary);
                stream.Close();
            }
        }
        
        private  T LoadFromXml<T>(string path) where T: class
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("path: " + path);
            
            T result;

            using (TextReader stream = new StreamReader(path, Encoding.UTF8)) // (path, FileMode.Open, FileAccess.Read))
            {
                var serializer = new XmlSerializer(typeof(T));
                result = serializer.Deserialize(stream) as T;
                stream.Close();

                if (result == null)
                    throw new SerializationException("File not Deserialize");
            }

            return result;
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
