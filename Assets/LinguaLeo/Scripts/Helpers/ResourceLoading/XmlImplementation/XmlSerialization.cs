﻿using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace LinguaLeo.Scripts.Helpers.ResourceLoading.XmlImplementation
{
    /// <summary>
    /// Сохраняет и загружает Xml файл сеапилизацией
    /// </summary>
    /// <typeparam name="T">Класс для сеапилизации</typeparam>
    public class XmlSerialization<T> where T: class
    {
        public void Save(string path,  T vocabulary)
        { 
            if (!File.Exists(path))
                throw new FileNotFoundException("path: " + path);

            using (TextWriter stream = new StreamWriter(path, false, Encoding.UTF8))
            {

                //Now save game data
                var xmlSerializer = new XmlSerializer(typeof(T));

                xmlSerializer.Serialize(stream, vocabulary);
                stream.Close();
            }
        }
        
        public  T Load(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("path: " + path);
            
            T result;

            using (TextReader stream = new StreamReader(path, Encoding.UTF8)) // (path, FileMode.Open, FileAccess.Read))
            {
                var serializer = new XmlSerializer(typeof(T));
                result = serializer.Deserialize(stream) as T;
                stream.Close();

                if (result == null)
                    throw new SerializationException("File not Deserialize");
            }

            return result;
        }
    }
}