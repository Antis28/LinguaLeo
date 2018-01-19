using System.Xml.Serialization;

/// <summary>
/// Mapping Class
/// </summary>
public class WordLeo
{
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
    public string groups;
    [XmlAttribute]
    public string clozefiedContext;

    override
    public string ToString()
    {
        return wordValue;
    }
}