using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioClip sayClip;
    [SerializeField]
    private AudioSource music;

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
        string folder = "!Audio";
        sayClip = Resources.Load<AudioClip>(folder + "/" + fileName);
    }
    public void SayWord()
    {
        if (sayClip == null)
            return;
        Music.PlayOneShot(sayClip);
    }
}
