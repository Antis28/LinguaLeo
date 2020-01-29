using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LinguaLeo.Scripts.Behaviour._02b_groupСhoice
{
    /// <summary>
    /// Поиск в списке по первым набранным буквам.
    /// </summary>
    public class SearchByFirstChar : MonoBehaviour
    {
        public Text gt;
        SelectGroup selectGroup = null;

        void Start()
        {
            gt = GetComponent<Text>();
            selectGroup = FindObjectOfType<SelectGroup>();
        }
        void Update()
        {
            FindSubString();
        }

        private void FindSubString()
        {
            foreach (char c in Input.inputString)
            {
                if (c == "\b"[0])
                {
                    if (gt.text.Length != 0)

                        gt.text = gt.text.Substring(0, gt.text.Length - 1);
                }
                else
                    gt.text += c;


                WordSetPanel panel = FindPanelByCaption(gt.text);
                if (panel == null)
                    return;
                GoToPanel(panel);
                EventSystem.current.SetSelectedGameObject(panel.gameObject);
            }
        }

        private void GoToPanel(WordSetPanel panel)
        {
            int index = selectGroup.GetTiles().ToList().FindIndex((x) => panel.GetName() == x.GetName());

            float high = selectGroup.CalulateHightContainer(index + 1);
            selectGroup.SetHeigtContent(high);
            selectGroup.HighLightTile(index);
        }

        private WordSetPanel FindPanelByCaption(string substring)
        {
            substring = substring.ToLower();
            var allWordSetPanel = selectGroup.GetTiles();

            var actualPanels = from p in allWordSetPanel
                orderby p.GetName()
                where p.GetName().ToLower().StartsWith(substring)
                select p;
            if (actualPanels.Count() == 0)
            {
                actualPanels = from p in allWordSetPanel
                    orderby p.GetName()
                    where p.GetName().ToLower().Contains(substring)
                    select p;
            }

            if (actualPanels.Count() > 0)
                return actualPanels.First();

            return null;
        }
    }
}
