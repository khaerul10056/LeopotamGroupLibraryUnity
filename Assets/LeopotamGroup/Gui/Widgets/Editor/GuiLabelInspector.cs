//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using LeopotamGroup.Gui.UnityEditors;
using UnityEditor;
using UnityEngine;

namespace LeopotamGroup.Gui.Widgets.UnityEditors {
    [CustomEditor (typeof (GuiLabel))]
    sealed class GuiLabelInspector : Editor {
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
            EditorGUILayout.IntSlider (serializedObject.FindProperty ("_depth"), -49, 49);
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
                
            if (GUILayout.Button ("Bake scale to widget size")) {
                GuiLabel l;
                foreach (var item in targets) {
                    l = item as GuiLabel;
                    if (l != null) {
                        l.BakeScale ();
                    }
                }
                SceneView.RepaintAll ();
            }

            EditorGUILayout.HelpBox ("Only strings with length <= 75 (except spaces) can be batched.\n\n" +
            "Only strings with length <= 37 and shadow effect (except spaces) can be batched.\n\n" +
            "Only strings with length <= 15 and outline effect (except spaces) can be batched.\n\n" +
            "Copy&paste labels for creating custom shading/glowing and keep batching.", MessageType.Warning);

            if (serializedObject.ApplyModifiedProperties () || EditorIntegration.IsUndo ()) {
                EditorIntegration.UpdateVisuals (target);
            }
        }

        [DrawGizmo (GizmoType.NonSelected | GizmoType.InSelectionHierarchy)]
        static void OnDrawRootGizmo (GuiLabel lbl, GizmoType gizmoType) {
            if (lbl.IsVisible) {
                Gizmos.color = (gizmoType & GizmoType.InSelectionHierarchy) != 0 ? Color.yellow : new Color (0.5f, 0.5f, 0f);
                Gizmos.DrawWireCube (lbl.transform.position, new Vector3 (lbl.Width, lbl.Height, 0f));
            }
        }
    }
}