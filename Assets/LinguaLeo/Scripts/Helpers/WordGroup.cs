using System.Xml.Serialization;

namespace LinguaLeo.Scripts.Helpers
{
    public struct WordGroup
    {
        [XmlAttribute]
        public string name;

        [XmlAttribute]
        public int wordCount;

        public string pictureName;
    }
}
