#region

using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

#endregion

namespace LinguaLeo.Scripts.Helpers.ResourceLoading
{
    public class AudioLoader
    {
        #region Public variables

        public UnityEvent LoadingComplete;

        #endregion

        #region Private variables

        private readonly string audioDirectory;
        private string fullPath;
        private readonly string resExt = ".ogg";

        private readonly string resFolder = @"M:\My_projects\!_Unity\LinguaLeo\Data\Audio\OGG\";

        #endregion

        #region Public Methods

        public Task<AudioClip> GetAudioClip(string fileName)
        {
            fullPath = Path.Combine(audioDirectory, fileName + resExt);
            return LoadAudioFromFile(fullPath);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Загружает аудио Ogg файл
        /// функци медленная требуется отдельный поток(корутина).
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        private async Task<AudioClip> LoadAudioFromFile(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException();

            using (var www = UnityWebRequestMultimedia.GetAudioClip(fullPath, AudioType.OGGVORBIS))
            {
                var asyncOperation = www.SendWebRequest();

                while (!asyncOperation.isDone) { Task.Yield(); }

                if (www.isNetworkError) { Debug.Log(www.error); }

                return DownloadHandlerAudioClip.GetContent(www);
            }
        }

        #endregion

        public AudioLoader(string pathToRootResources)
        {
            audioDirectory = Path.Combine(pathToRootResources, "Audio", "OGG");
            audioDirectory = resFolder;
        }
    }
}
