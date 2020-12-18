using System;
using System.IO;
using UnityEngine;

namespace LinguaLeo.Scripts.Helpers.ResourceLoading
{
    public class SpriteLoader
    {
        #region Private variables

        private readonly string pictureDirectory;
        private readonly string coverDirectory;

        private readonly string pictureDirectoryName = "Picture";
        private readonly string coversDirectoryName = "Covers";
        private readonly string fileExtension = ".png";

        #endregion

        #region Public Methods

        public Sprite GetSpriteFromCovers(string fileName)
        {
            var fullPath = Path.Combine(coverDirectory,  fileName + fileExtension);
            var sprite = LoadSpriteFromFile(fullPath);
            return sprite;
        }

        public Sprite GetSpriteFromPicture(string fileName)
        {
            var fullPath = Path.Combine(pictureDirectory, fileName + fileExtension);
            var sprite = LoadSpriteFromFile(fullPath);
            return sprite;
        }

        #endregion

        #region Private Methods

        private Sprite LoadSpriteFromFile(string path)
        {
            if (!File.Exists(path))
            {
                Debug.LogWarning("File not found\n" + path);
                path = "Data/Picture" + "/" + "image-not-found.png";
                if (!File.Exists(path))
                    throw new FileLoadException(path);
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

        #endregion

        public SpriteLoader(string pathToRootResources)
        {
            pictureDirectory = Path.Combine(pathToRootResources, pictureDirectoryName);
            coverDirectory = Path.Combine(pathToRootResources, coversDirectoryName);
        }
    }
}
