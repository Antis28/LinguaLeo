using System.Xml.Serialization;

namespace Helpers
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
