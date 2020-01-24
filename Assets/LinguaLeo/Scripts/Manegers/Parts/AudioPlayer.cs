using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioClip sayClip;
    [SerializeField]
    private AudioSource music;

    AssetBundle voiceBundle;

    private readonly string bundleFolder = @"M:\My_projects\!_Unity\LinguaLeo\Assets\AssetBundles\";//@"M:\My_projects\!_Unity\LinguaLeo\Data\Audio\";//"/Data/Audio";
    private readonly string bundleName = "voices";

    private readonly string resFolder = @"M:\My_projects\!_Unity\LinguaLeo\Data\Audio\OGG\";
    private readonly string resExt = ".ogg";

    private string lastPath = null;

    public AudioSource Music
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

    public void SetSound(string fileName)
    {
        //sayClip = Resources.Load<AudioClip>(folder + "/" + fileName);
        //sayClip = ExtractFromBundle();
        lastPath = resFolder + fileName + resExt;
        StartCoroutine(LoadMusicFromFile());
    }
    public void SayWord()
    {
        if (sayClip == null)
        {
            StartCoroutine(LoadMusicFromFile());
        }
        StartCoroutine(WaitLoadingAudio());
    }

    public IEnumerator LoadMusicFromFile()
    {
        if (!File.Exists(lastPath))
            throw new FileNotFoundException();
        sayClip = null;
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + lastPath, AudioType.OGGVORBIS))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                sayClip = DownloadHandlerAudioClip.GetContent(www);
            }
        }
    }

    IEnumerator WaitLoadingAudio()
    {
        while (sayClip == null)
        {
            yield return null;
        }
        Music.PlayOneShot(sayClip);
    }

    private void LoadBundle()
    {
        //string path = Path.GetFullPath(folder + "/" + fileName);
        string path = bundleFolder + bundleName;
        Debug.Log(path);

        voiceBundle = AssetBundleAdapt.LoadFromFile(path);

        foreach (var item in voiceBundle.GetAllAssetNames())
        {
            print(item);
        }


        if (!File.Exists(path))
        {
            Debug.LogError("File not found");
            Debug.LogError(path);
            return;
        }
        path = bundleFolder + bundleName;
    }

    void ExtractFromBundle()
    {
        string fileName = "Assets/Resources/81-631152000.mp3";
        if (voiceBundle.Contains(fileName))
            voiceBundle.LoadAsset<AudioClip>(fileName);
        else
        {
            Debug.LogError("Clip " + fileName + " not found");
        }
    }
}
