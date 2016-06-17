//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System.Collections.Generic;
using LeopotamGroup.Gui.UnityEditors;
using UnityEditor;
using UnityEngine;

namespace LeopotamGroup.Gui.Common.UnityEditors {
    [CustomEditor (typeof (GuiSystem))]
    sealed class GuiSystemInspector : Editor {
        public override void OnInspectorGUI () {
            serializedObject.Update ();

            EditorGUILayout.PropertyField (serializedObject.FindProperty ("_clearFlags"), new GUIContent ("Clear flags"));
            EditorGUILayout.PropertyField (serializedObject.FindProperty ("_backgroundColor"));

            var cullingProp = serializedObject.FindProperty ("_cullingMask");
            cullingProp.intValue = LayerMaskField ("Culling mask", cullingProp.intValue);

            EditorGUILayout.PropertyField (serializedObject.FindProperty ("_depth"));

            var heightProp = serializedObject.FindProperty ("_screenHeight");
            EditorGUILayout.PropertyField (heightProp);
            if (heightProp.intValue <= 0) {
                heightProp.intValue = 1;
            }

            EditorGUILayout.PropertyField (serializedObject.FindProperty ("IsInputLocked"));

            if (serializedObject.ApplyModifiedProperties () || EditorIntegration.IsUndo ()) {
                (target as GuiSystem).Validate ();
                EditorIntegration.UpdateVisuals (target);
            }
        }

        [DrawGizmo (GizmoType.NonSelected | GizmoType.InSelectionHierarchy)]
        static void OnDrawRootGizmo (GuiSystem guiSystem, GizmoType gizmoType) {
            if (guiSystem.enabled && guiSystem.Camera != null) {
                var oldColor = Gizmos.color;
                Gizmos.color = Color.magenta;
                Gizmos.matrix = Matrix4x4.TRS (guiSystem.transform.position, guiSystem.transform.rotation, guiSystem.transform.localScale);
                Gizmos.DrawWireCube (Vector3.zero, new Vector3 (guiSystem.Camera.aspect * guiSystem.ScreenHeight, guiSystem.ScreenHeight, 0f));
                Gizmos.color = oldColor;
            }
        }

        static readonly List<int> _layerNumbers = new List<int> (32);

        static LayerMask LayerMaskField (string label, LayerMask layerMask) {
            var layers = UnityEditorInternal.InternalEditorUtility.layers;

            _layerNumbers.Clear ();

            for (int i = 0; i < layers.Length; i++) {
                _layerNumbers.Add (LayerMask.NameToLayer (layers[i]));
            }

            var maskWithoutEmpty = 0;
            var count = _layerNumbers.Count;
            for (var i = 0; i < count; i++) {
                if (((1 << _layerNumbers[i]) & layerMask.value) != 0) {
                    maskWithoutEmpty |= (1 << i);
                }
            }

            maskWithoutEmpty = EditorGUILayout.MaskField (label, maskWithoutEmpty, layers);

            var mask = 0;
            for (int i = 0; i < count; i++) {
                if ((maskWithoutEmpty & (1 << i)) != 0) {
                    mask |= (1 << _layerNumbers[i]);
                }
            }
            layerMask.value = mask;

            return layerMask;
        }
    }
}