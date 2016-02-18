//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using LeopotamGroup.LazyGui.Core;
using LeopotamGroup.LazyGui.UnityEditors;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LeopotamGroup.LazyGui.Widgets.UnityEditors {
    [CustomEditor (typeof (LguiLabel))]
    sealed class LguiLabelInspector : Editor {
        public override void OnInspectorGUI () {
            serializedObject.Update ();
            EditorGUILayout.PropertyField (serializedObject.FindProperty ("_font"));
            EditorGUILayout.PropertyField (serializedObject.FindProperty ("_fontSize"));
            EditorGUILayout.PropertyField (serializedObject.FindProperty ("_fontStyle"));
            EditorGUILayout.PropertyField (serializedObject.FindProperty ("_alignment"));
            EditorGUILayout.PropertyField (serializedObject.FindProperty ("_text"));
            EditorGUILayout.PropertyField (serializedObject.FindProperty ("_width"));
            EditorGUILayout.PropertyField (serializedObject.FindProperty ("_height"));
            EditorGUILayout.PropertyField (serializedObject.FindProperty ("_depth"));
            EditorGUILayout.PropertyField (serializedObject.FindProperty ("_color"));

            var propLineHeight = serializedObject.FindProperty ("_lineHeight");
            var prevLineHeight = propLineHeight.floatValue;
            EditorGUILayout.PropertyField (propLineHeight);
            if (propLineHeight.floatValue <= 0f) {
                propLineHeight.floatValue = prevLineHeight;
            }

            if (serializedObject.ApplyModifiedProperties () || (Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "UndoRedoPerformed")) {
                EditorIntegration.UpdateVisuals (target);
            }
        }

        [DrawGizmo (GizmoType.NonSelected | GizmoType.InSelectionHierarchy)]
        static void OnDrawRootGizmo (LguiLabel lbl, GizmoType gizmoType) {
            if (lbl.IsVisible) {
                Gizmos.color = Color.white;
                Gizmos.DrawWireCube (lbl.transform.position, new Vector3 (lbl.Width, lbl.Height, 0f));
            }
        }
    }

    sealed class LguiLabelOnSceneSaveFixer : UnityEditor.AssetModificationProcessor {
        static string[] OnWillSaveAssets (string[] paths) {
            var currentScene = SceneManager.GetActiveScene ().name;
            foreach (var path in paths) {
                if (path == currentScene) {
                    FixLabels ();
                    break;
                }
            }

            return paths;
        }

        static void FixLabels () {
            foreach (var item in Object.FindObjectsOfType<LguiLabel> ()) {
                item.AddVisualChanges (ChangeType.Geometry);
                EditorUtility.SetDirty (item);
            }
        }
    }
}