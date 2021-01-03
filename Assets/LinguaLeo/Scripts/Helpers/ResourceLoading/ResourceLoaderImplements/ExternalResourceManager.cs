// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using JetBrains.Annotations;
using LinguaLeo.Scripts.Helpers.Interfaces;
using LinguaLeo.Scripts.Helpers.ResourceLoading.XmlImplementation;
using UnityEngine;

#endregion

namespace LinguaLeo.Scripts.Helpers.ResourceLoading.ResourceLoaderImplements
{
    public class ExternalResourceManager : IExternalResourceManager
    {
        #region Private variables

        private string pathToRootResources = string.Empty;


        private readonly VocabularyLoader vocabularyLoader;
        private WordGroupFromXml wordGroupFromXml;

        private DataLoader dataLoader;

        #endregion

        #region Public Methods

        public ExternalResourceManager()
        {
            SelectPathByPlatform();
            vocabularyLoader = new VocabularyLoader(pathToRootResources);
            wordGroupFromXml = new WordGroupFromXml(pathToRootResources);
            dataLoader = new DataLoader();
        }

        public AudioClip GetAudioClip(string fileName)
        {

            var rrr = GetAudioClipAsync(fileName);
            throw new Exception();
        }

       
        private async Task<AudioClip> GetAudioClipAsync(string fileName)
        {
            return  await dataLoader.GetAudioClip(fileName);
        }

        public Sprite GetCover(string fileName)
        {
            throw new System.NotImplementedException();
        }

        public Sprite GetPicture(string fileName)
        {
            throw new System.NotImplementedException();
        }

        public WordCollection GetVocabulary()
        {
            return vocabularyLoader.GetVocabulary();
        }

        public List<WordGroup> GetWordGroup()
        {
            return wordGroupFromXml.Load();
        }

        public void SaveVocabulary(WordCollection vocabulary)
        {
            throw new System.NotImplementedException();
        }

        public void SaveWordGroup(List<WordGroup> groups)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region Private Methods

        private void SelectPathByPlatform()
        {
            pathToRootResources = Application.persistentDataPath;
        }

        #endregion
    }
}
