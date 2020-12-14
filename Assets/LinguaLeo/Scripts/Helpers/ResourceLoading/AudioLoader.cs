using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace LinguaLeo.Scripts.Helpers.ResourceLoading
{
    public class AudioLoader
    {
        #region Public variables

        public UnityEvent loadingComplete;

        #endregion

        #region Private variables

        private readonly string audioDirectory;
        private readonly string resExt = ".ogg";

        #endregion

        #region Public Methods

        public AudioClip GetAudioClip(string fileName)
        {
            var fullPath = Path.Combine(audioDirectory, fileName + resExt);
            return LoadAudioFromFile(fullPath);
        }

        #endregion

        #region Private Methods

        private AudioClip LoadAudioFromFile(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException();

            var fullPath = Path.Combine(audioDirectory, path);

            using (var www = UnityWebRequestMultimedia.GetAudioClip(fullPath, AudioType.OGGVORBIS))
            {
                www.SendWebRequest();

                if (www.isNetworkError) { Debug.Log(www.error); }

                return DownloadHandlerAudioClip.GetContent(www);
            }
        }

        #endregion

        public AudioLoader(string pathToRootResources)
        {
            audioDirectory = Path.Combine(pathToRootResources, "Audio", "OGG");
        }
    }
}
