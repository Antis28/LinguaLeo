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
        public List<WordLeo> allWords; // полный словарь

        #endregion

        #region Public Methods

        public static explicit operator WordCollection(WordCollectionXml param)
        {
            return new WordCollection {allWords = param.allWords};
        }

        public static explicit operator WordCollectionXml(WordCollection param)
        {
            return new WordCollectionXml {allWords = param.allWords};
        }

        #endregion
    }
}
