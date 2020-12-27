using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using LinguaLeo.Scripts.Helpers.Interfaces;
using UnityEngine;
using UnityEngine.Networking;

namespace LinguaLeo.Scripts.Helpers.ResourceLoading.ResourceLoaderImplements
{
    public sealed class AssetsBundleLoader : IResourcesLoader
    {
        private readonly string assetBundlePictureName = "pictures_";
        private readonly string assetBundleAudioName = "audio_";

        private string folderPath;

        private List<AssetBundle> audioAssetBundles;
        private List<AssetBundle> pictureAssetBundles;

        public event Action NotifyLoadingCompleted;
        public event Action<float> NotifyLoadingProgress;


        public AssetsBundleLoader()
        {
            folderPath = Application.persistentDataPath;
            audioAssetBundles = new List<AssetBundle>(8);
            pictureAssetBundles = new List<AssetBundle>(8);
            
            LoadAllBundles();
        }

        private async void LoadAsync()
        {
           
        }

        private async void LoadAllBundles()
        {
            var allBundles = 8;
            for (var counter = 1; counter < allBundles; counter++)
            {
                var progress = (counter * 1f / allBundles) / 0.8f;
                OnNotifyLoadingProgress(progress);
                var pictureBundle = await LoadBundle(assetBundlePictureName + counter);
                pictureAssetBundles.Add(pictureBundle);

                //   var audioBundle = await LoadBundle(assetBundleAudioName + counter);
                //   audioAssetBundles.Add(audioBundle);
            }

            OnLoadingCompleted();
          
        }

        private async Task<AssetBundle> LoadBundle(string assetBundleName)
        {
            var uri = Path.Combine("file:///" + folderPath, assetBundleName);

            if (File.Exists(uri)) { throw new FileNotFoundException(); }

            var request = UnityWebRequestAssetBundle.GetAssetBundle(uri, 0);
            var asyncOperation = request.SendWebRequest();

            while (!asyncOperation.isDone)
            { 
                await  Task.Yield();
                var progress = asyncOperation.progress / 0.9f;
            }

            return DownloadHandlerAssetBundle.GetContent(request);
        }

        public Task<AudioClip> GetAudioClip(string fileName)
        {
            throw new System.NotImplementedException();
        }

        public Sprite GetCover(string fileName)
        {
            throw new System.NotImplementedException();
        }

        public Sprite GetPicture(string fileName)
        {
            if (pictureAssetBundles.Count == 0) { throw new FileLoadException("Bundles Not Loaded!!!!"); }

            return pictureAssetBundles[0].LoadAsset<Sprite>(fileName);
        }

        public WordCollection LoadVocabulary()
        {
            throw new System.NotImplementedException();
        }

        public List<WordGroup> LoadWordGroup()
        {
            throw new System.NotImplementedException();
        }

        public void SaveVocabulary(WordCollection vocabulary)
        {
            throw new System.NotImplementedException();
        }

        public void SaveWordGroup(List<WordGroup> groups)
        {
            throw new System.NotImplementedException();
        }

        private void OnLoadingCompleted()
        {
            NotifyLoadingCompleted?.Invoke();
        }

        private void OnNotifyLoadingProgress(float obj)
        {
            NotifyLoadingProgress?.Invoke(obj);
        }
    }
}
