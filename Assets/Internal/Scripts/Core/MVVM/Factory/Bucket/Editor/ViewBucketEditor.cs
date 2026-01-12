using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Core.MVVM.Bucket.Editor
{
    [CustomEditor(typeof(ViewBucket))]
    public class ViewBucketEditor : UnityEditor.Editor
    {
        private SerializedProperty _entriesProp;
        private SerializedProperty _isValidProp;

        private bool _isValid = true;

        private void OnEnable()
        {
            _entriesProp = serializedObject.FindProperty("viewEntries");
            _isValidProp = serializedObject.FindProperty("isValid");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(_entriesProp, includeChildren: true);
            
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                ReportDuplicateGuids();
            }

            ShowDuplicateGuidWarningsInline();
            _isValidProp.boolValue = _isValid;
            serializedObject.ApplyModifiedProperties();
        }

        private void ReportDuplicateGuids()
        {
            var dups = CollectDuplicateGuidIndexGroups();
            
            if (dups.Count == 0)
            {
                _isValid = true;
                return;
            }

            foreach (var group in dups)
            {
                var indicesStr = string.Join(", ", group);
                Debug.LogError($"[ViewBucket] Duplicate asset detected at entry indices: [{indicesStr}]");
            }
            
            _isValid = false;
        }

        private void ShowDuplicateGuidWarningsInline()
        {
            var dups = CollectDuplicateGuidIndexGroups();
            if (dups.Count == 0)
            {
                return;
            }

            foreach (var group in dups)
            {
                var indicesStr = string.Join(", ", group);
                EditorGUILayout.HelpBox(
                    $"Duplicate asset with the same Type detected at entry indices: [{indicesStr}]",
                    MessageType.Error
                );
            }
        }

        private List<List<int>> CollectDuplicateGuidIndexGroups()
        {
            var guidToIndices = new Dictionary<Type, List<int>>();

            if (_entriesProp == null) return new List<List<int>>();

            for (int i = 0; i < _entriesProp.arraySize; i++)
            {
                var entry = _entriesProp.GetArrayElementAtIndex(i);
                
                var prefabProp = entry.FindPropertyRelative("prefab");
                var prefabObj = prefabProp != null ? prefabProp.objectReferenceValue : null;

                if (prefabObj == null) continue;

                var guid = prefabObj.GetType();
                
                if (!guidToIndices.TryGetValue(guid, out var list))
                {
                    list = new List<int>();
                    guidToIndices[guid] = list;
                }

                list.Add(i);
            }

            return guidToIndices.Values.Where(v => v.Count > 1).ToList();
        }
    }
}