using UnityEditor;
using yaSingleton.Utililty;

namespace yaSingleton.Editor {
    /// <summary>
    /// Fallback Singleton Inspector - validates the singleton location. Extend this class if you want the validation in your singletons.
    /// Note: The editor targets ScriptableObjects due BaseSingleton being abstract (no custom inspectors for abstract types).
    /// </summary>
    [CustomEditor(typeof(YScriptableObject), true, isFallback = true)]
    public class SingletonEditor : UnityEditor.Editor {
        protected bool isSingletonEditor;

        /// <summary>
        /// Disable in inherited editors to draw the validation at any point of your code.
        /// </summary>
        protected bool autoDrawSingletonValidation = true;

        protected virtual void OnEnable() {
            isSingletonEditor = target && target.GetType().IsSubclassOf(typeof(BaseSingleton));
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            
            if(autoDrawSingletonValidation) {
                DrawSingletonValidation();
            }
        }

        protected void DrawSingletonValidation() {
            if(!isSingletonEditor) {
                return;
            }

            var path = AssetDatabase.GetAssetPath(target);
            var separator = System.IO.Path.DirectorySeparatorChar;
            var altSeparator = System.IO.Path.AltDirectorySeparatorChar;

            var isPathInvalid = string.IsNullOrEmpty(path) ||
                              (!path.Contains(separator + "Resources" + separator) &&
                               !path.Contains(altSeparator + "Resources" + altSeparator));
            
                if(isPathInvalid) {
                EditorGUILayout.HelpBox("Disabled.", MessageType.Warning, true);
                EditorGUILayout.HelpBox(
                    "All " + target.GetType().Name +
                    " (Singleton) instances must be in the Resources folder. Otherwise they can't be found by the engine.",
                    MessageType.Info, true);
            }
        }
    }
}