using System;
using System.IO;
using UnityEngine;

namespace LinguaLeo.Scripts.Helpers.ResourceLoading
{
    public static class SpriteLoader
    {
        private static readonly string pictureDirectory = Path.Combine(ResourcesLoader.PathToRootResources, "Picture");
        private static string coverDirectory = Path.Combine(ResourcesLoader.PathToRootResources, "Covers");


        public static Sprite GetSpriteFromPicture(string fileName)
        {
            var normalisedName = ResourcesLoader.ConverterUrlToName(fileName);
            var fullPath = Path.Combine(pictureDirectory, normalisedName);
            var sprite = LoadSpriteFromFile(fullPath);
            return sprite;
        }

        public static Sprite GetSpriteFromCovers(string fileName) { throw new NotImplementedException(); }

        private static Sprite LoadSpriteFromFile(string path)
        {
            //string path = "Data/Covers" + "/" + pictureName + ".png";
            if (!File.Exists(path))
            {
                Debug.LogWarning("File not found\n" + path);
                path = "Data/Picture" + "/" + "image-not-found.png";
                if (!File.Exists(path))
                    return null;
            }

            byte[] picture;

            using (var stream = new FileStream(path, FileMode.Open))
            {
                picture = new byte[stream.Length];
                // считываем данные
                stream.Read(picture, 0, picture.Length);
            }

            var texture2D = new Texture2D(1, 1);
            texture2D.LoadImage(picture);

            var sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height),
                                       new Vector2(.5f, .5f));
            return sprite;
        }
    }
}
