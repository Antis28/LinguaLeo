using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace LinguaLeo.Scripts.Helpers.Interfaces
{
    public interface IResourcesLoader
    {
        event Action NotifyLoadingCompleted;
        event Action<float>  NotifyLoadingProgress;
        
        #region Public Methods

        Task<AudioClip> GetAudioClip(string fileName);
        Sprite GetCover(string fileName);
        Sprite GetPicture(string fileName);

        WordCollection LoadVocabulary();

        List<WordGroup> LoadWordGroup();
        void SaveVocabulary(WordCollection vocabulary);
        void SaveWordGroup(List<WordGroup> groups);

        #endregion
    }
}
