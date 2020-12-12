using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace LinguaLeo.Scripts.Helpers.ResourceLoading.ResourceLoaderImplements
{
    /// <summary>
    /// Базовый загрузчик не имеющий пути.
    /// </summary>
    public abstract class AbstractLoader
    {
        protected string pathToRootResources = string.Empty;
        protected SpriteLoader spriteLoader;
        protected AudioLoader audioLoader;
        protected VocabularyLoader vocabularyLoader;

        public Sprite GetPicture(string fileName)
        {
            var normalizeName = ConverterUrlToName(fileName);
            return spriteLoader.GetSpriteFromPicture(normalizeName);
        }

        public Sprite GetCover(string fileName) { return spriteLoader.GetSpriteFromCovers(fileName); }

        public AudioClip GetAudioClip(string fileName) { return audioLoader.GetAudioClip(fileName); }

        /// <summary>
        /// Вызывать ТОЛЬКО после установки пути к ресурсам(pathToRootResources)
        /// </summary>
        protected void InitAllLoaders()
        {
            spriteLoader = new SpriteLoader(pathToRootResources);
            audioLoader = new AudioLoader(pathToRootResources);
            vocabularyLoader = new VocabularyLoader(pathToRootResources);
        }
        
        private string ConverterUrlToName(string url, bool withExtension = true)
        {
            //string url = "http://contentcdn.lingualeo.com/uploads/picture/3466359.png";
            //string url = "http://contentcdn.lingualeo.com/uploads/picture/96-631152008.mp3";
            const string patern = @"(\d+.png$)|(\d+-\d+.mp3$)";
            var rg = new Regex(patern, RegexOptions.IgnoreCase);
            var mat = rg.Match(url);

            if (withExtension)
                return Path.GetFileName(mat.Value);

            return Path.GetFileNameWithoutExtension(mat.Value);
        }
    }
}
