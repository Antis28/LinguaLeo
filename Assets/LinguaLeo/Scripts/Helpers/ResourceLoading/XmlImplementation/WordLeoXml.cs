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

        public static explicit operator WordLeo(WordLeoXml coll)
        {
            WordLeo word = new WordLeo();

            word.groups = coll.groups;
            word.progress = coll.progress;
            word.transcription = coll.transcription;
            word.translations = coll.translations;
            word.clozefiedContext = coll.closeFieldContext;
            word.highlightedContext = coll.highlightedContext;
            word.pictureUrl = coll.pictureUrl;
            word.soundUrl = coll.soundUrl;
            word.wordValue = coll.wordValue;

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
                pictureUrl = coll.pictureUrl,
                soundUrl = coll.soundUrl,
                wordValue = coll.wordValue
            };
        }
    }
}
