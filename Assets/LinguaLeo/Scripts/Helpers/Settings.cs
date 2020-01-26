using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

[XmlRoot("Settings")]
public class Settings
{
    [XmlIgnore]
    private static readonly string folderXml = @"Data/";
    [XmlIgnore]
    private static readonly string fileNameXml = "Settings.xml";
    [XmlIgnore]
    private static readonly string path = folderXml + fileNameXml;
    [XmlIgnore]
    private static Settings instance;

    /// <summary>
    /// Название последней выбранной группы до выхода из игры.
    /// </summary>
    public string lastWordGroup = "";

    public static Settings Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Settings();
            }
            return instance;

        }
    }

    private Settings() { }

    public static void SaveToXml()
    {        
        using (TextWriter stream = new StreamWriter(path, false, Encoding.UTF8))
        {
            //Now save game data
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Settings));

            xmlSerializer.Serialize(stream, Instance);
            stream.Close();
        }
    }
    public static void LoadFromXml()
    {
        if (!File.Exists(path))
        {
            SaveToXml();
        }
        Settings result = null;
        using (TextReader Stream = new StreamReader(path, Encoding.UTF8))
        {
            XmlSerializer Serializer = new XmlSerializer(typeof(Settings));
            result = Serializer.Deserialize(Stream) as Settings;
            Stream.Close();
        }

        instance = result;
    }

}
