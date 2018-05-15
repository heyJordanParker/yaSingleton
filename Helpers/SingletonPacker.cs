using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace yaSingleton.Helpers {
    
    // TODO the issue with that approach is although by being in the resources the SingletonPacker gets included in the build, anything it references is lost. A manual referene to the packer is needed anyway. A (never accessed) scene or a hidden object referencing it is necessary. 
    
    /// <summary>
    /// Singleton Packer. Holds a reference to all singletons. Using it you can place one file in the resources folder instead of all singletons. 
    /// </summary>
    public class SingletonPacker : ScriptableObject {
#pragma warning disable CS0414
        [SerializeField]
        public BaseSingleton[] _singletons;
#pragma warning restore CS0414
        
#if UNITY_EDITOR
        private void OnEnable() {
            UpdateSingletons();
        }

        private void UpdateSingletons() {
            _singletons = AssetDatabase.FindAssets("t:BaseSingleton").Select(guid => AssetDatabase.LoadAssetAtPath<BaseSingleton>(AssetDatabase.GUIDToAssetPath(guid))).ToArray();
        }

        /// <inheritdoc />
        /// <summary>
        /// SingletonPackerUpdater. Hooks the SingletonPacker to the AssetModificationProcessor events.
        /// </summary>
        private class SingletonPackerUpdater : UnityEditor.AssetModificationProcessor {
            private static void OnWillCreateAsset(string path) {
                if(!path.EndsWith(".asset")) {
                    return;
                }
    
                UpdateSingletonPacker();
            }
    
            private static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions options) {
                if(assetPath.EndsWith(".asset")) {
                    UpdateSingletonPacker();
                }
    
                return AssetDeleteResult.DidNotDelete;
            }
            
            static string[] OnWillSaveAssets(string[] paths)
            {
                if(paths.Any(p => p.EndsWith(".asset"))) {
                    UpdateSingletonPacker();
                }
                
                return paths;
            }
    
            private static void UpdateSingletonPacker() {
                var singletonPacker = Resources.LoadAll<SingletonPacker>(string.Empty).FirstOrDefault();
                
                if(!singletonPacker) {
                    return;
                }
                
                EditorApplication.delayCall += singletonPacker.UpdateSingletons;
            }
        }
#endif

    }
}
