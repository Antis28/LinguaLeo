using UnityEditor;

namespace Editor.Hierarchy
{
    public class BatchRename : ScriptableWizard
    {

        public string baseName = "MyObject_";
        public int startNumber = 0;
        public int increment = 1;

        [MenuItem("MyFeature/BatchRename...")]
        static void CreateWizard()
        {
            ScriptableWizard.DisplayWizard<BatchRename>("BatchRename", "Rename");
        }

        // Вызывается при первом появлении
        public void OnEnable()
        {
            UpdateSelectionHelper();
        }

        /// <summary>
        /// Update selection counter
        /// </summary>
        private void UpdateSelectionHelper()
        {
            helpString = "";
            if( Selection.objects != null )
            {
                helpString = "Колличество объектов: " + Selection.objects.Length;
            }
        }

        // Rename
        void OnWizardCreate()
        {
            // If selection empty, then exit
            if( Selection.objects == null )
                return;

            // Current increment
            int postFix = startNumber;

            // Cycle and rename
            foreach( var obj in Selection.objects )
            {            
                obj.name = baseName + postFix;
                postFix += increment;           
            }
        
        }
    }
}
