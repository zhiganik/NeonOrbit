using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace Core.MVVM.Bucket.Editor
{
    [CustomPropertyDrawer(typeof(BucketEntry))]
    public class BucketEntryDrawer : PropertyDrawer
    {
        private static GUIStyle _errorLabel;
        
        public static GUIStyle ErrorLabel
        {
            get
            {
                if (_errorLabel == null)
                {
                    _errorLabel = new GUIStyle(EditorStyles.textField);
                    _errorLabel.normal.textColor = Color.red;
                    _errorLabel.wordWrap = true;
                    _errorLabel.fontStyle = FontStyle.Bold;
                }
                
                return _errorLabel;
            }
        }
        
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            EditorGUI.BeginProperty(pos, label, prop);

            var prefabProp = prop.FindPropertyRelative("prefab");
            var guidProp = prop.FindPropertyRelative("key");
            var isAddressableAsset = prop.FindPropertyRelative("isAddressableAsset");
            var typeProp = prop.FindPropertyRelative("type").FindPropertyRelative("assemblyQualifiedName");

            var line = new Rect(pos.x, pos.y, pos.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(line, label);

            EditorGUI.indentLevel++;
            line.y += line.height + 2;
            EditorGUI.PropertyField(line, prefabProp, new GUIContent("ViewLayer"));
            var error = string.Empty;

            var viewLayer = prefabProp.objectReferenceValue as ViewLayer;
                
            if (viewLayer == null) return;
                
            var inResources = TryGetResourcesRelativePath(viewLayer, out var resourcesPath);
            var inAddressables = ValidateAddressable(viewLayer.gameObject);

            if (!inResources && !inAddressables)
            {
                guidProp.stringValue = string.Empty;
                typeProp.stringValue = string.Empty;
                isAddressableAsset.boolValue = false;
                error = "View must be placed in Addressables or Resources!";
            }
                    
            if (inResources && inAddressables)
            {
                guidProp.stringValue = string.Empty;
                typeProp.stringValue = string.Empty;
                isAddressableAsset.boolValue = false;
                error = "View placed both in Addressables and Resources!";
            }
            else if (inResources)
            {
                guidProp.stringValue = resourcesPath;
                isAddressableAsset.boolValue = false;
            }
            else if (inAddressables)
            {
                var path = AssetDatabase.GetAssetPath(viewLayer);
                guidProp.stringValue = string.IsNullOrEmpty(path) 
                    ? string.Empty 
                    : AssetDatabase.AssetPathToGUID(path);
                        
                isAddressableAsset.boolValue = true;
            }
                    
            typeProp.stringValue = viewLayer.GetType().AssemblyQualifiedName;    

            GUI.enabled = false;
            line.y += line.height + 2;
            EditorGUI.TextField(line, "Key", guidProp.stringValue);
            line.y += line.height + 2;

            if (!string.IsNullOrEmpty(error))
            {
                EditorGUI.TextField(line, "Error", error, ErrorLabel);
            }
            else
            {
                EditorGUI.TextField(line, "Loader Type", isAddressableAsset.boolValue ? "Addressable" : "Resources");
            }
            
            GUI.enabled = true;

            EditorGUI.indentLevel--;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            => EditorGUIUtility.singleLineHeight * 4 + 8;
        
        private bool TryGetResourcesRelativePath(ViewLayer view, out string path)
        {
            path = null;

            if (!TryGetPrefabAssetPath(view.gameObject, out var assetPath))
                return false;

            const string needle = "/Resources/";
            var idx = assetPath.LastIndexOf(needle, System.StringComparison.OrdinalIgnoreCase);
            if (idx < 0)
                return false;

            var within = assetPath.Substring(idx + needle.Length);
            within = Path.ChangeExtension(within, null);
            if (string.IsNullOrEmpty(within))
                return false;

            path = within.Replace("\\", "/");
            return true;
        }
        
        private bool TryGetPrefabAssetPath(GameObject go, out string assetPath)
        {
            assetPath = null;

            if (!PrefabUtility.IsPartOfAnyPrefab(go))
                return false;

            Object assetObj = null;
            if (PrefabUtility.IsPartOfPrefabInstance(go))
            {
                assetObj = PrefabUtility.GetCorrespondingObjectFromSource(go);
            }
            else if (PrefabUtility.IsPartOfPrefabAsset(go))
            {
                assetObj = go;
            }

            if (assetObj == null)
                return false;

            assetPath = AssetDatabase.GetAssetPath(assetObj);
            return !string.IsNullOrEmpty(assetPath);
        }
        
        private bool ValidateAddressable(GameObject prefab)
        {
            if (prefab == null)
            {
                Debug.LogError("Null prefab passed to AddressableValidator.");
                return false;
            }

            string path = AssetDatabase.GetAssetPath(prefab);
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError($"Prefab '{prefab.name}' not found in project.");
                return false;
            }

            string guid = AssetDatabase.AssetPathToGUID(path);
            
            if (string.IsNullOrEmpty(guid))
            {
                Debug.LogError($"Could not resolve GUID for prefab '{prefab.name}'.");
                return false;
            }

            var settings = AddressableAssetSettingsDefaultObject.Settings;
            
            if (settings == null)
            {
                Debug.LogError("Addressable settings not found (check Project Settings > Addressables).");
                return false;
            }

            AddressableAssetEntry entry = settings.FindAssetEntry(guid);
            
            if (entry == null)
            {
                return false;
            }

            if (entry.parentGroup == null)
            {
                return false;
            }

            return true;
        }
    }
}