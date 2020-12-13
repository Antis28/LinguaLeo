using UnityEditor;

namespace Editor.Hierarchy
{
    public class BatchRename : ScriptableWizard
    {
        #region Public variables

        public string baseName = "MyObject_";
        public int startNumber = 0;
        public int increment = 1;

        #endregion

        #region Events

        // Вызывается при первом появлении
        public void OnEnable()
        {
            UpdateSelectionHelper();
        }

        // Rename
        private void OnWizardCreate()
        {
            // If selection empty, then exit
            if (Selection.objects == null)
                return;

            // Current increment
            int postFix = startNumber;

            // Cycle and rename
            foreach (var obj in Selection.objects)
            {
                obj.name = baseName + postFix;
                postFix += increment;
            }
        }

        #endregion

        #region Private Methods

        [MenuItem("MyFeature/BatchRename...")]
        private static void CreateWizard()
        {
            DisplayWizard<BatchRename>("BatchRename", "Rename");
        }

        /// <summary>
        /// Update selection counter
        /// </summary>
        private void UpdateSelectionHelper()
        {
            helpString = "";
            if (Selection.objects != null) { helpString = "Колличество объектов: " + Selection.objects.Length; }
        }

        #endregion
    }
}
