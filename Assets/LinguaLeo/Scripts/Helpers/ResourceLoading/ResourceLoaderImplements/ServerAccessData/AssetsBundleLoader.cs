// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using LinguaLeo.Scripts.Helpers.Interfaces;
using LinguaLeo.Scripts.Helpers.ResourceLoading.XmlImplementation;
using UnityEngine;
using UnityEngine.Networking;

#endregion

namespace LinguaLeo.Scripts.Helpers.ResourceLoading.ResourceLoaderImplements
{
    public sealed class AssetsBundleLoader
    {
        #region Public variables

        public event Action<float> NotifyBundleProgress;

        public event Action NotifyLoadingCompleted;
        public event Action<string> NotifyLoadingFall;
        public event Action<float> NotifyLoadingProgress;

        public event Action<string> NotifyProgress;

        #endregion

        #region Private variables

        private readonly string assetBundleAudioName = "audio_";

        private readonly string assetBundlePictureName = "pictures_";
        private readonly string assetBundleVocabularyName = "vocabulary";

        private readonly List<AssetBundle> audioAssetBundles;

        private readonly int countAllBundles;
        private int countLoadingBundles = 0;


        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private string folderPath;
        private readonly List<AssetBundle> pictureAssetBundles;

        private readonly int quantityAudioBundles = 3;
        private readonly int quantityPictureBundles = 8;
        private AssetBundle vocabularyAssetBundle;

        #endregion

        #region Public Methods

        public AssetsBundleLoader()
        {
            OnNotifyProgress("In AssetsBundleLoader");
            folderPath = Application.persistentDataPath;
            audioAssetBundles = new List<AssetBundle>(quantityAudioBundles);
            pictureAssetBundles = new List<AssetBundle>(quantityPictureBundles);

            // из-за того, что счетчик списка начинается с нуля, а списка 2, то и коррекция -2. 
            var correctionQuantity = -2;

            countAllBundles = quantityAudioBundles + quantityPictureBundles + correctionQuantity;
        }

        public Task<AudioClip> GetAudioClip(string fileName)
        {
            throw new NotImplementedException();
        }

        public Sprite GetCover(string fileName)
        {
            throw new NotImplementedException();
        }

        public Sprite GetPicture(string fileName)
        {
            if (pictureAssetBundles.Count == 0) { throw new FileLoadException("Bundles Not Loaded!!!!"); }

            var bundle = SelectPictureBundleByName(Path.GetFileNameWithoutExtension(fileName));
            
            return bundle.LoadAsset<Sprite>(fileName);
        }


        public async void LoadAllBundlesAsync()
        {
            try
            {
                await LoadPictureBundles();
             //   await LoadAudioBundles();

                OnLoadingCompleted();
            } catch (Exception e)
            {
                OnNotifyLoadingFall("Загрузка пакетов не удалась!");
                Console.WriteLine(e);
            }
        }

        public WordCollection LoadVocabulary()
        {
            var wordCollection = new WordCollection();


            var xmlFile = vocabularyAssetBundle.LoadAsset<TextAsset>("WordBase");
            var xmlSerialization = new XmlSerialization<WordCollectionXml>();
            var t = xmlSerialization.LoadFromString(xmlFile.text);

            return (WordCollection) t;
        }

        public List<WordGroup> LoadWordGroup()
        {
            throw new NotImplementedException();
        }

        public void SaveVocabulary(WordCollection vocabulary)
        {
            throw new NotImplementedException();
        }

        public void SaveWordGroup(List<WordGroup> groups)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private Methods

        private int GetProgress()
        {
            countLoadingBundles++;
            var t = (int) (countLoadingBundles * 1f / countAllBundles * 100);
            return t;
        }

        private async Task LoadAudioBundles()
        {
            for (var counter = 1; counter < quantityAudioBundles; counter++)
            {
                audioAssetBundles.Add(await LoadBundleAsync(assetBundleAudioName + counter));
            }
            OnNotifyProgress("In LoadAudioBundles");
        }

        private async Task<AssetBundle> LoadBundleAsync(string assetBundleName)
        {
            var uri = Path.Combine("file:///" + folderPath, assetBundleName);

            var request = UnityWebRequestAssetBundle.GetAssetBundle(uri, 0);
            var asyncOperation = request.SendWebRequest();

            while (!asyncOperation.isDone)
            {
                var progress = asyncOperation.progress / 0.9f;
                OnNotifyBundleProgress(progress);
              //  OnNotifyProgress($"Bundle {assetBundleName} - progress = {progress}");
                await Task.Yield();
            }

            OnNotifyBundleProgress(1);
            if (request.isHttpError || request.isNetworkError) { throw new FileLoadException(request.error); }

            OnNotifyLoadingProgress(GetProgress());

            return DownloadHandlerAssetBundle.GetContent(request);
        }


        private async Task LoadPictureBundles()
        {
            for (var counter = 1; counter < quantityPictureBundles; counter++)
            {
                pictureAssetBundles.Add(await LoadBundleAsync(assetBundlePictureName + counter));
            }
            OnNotifyProgress("In LoadPictureBundles");
        }

        private async Task LoadVocabularyBundles()
        {
            // vocabularyAssetBundle = await LoadBundleAsync(assetBundleVocabularyName);
        }

        private void OnLoadingCompleted()
        {
            NotifyLoadingCompleted?.Invoke();
        }

        private void OnNotifyBundleProgress(float obj)
        {
            NotifyBundleProgress?.Invoke(obj);
        }

        private void OnNotifyLoadingFall(string obj)
        {
            NotifyLoadingFall?.Invoke(obj);
        }

        private void OnNotifyLoadingProgress(float obj)
        {
            NotifyLoadingProgress?.Invoke(obj);
        }

        private void OnNotifyProgress(string obj)
        {
            NotifyProgress?.Invoke(obj);
        }

        private AssetBundle SelectAudioBundleByName(string name)
        {
            int.TryParse(name, out var number);

            AssetBundle bundle = null;
/*
            if (number >= 67631152000 && number <= 35661631152008) { bundle = pictureAssetBundles[0];}
            else if (number >= 35670631152008 && number <= 91373338631170000) { bundle = pictureAssetBundles[1]; }
*/
            return bundle;
        }


        private AssetBundle SelectPictureBundleByName(string name)
        {
            int.TryParse(name, out var number);
            AssetBundle bundle = null;

            if (number >= 533 && number <= 1996049) { bundle = pictureAssetBundles[0]; }
            else if (number >= 1997605 && number <= 3221199) { bundle = pictureAssetBundles[1]; }
            else if (number >= 3221200 && number <= 3237929) { bundle = pictureAssetBundles[2]; }
            else if (number >= 3237930 && number <= 3245633) { bundle = pictureAssetBundles[3]; }
            else if (number >= 3245637 && number <= 3331632) { bundle = pictureAssetBundles[4]; }
            else if (number >= 3331970 && number <= 3610003) { bundle = pictureAssetBundles[5]; }
            else if (number >= 3610055 && number <= 3770132) { bundle = pictureAssetBundles[6]; }
            else if (number >= 3610055 && number <= 3770132) { bundle = pictureAssetBundles[7]; }

            return bundle;
        }

        #endregion
    }
}
