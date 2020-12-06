using System.IO;
using UnityEngine;

namespace LinguaLeo.Scripts.Helpers.ResourceLoading
{
    public class SpriteLoader
    {
        private string spriteDirectory = Path.Combine(ResourcesLoader.PathToRootResources, "Picture");
        
        
        public static Sprite GetSprite(string fileName)
        {
            const string folder = "Data/Picture/";
            //Sprite sprite = Resources.Load<Sprite>(foloder + "/" + MyUtilities.ConverterUrlToName(fileName));
            var sprite = LoadSpriteFromFile(folder + ResourcesLoader.ConverterUrlToName(fileName));
            return sprite;
        }
        
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
