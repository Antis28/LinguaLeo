using System;
using System.Collections;
using System.IO;
using JetBrains.Annotations;
using LinguaLeo._Adapters;
using UnityEngine;
using UnityEngine.Networking;

namespace LinguaLeo.Scripts.Managers.Parts
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioPlayer : MonoBehaviour
    {
        #region SerializeFields

        [SerializeField]
        private AudioClip sayClip;

        [SerializeField]
        private AudioSource music;

        #endregion

        #region Public variables

        private AudioSource Music
        {
            get
            {
                if (music == null)
                {
                    music = gameObject.GetComponent<AudioSource>();
                    music.loop = false;
                }

                return music;
            }
        }

        #endregion

        #region Private variables

        private AssetBundle voiceBundle;

        private readonly string
            bundleFolder =
                @"M:\My_projects\!_Unity\LinguaLeo\Assets\AssetBundles\"; //@"M:\My_projects\!_Unity\LinguaLeo\Data\Audio\";//"/Data/Audio";

        private readonly string bundleName = "voices";

        private string lastPath = null;

        #endregion

        #region Public Methods

        public IEnumerator LoadMusicFromFile()
        {
            
            if (!File.Exists(lastPath))
                throw new FileNotFoundException();
           
            
            using (UnityWebRequest www =
                UnityWebRequestMultimedia.GetAudioClip("file://" + lastPath, AudioType.OGGVORBIS))
            {
                yield return www.SendWebRequest();

                if (www.isNetworkError) { Debug.Log(www.error); } else
                {
                    sayClip = DownloadHandlerAudioClip.GetContent(www);
                }
            }
        }
        

        public void SayWord()
        {
           // if (sayClip == null) { StartCoroutine(LoadMusicFromFile()); }
            StartCoroutine(WaitLoadingAudio());
        }

        public void SetSound([NotNull] string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                lastPath = string.Empty;
                sayClip = null;
                return;
            }

            lastPath = fileName;
            sayClip = null;
               
            //sayClip = Resources.Load<AudioClip>(folder + "/" + fileName);
            //sayClip = ExtractFromBundle();
          
        }

        #endregion

        #region Private Methods

        private void ExtractFromBundle()
        {
            string fileName = "Assets/Resources/81-631152000.mp3";
            if (voiceBundle.Contains(fileName))
                voiceBundle.LoadAsset<AudioClip>(fileName);
            else { Debug.LogError("Clip " + fileName + " not found"); }
        }

        private void LoadBundle()
        {
            //string path = Path.GetFullPath(folder + "/" + fileName);
            string path = bundleFolder + bundleName;
            Debug.Log(path);

            voiceBundle = AssetBundleAdapt.LoadFromFile(path);

            foreach (var item in voiceBundle.GetAllAssetNames()) { print(item); }


            if (!File.Exists(path))
            {
                Debug.LogError("File not found");
                Debug.LogError(path);
                return;
            }

            path = bundleFolder + bundleName;
        }

        private IEnumerator WaitLoadingAudio()
        {
           var  task =  GameManager.ResourcesLoader.GetAudioClip(lastPath);
            while (!task.IsCompleted)
            {
                yield return null;
            }
            sayClip = task.Result;
            Music.PlayOneShot(sayClip);
        }

        #endregion
    }
}
