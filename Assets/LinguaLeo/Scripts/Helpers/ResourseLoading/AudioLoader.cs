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
        private readonly string audioDirectory;
        private readonly string resExt = ".ogg";

        public UnityEvent LoadingComplete;

        public AudioLoader(string PathToRootResources)
        {
            this.audioDirectory = Path.Combine(PathToRootResources, "Audio", "OGG");
        }

        public AudioClip GetAudioClip(string fileName)
        {
            var fullPath = Path.Combine(audioDirectory, fileName + resExt);
            return LoadAudioFromFile(fullPath);
        }

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
    }
}
