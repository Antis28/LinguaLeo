using System.Xml.Serialization;

namespace LinguaLeo.Scripts.Helpers.ResourceLoading.XmlImplementation
{
    /// <summary>
    /// Mapping Class
    /// см. класс ExportWordLeo
    /// в проекте Обучение/CSVReader
    /// </summary>
    public class WordLeoXml
    {
        #region Public variables

        [XmlAttribute]
        public string wordValue;

        [XmlAttribute]
        public string translations;

        [XmlAttribute]
        public string pictureUrl;

        [XmlAttribute]
        public string transcription;

        [XmlAttribute]
        public string highlightedContext;

        [XmlAttribute]
        public string soundUrl;

        [XmlAttribute]
        public string closeFieldContext;

        [XmlArray]
        [XmlArrayItem("group")]
        public string[] groups;

        [XmlElement]
        public WorkoutProgress progress;

        #endregion
    }
}
