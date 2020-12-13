using System.Collections.Generic;
using UnityEngine;

namespace LinguaLeo.Scripts.Helpers.ResourceLoading
{
    public interface IResourcesLoader
    {
        Sprite GetPicture(string fileName);
        Sprite GetCover(string fileName);
        AudioClip GetAudioClip(string fileName);
        
        WordCollection LoadVocabulary();
        void SaveVocabulary(WordCollection vocabulary);
        
        List<WordGroup> LoadWordGroup();
        void SaveWordGroup(List<WordGroup> groups);
    }
}
