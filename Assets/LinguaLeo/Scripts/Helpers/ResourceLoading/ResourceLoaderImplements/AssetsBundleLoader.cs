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

        private readonly int countAudioBundles = 3;
        private readonly int countPictureBundles = 8;


        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private string folderPath;

        private readonly List<AssetBundle> audioAssetBundles;
        private readonly List<AssetBundle> pictureAssetBundles;

        public event Action NotifyLoadingCompleted;
        public event Action<float> NotifyLoadingProgress;
        
        public event Action<string> NotifyProgress;
        public event Action<float> NotifyBundleProgress;

        private readonly int countAllBundles;
        private int countLoadingBundles = 0;

        private int GetProgress()
        {
            countLoadingBundles++;
            var t = (int) (countLoadingBundles * 1f / countAllBundles * 100);
            return t;
        }

        public AssetsBundleLoader()
        {
            OnNotifyProgress("In AssetsBundleLoader");
            folderPath = Application.persistentDataPath;
            audioAssetBundles = new List<AssetBundle>(countAudioBundles);
            pictureAssetBundles = new List<AssetBundle>(countPictureBundles);

            countAllBundles = countAudioBundles + countPictureBundles - 2;
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


        private async Task LoadPictureBundles()
        {
            var tasks = new List<Task<AssetBundle>>();
            for (var counter = 1; counter < countPictureBundles; counter++)
            {
                tasks.Add(LoadBundleAsync(assetBundlePictureName + counter));
            }

            foreach (var task in tasks)
            {
                pictureAssetBundles.Add(await task);
                var progress = GetProgress();
                OnNotifyLoadingProgress(progress);
            }
        }

        private async Task LoadAudioBundles()
        {
            var tasks = new List<Task<AssetBundle>>();
            for (var counter = 1; counter < countAudioBundles; counter++)
            {
                tasks.Add(LoadBundleAsync(assetBundleAudioName + counter));
            }

            foreach (var task in tasks)
            {
                audioAssetBundles.Add(await task);
                var progress = GetProgress();
                OnNotifyLoadingProgress(progress);
            }
        }


        public async void LoadAllBundlesAsync()
        {
            OnNotifyProgress("In LoadPictureBundles");
            await LoadPictureBundles();
            OnNotifyProgress("In LoadAudioBundles");
            await LoadAudioBundles();
            OnNotifyProgress("In OnLoadingCompleted");
            OnLoadingCompleted();
        }

        private async Task<AssetBundle> LoadBundleAsync(string assetBundleName)
        {
            var uri = Path.Combine("file:///" + folderPath, assetBundleName);

            if (File.Exists(uri)) { throw new FileNotFoundException(uri); }

            var request = UnityWebRequestAssetBundle.GetAssetBundle(uri, 0);
            var asyncOperation = request.SendWebRequest();

            while (!asyncOperation.isDone)
            {
                
                var progress = asyncOperation.progress / 0.9f;
                OnNotifyBundleProgress(progress);
                await Task.Yield();
            }
            OnNotifyBundleProgress(1);
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

            var bundle = SelectPictureBundleByName(Path.GetFileNameWithoutExtension(fileName));
            return bundle.LoadAsset<Sprite>(fileName);
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

        private void OnNotifyProgress(string obj)
        {
            NotifyProgress?.Invoke(obj);
        }

        private void OnNotifyBundleProgress(float obj)
        {
            NotifyBundleProgress?.Invoke(obj);
        }
    }
}
