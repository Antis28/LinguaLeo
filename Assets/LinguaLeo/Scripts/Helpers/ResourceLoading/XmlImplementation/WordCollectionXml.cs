using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace LinguaLeo.Scripts.Helpers.ResourceLoading.XmlImplementation
{
    /// <summary>
    /// Корневой элемент Xml файла словаря.
    /// </summary>
    [XmlRoot("Root")]
    public class WordCollectionXml 
    {
        #region Public variables

        [XmlArray("LeoWords")]
        [XmlArrayItem("word")]
        public List<WordLeoXml> allWords; // полный словарь

        #endregion

        #region Public Methods

        public static explicit operator WordCollection(WordCollectionXml coll)
        {
            var collection = new WordCollection();

            foreach (var val in coll.allWords)
            {
                collection.allWords.Add((WordLeo)val);
            }

            return collection;
        }

        public static explicit operator WordCollectionXml(WordCollection coll)
        {
            var collection = new WordCollectionXml();

            foreach (var val in coll.allWords)
            {
                collection.allWords.Add((WordLeoXml)val);
            }

            return collection;
        }

        #endregion

    
    }
}
