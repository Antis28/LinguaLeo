using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

public struct WordGroup
{
    [XmlAttribute]
    public string name;
    [XmlAttribute]
    public int wordCount;

    public string pictureName;
}
