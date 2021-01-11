using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Behaviour._02b_groupChoice
{
    /// <summary>
    ///     Поиск в списке по первым набранным буквам.
    /// </summary>
    public class SearchByFirstChar : MonoBehaviour
    {
        #region Public variables

        public Text gt;

        #endregion

        #region Private variables

        private SelectGroup selectGroup;

        #endregion

        #region Unity events

        private void Start()
        {
            gt = GetComponent<Text>();
            selectGroup = FindObjectOfType<SelectGroup>();
        }

        #endregion

        #region Private Methods

        private WordSetPanel FindPanelByCaption(string substring)
        {
            substring = substring.ToLower();
            var allWordSetPanel = selectGroup.GetTiles();

            var actualPanels = from p in allWordSetPanel
                               orderby p.GetName()
                               where p.GetName().ToLower().StartsWith(substring)
                               select p;
            if (!actualPanels.Any())
                actualPanels = from p in allWordSetPanel
                               orderby p.GetName()
                               where p.GetName().ToLower().Contains(substring)
                               select p;

            return actualPanels.Count() > 0 ? actualPanels.First() : null;
        }

        private void FindSubString()
        {
            foreach (var c in Input.inputString)
            {
                if (c == "\b"[0])
                {
                    if (gt.text.Length != 0)

                        gt.text = gt.text.Substring(0, gt.text.Length - 1);
                } else { gt.text += c; }

                var panel = FindPanelByCaption(gt.text);
                if (panel == null)
                    return;
                GoToPanel(panel);
                EventSystem.current.SetSelectedGameObject(panel.gameObject);
            }
        }

        private void GoToPanel(WordSetPanel panel)
        {
            var index = selectGroup.GetTiles().ToList().FindIndex(x => panel.GetName() == x.GetName());

            var high = selectGroup.GetHightToTile(index);

            selectGroup.SetHeigtContent(high);
            selectGroup.HighLightTile(index);
        }

        private void Update()
        {
            FindSubString();
        }

        #endregion
    }
}
