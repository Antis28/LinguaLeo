#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Helpers.Interfaces;
using Helpers.ResourceLoading.XmlImplementation;
using JetBrains.Annotations;
using UnityEngine;

#endregion

namespace Helpers.ResourceLoading.ResourceLoaderImplements.ForwardAccessData.Parts
{
    /// <summary>
    /// Базовый загрузчик не имеющий пути.
    /// От него наследуются остальные загрузчики.
    /// DataLoader, StreamingAssetsLoader, ResourcesLoader
    /// </summary>
    public abstract class AbstractLoader : IResourcesLoader
    {
        #region Public variables

        public event Action NotifyLoadingCompleted;
        public event Action<float> NotifyLoadingProgress;

        #endregion

        #region Private variables

        protected string pathToRootResources = string.Empty;
        private SpriteLoader spriteLoader;
        private AudioLoader audioLoader;
        private VocabularyLoader vocabularyLoader;
        private WordGroupFromXml wordGroupFromXml;

        #endregion

        #region Public Methods

        public Task<AudioClip> GetAudioClip(string fileName)
        {
            return audioLoader.GetAudioClip(fileName);
        }

        public Sprite GetCover(string fileName)
        {
            return spriteLoader.GetSpriteFromCovers(fileName);
        }

        public Sprite GetPicture(string fileName)
        {
            var normalizeName = ConverterUrlToName(fileName, false);
            return spriteLoader.GetSpriteFromPicture(normalizeName);
        }

        public WordCollection LoadVocabulary()
        {
            return vocabularyLoader.GetVocabulary();
        }

        public List<WordGroup> LoadWordGroup()
        {
            return wordGroupFromXml.Load();
        }

        public void SaveVocabulary(WordCollection vocabulary)
        {
            vocabularyLoader.SaveVocabulary(vocabulary);
        }

        public void SaveWordGroup(List<WordGroup> groups)
        {
            wordGroupFromXml.Save(groups);
        }

        #endregion

        #region Private Methods

        private string ConverterUrlToName([NotNull] string url, bool withExtension = true)
        {
            if (url == null) throw new ArgumentNullException(nameof(url));

            //string url = "http://contentcdn.lingualeo.com/uploads/picture/3466359.png";
            //string url = "http://contentcdn.lingualeo.com/uploads/picture/96-631152008.mp3";
            const string patern = @"(\d+.png$)|(\d+-\d+.mp3$)";
            var rg = new Regex(patern, RegexOptions.IgnoreCase);
            var mat = rg.Match(url);

            if (withExtension)
                return Path.GetFileName(mat.Value);

            return Path.GetFileNameWithoutExtension(mat.Value);
        }


        /// <summary>
        /// Вызывать ТОЛЬКО после установки пути к ресурсам(pathToRootResources)
        /// </summary>
        protected void InitAllLoaders()
        {
            spriteLoader = new SpriteLoader(pathToRootResources);
            audioLoader = new AudioLoader(pathToRootResources);
            vocabularyLoader = new VocabularyLoader(pathToRootResources);
            wordGroupFromXml = new WordGroupFromXml(pathToRootResources);
        }

        #endregion
    }
}
