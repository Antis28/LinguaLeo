using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioClip sayClip;
    [SerializeField]
    private AudioSource music;

    AssetBundle voiceBundle;

    private string bundleFolder = @"M:\My_projects\!_Unity\LinguaLeo\Assets\AssetBundles\";//@"M:\My_projects\!_Unity\LinguaLeo\Data\Audio\";//"/Data/Audio";
    private string bundleName = "voices";

    private string resFolder = @"M:\My_projects\!_Unity\LinguaLeo\Data\Audio\OGG\";
    private string resExt = ".ogg";

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
        string path = resFolder + fileName + resExt;
        sayClip = Utilities.LoadMusicFromFile(path);
    }
    public void SayWord()
    {
        if (sayClip == null)
        {
            Debug.Log("Clip not loaded");
            return;
        }
        StartCoroutine(WaitLoadingAudio());
    }

    IEnumerator WaitLoadingAudio()
    {
        while (sayClip.loadState != AudioDataLoadState.Loaded)
        {
            yield return null;
        }
        Music.PlayOneShot(sayClip);
    }

    public void Start()
    {
        //LoadBundle();
    }

    private void LoadBundle()
    {
        //string path = Path.GetFullPath(folder + "/" + fileName);
        string path = bundleFolder + bundleName;
        Debug.Log(path);
        voiceBundle = AssetBundle.LoadFromFile(path);

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
