// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

#region

using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#endregion

namespace LinguaLeo.Scripts.Helpers.ResourceLoading.ResourceLoaderImplements.ServerAccessData
{
    public class BundleLoading : MonoBehaviour
    {
        #region SerializeFields

        [SerializeField]
        private Image imgBundleLoading;

        [SerializeField]
        private Image imgProgressLoading;
        
        [SerializeField]
        private Image imgPanel;

        [SerializeField]
        private UnityEvent PictureComplete;

        [SerializeField]
        private UnityEvent SoundComplete;

        [SerializeField]
        private Text textProgressLoading;

        [SerializeField]
        private UnityEvent VocabularyComplete;

        #endregion

        #region Private variables

        private AssetsBundleLoader loader;

        #endregion

        #region Unity events

        // Start is called before the first frame update
        private  void Start()
        {
            LoadBundlesToMemory();

            ShowAsync();
        }

        private async void ShowAsync()
        {
            textProgressLoading.text = await VL();
        }


        private async Task<string> VL()
        {
            var erm = new ExternalResourceManager();
            await Task.Yield();
            var wC = erm.GetVocabulary();
            var dfd = wC.allWords;


            return dfd[1].wordValue;
        }

        private void LoadBundlesToMemory()
        {
            var counter = 0;
            loader = new AssetsBundleLoader();
            loader.NotifyBundleProgress += f => { imgBundleLoading.fillAmount = f; };
            loader.NotifyLoadingCompleted += () =>
            {
                print("LoadingCompleted");
                imgPanel.sprite = loader.GetPicture("1308");
                imgPanel.color = Color.white;
            };
            loader.NotifyLoadingProgress += f =>
            {
                UpdateProgressLoading(f);
                print(f);
            };
            loader.NotifyProgress += s =>
            {
                counter++;
                print(s);
                switch (counter)
                {
                    case 1:
                        PictureComplete?.Invoke();
                        break;
                    case 2:
                        SoundComplete?.Invoke();
                        break;
                }
            };
            loader.NotifyLoadingFall += s =>
            {
                imgBundleLoading.fillAmount = 0;
                UpdateProgressLoading(0);
            };

            loader.LoadAllBundlesAsync();
        }

        #endregion

        #region Private Methods

        private void UpdateProgressLoading(float value)
        {
            imgProgressLoading.fillAmount = value / 100;
            textProgressLoading.text = $"{value}%";
        }

        #endregion
    }
}
