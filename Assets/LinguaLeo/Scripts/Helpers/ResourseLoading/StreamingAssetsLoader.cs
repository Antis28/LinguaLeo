using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace LinguaLeo.Scripts.Helpers.ResourceLoading
{
    public class StreamingAssetsLoader : IResourcesLoader
    {
        private string pathToRootResources = string.Empty;
        private SpriteLoader spriteLoader;
        private AudioLoader audioLoader;

        public StreamingAssetsLoader()
        {
            SelectPathByPlatform();
            spriteLoader = new SpriteLoader(pathToRootResources);
            audioLoader = new AudioLoader(pathToRootResources);
        }

        private void SelectPathByPlatform()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            //Android uses files inside a compressed APK/JAR file
            //To read streaming Assets on platforms like Android and WebGL
            //, where you cannot access streaming Asset files directly, use UnityWebRequest.
            //For an example, see Application.streamingAssetsPath. On many platforms, the streaming assets folder
            //location is read-only; you can not modify or write new files there at runtime.
            //Use Application.persistentDataPath for a folder location that is writable.
            pathToRootResources = "jar:file://" + Application.dataPath + "!/assets";
#endif

#if UNITY_EDITOR || UNITY_STANDALONE
            // Most platforms (Unity Editor, Windows, Linux players, PS4, Xbox One, Switch) use Application.dataPath + "/StreamingAssets"
            pathToRootResources = Path.Combine(Application.dataPath, "StreamingAssets");
#endif
            pathToRootResources = Path.Combine(pathToRootResources, "Data");
        }

        public string ConverterUrlToName(string url, bool withExtension = true)
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


        public Sprite GetPicture(string fileName)
        {
            var normalizeName = ConverterUrlToName(fileName);
            return spriteLoader.GetSpriteFromPicture(normalizeName);
        }

        public Sprite GetCover(string fileName) { return spriteLoader.GetSpriteFromCovers(fileName); }

        public AudioClip GetAudioClip(string fileName) { return audioLoader.GetAudioClip(fileName); }
    }
}
