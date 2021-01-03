using UnityEngine;

namespace LinguaLeo._Adapters
{
    public class AssetBundleAdapt
    {
        #region Public Methods

        public static AssetBundle LoadFromFile(string path)
        {
#if UNITY_2017 || UNITY_2018 || UNITY_2019 || UNITY_2020
            return AssetBundle.LoadFromFile(path);
#endif
#if UNITY_5_2
        return AssetBundle.CreateFromFile(path);
#endif
        }

        #endregion
    }
}
