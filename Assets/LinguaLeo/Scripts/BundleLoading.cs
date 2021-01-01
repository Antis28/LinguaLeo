using System;
using LinguaLeo.Scripts.Helpers.Interfaces;
using LinguaLeo.Scripts.Helpers.ResourceLoading.ResourceLoaderImplements;
using UnityEngine;
using UnityEngine.UI;

namespace LinguaLeo.Scripts
{
    public class BundleLoading : MonoBehaviour
    {
        [SerializeField]
        private Image imgProgressLoading;

        [SerializeField]
        private Text textProgressLoading;
        
        [SerializeField]
        private Image imgBundleLoading;

        private AssetsBundleLoader loader;


        // Start is called before the first frame update
        private void Start()
        {
            loader = new AssetsBundleLoader();
            loader.NotifyBundleProgress += f => { imgBundleLoading.fillAmount = f; };
            loader.NotifyLoadingCompleted += () =>
            {
                print("LoadingCompleted");
            };
            loader.NotifyLoadingProgress += f =>
            {
                UpdateProgressLoading(f);
                print(f);
            };
            loader.NotifyProgress += print;
            
            loader.LoadAllBundlesAsync();
        }

        private void UpdateProgressLoading(float value)
        {
            imgProgressLoading.fillAmount = value/100;
            textProgressLoading.text = $"{value:0}%";
        }
    }
}
