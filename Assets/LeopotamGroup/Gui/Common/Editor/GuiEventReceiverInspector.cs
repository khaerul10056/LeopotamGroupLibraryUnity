//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using LeopotamGroup.Gui.UnityEditors;
using UnityEditor;
using UnityEngine;

namespace LeopotamGroup.Gui.Common.UnityEditors {
    [CustomEditor (typeof (GuiEventReceiver))]
    class GuiEventReceiverInspector : Editor {
        public override void OnInspectorGUI () {
            serializedObject.Update ();

            EditorGUILayout.PropertyField (serializedObject.FindProperty ("Width"));
            EditorGUILayout.PropertyField (serializedObject.FindProperty ("Height"));
            EditorGUILayout.IntSlider (serializedObject.FindProperty ("Depth"), -49, 49);
            EditorGUILayout.PropertyField (serializedObject.FindProperty ("OnPress"));
            EditorGUILayout.PropertyField (serializedObject.FindProperty ("OnClick"));
            EditorGUILayout.PropertyField (serializedObject.FindProperty ("OnDrag"));

            if (serializedObject.ApplyModifiedProperties () || EditorIntegration.IsUndo ()) {
                EditorIntegration.UpdateVisuals (target);
            }
        }

        [DrawGizmo (GizmoType.NonSelected | GizmoType.InSelectionHierarchy)]
        static void OnDrawRootGizmo (GuiEventReceiver receiver, GizmoType gizmoType) {
            if (receiver.enabled) {
                var tr = receiver.transform;
                var oldColor = Gizmos.color;
                Gizmos.color = (gizmoType & GizmoType.InSelectionHierarchy) != 0 ? Color.green : new Color (0f, 0.5f, 0f);
                var oldMat = Gizmos.matrix;
                Gizmos.matrix = Matrix4x4.TRS (tr.position, tr.rotation, tr.localScale);
                Gizmos.DrawWireCube (Vector3.zero, new Vector3 (receiver.Width, receiver.Height));
                Gizmos.matrix = oldMat;
                Gizmos.color = oldColor;
            }
        }
    }
}