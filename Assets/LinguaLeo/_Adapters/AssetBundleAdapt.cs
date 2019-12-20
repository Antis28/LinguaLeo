﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleAdapt {

    public static AssetBundle LoadFromFile(string path)
    {
#if UNITY_2018
        return AssetBundle.LoadFromFile(path);
#endif
#if UNITY_2017
        return AssetBundle.LoadFromFile(path);
#endif
#if UNITY_5_2
        return AssetBundle.CreateFromFile(path);
#endif
    }
}
