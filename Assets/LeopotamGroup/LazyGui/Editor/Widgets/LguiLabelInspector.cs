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
            var prop = serializedObject.FindProperty ("_fontSize");
            EditorGUILayout.PropertyField (prop);
            if (prop.intValue < 4) {
                prop.intValue = 4;
            }
            EditorGUILayout.PropertyField (serializedObject.FindProperty ("_alignment"));
            EditorGUILayout.PropertyField (serializedObject.FindProperty ("_text"));
            EditorGUILayout.PropertyField (serializedObject.FindProperty ("_width"));
            EditorGUILayout.PropertyField (serializedObject.FindProperty ("_height"));
            EditorGUILayout.PropertyField (serializedObject.FindProperty ("_depth"));
            EditorGUILayout.PropertyField (serializedObject.FindProperty ("_color"));

            prop = serializedObject.FindProperty ("_lineHeight");
            EditorGUILayout.PropertyField (prop);
            if (prop.floatValue <= 0.1f) {
                prop.floatValue = 0.1f;
            }

            prop = serializedObject.FindProperty ("_effect");
            EditorGUILayout.PropertyField (prop);

            if (prop.enumValueIndex != 0) {
                prop = serializedObject.FindProperty ("_effectValue");
                prop.vector2Value = EditorGUILayout.Vector2Field ("Effect Value", prop.vector2Value);
                EditorGUILayout.PropertyField (serializedObject.FindProperty ("_effectColor"));
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
}