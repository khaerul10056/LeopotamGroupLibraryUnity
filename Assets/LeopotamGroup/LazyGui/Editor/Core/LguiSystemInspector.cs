//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LeopotamGroup.LazyGui.Core.UnityEditors {
    [CustomEditor (typeof (LguiSystem))]
    sealed class LguiSystemInspector : Editor {
        public override void OnInspectorGUI () {
            var root = target as LguiSystem;
            root.ClearFlags = (CameraClearFlags) EditorGUILayout.EnumPopup ("Clear flags", root.ClearFlags);
            root.BackgroundColor = EditorGUILayout.ColorField ("Background", root.BackgroundColor);
            root.CullingMask = LayerMaskField ("Culling mask", root.CullingMask);
            root.Depth = EditorGUILayout.IntField ("Camera depth", root.Depth);
            root.ScreenHeight = EditorGUILayout.IntField ("Screen height", root.ScreenHeight);
            if (GUI.changed) {
                EditorUtility.SetDirty (root);
            }
        }

        [DrawGizmo (GizmoType.NonSelected | GizmoType.InSelectionHierarchy)]
        static void OnDrawRootGizmo (LguiSystem guiSystem, GizmoType gizmoType) {
            if (guiSystem.enabled && guiSystem.Camera != null) {
                Gizmos.matrix = Matrix4x4.TRS (guiSystem.transform.position, guiSystem.transform.rotation, guiSystem.transform.localScale);
                Gizmos.DrawWireCube (Vector3.zero, new Vector3 (guiSystem.Camera.aspect * guiSystem.ScreenHeight, guiSystem.ScreenHeight, 0f));
            }
        }

        static readonly List<int> _layerNumbers = new List<int> ();

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