using System.Xml.Serialization;

namespace Helpers.ResourceLoading.XmlImplementation
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
        public string pictureURL;

        [XmlAttribute]
        public string transcription;

        [XmlAttribute]
        public string highlightedContext;

        [XmlAttribute]
        public string soundURL;

        [XmlAttribute]
        public string closeFieldContext;

        [XmlArray]
        [XmlArrayItem("group")]
        public string[] groups;

        [XmlElement]
        public WorkoutProgress progress;

        #endregion

        public static explicit operator WordLeo(WordLeoXml coll)
        {
            WordLeo word = new WordLeo();

            word.groups = coll.groups;
            word.progress = coll.progress;
            word.transcription = coll.transcription ?? string.Empty;
            word.translations = coll.translations ?? string.Empty;
            word.clozefiedContext = coll.closeFieldContext ?? string.Empty;
            word.highlightedContext = coll.highlightedContext ?? string.Empty;
            word.pictureUrl = coll.pictureURL ?? string.Empty;
            word.soundUrl = coll.soundURL ?? string.Empty;
            word.wordValue = coll.wordValue ?? string.Empty;

            return word;
        }

        public static explicit operator WordLeoXml(WordLeo coll)
        {
            return new WordLeoXml
            {
                groups = coll.groups,
                progress = coll.progress,
                transcription = coll.transcription,
                translations = coll.translations,
                closeFieldContext = coll.clozefiedContext,
                highlightedContext = coll.highlightedContext,
                pictureURL = coll.pictureUrl,
                soundURL = coll.soundUrl,
                wordValue = coll.wordValue
            };
        }
    }
}
