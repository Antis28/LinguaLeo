using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace LinguaLeo.Scripts.Helpers
{
    [XmlRoot("Settings")]
    public class Settings
    {
        #region Static Fields and Constants

        [XmlIgnore]
        private static readonly string FolderXml = @"Data/";

        [XmlIgnore]
        private static readonly string FileNameXml = "Settings.xml";

        [XmlIgnore]
        private static readonly string Path = FolderXml + FileNameXml;

        [XmlIgnore]
        private static Settings instance;

        #endregion

        #region Public variables

        /// <summary>
        /// Название последней выбранной группы до выхода из игры.
        /// </summary>
        public string lastWordGroup = "";

        public static Settings Instance
        {
            get
            {
                if (instance == null) { instance = new Settings(); }

                return instance;
            }
        }

        #endregion

        #region Public Methods

        public static void LoadFromXml()
        {
            if (!File.Exists(Path)) { SaveToXml(); }

            Settings result = null;
            using (TextReader stream = new StreamReader(Path, Encoding.UTF8))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                result = serializer.Deserialize(stream) as Settings;
                stream.Close();
            }

            instance = result;
        }

        public static void SaveToXml()
        {
            using (TextWriter stream = new StreamWriter(Path, false, Encoding.UTF8))
            {
                //Now save game data
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Settings));

                xmlSerializer.Serialize(stream, Instance);
                stream.Close();
            }
        }

        #endregion

        private Settings() { }
    }
}
