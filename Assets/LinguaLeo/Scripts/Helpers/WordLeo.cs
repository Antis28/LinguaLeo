﻿using System;
using System.Xml.Serialization;

/// <summary>
/// Mapping Class
/// </summary>
public class WordLeo
{
    [XmlAttribute]
    public string engWord;
    [XmlAttribute]
    public string ruWord;
    [XmlAttribute]
    public string imageURL;
    [XmlAttribute]
    public string transcript;
    [XmlAttribute]
    public string context;
    [XmlAttribute]
    public string audioURL;
    [XmlAttribute]
    public string addedDate;
    [XmlAttribute]
    public string context2;

    [XmlIgnore]
    public bool isTrue;

    override
    public string ToString()
    {
        return engWord;
    }
}