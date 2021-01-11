// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

#region

using System.Collections.Generic;
using UnityEngine;

#endregion

namespace Helpers.Interfaces
{
    public interface IExternalResourceManager
    {
        #region Public Methods

        AudioClip GetAudioClip(string fileName);

        Sprite GetCover(string fileName);

        Sprite GetPicture(string fileName);
        WordCollection GetVocabulary();
        
        List<WordGroup> GetWordGroup();

        void SaveVocabulary(WordCollection vocabulary);

        void SaveWordGroup(List<WordGroup> groups);

        #endregion
    }
}
