using UnityEngine;

namespace LinguaLeo.Scripts.Helpers.ResourceLoading
{
    public interface IResourcesLoader
    {
        Sprite GetPicture(string fileName);
        Sprite GetCover(string fileName);
        AudioClip GetAudioClip(string fileName);
    }
}
