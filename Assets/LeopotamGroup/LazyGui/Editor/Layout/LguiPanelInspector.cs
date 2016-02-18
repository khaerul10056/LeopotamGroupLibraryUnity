//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using LeopotamGroup.LazyGui.Core;
using LeopotamGroup.LazyGui.UnityEditors;
using LeopotamGroup.LazyGui.Widgets;
using UnityEditor;
using UnityEngine;

namespace LeopotamGroup.LazyGui.Layout.UnityEditor {
    [CustomEditor (typeof (LguiPanel))]
    sealed class LguiPanelInspector : Editor {
        public override void OnInspectorGUI () {
            var panel = target as LguiPanel;
            if (panel.GetComponent <LguiVisualBase> () != null) {
                EditorGUILayout.HelpBox ("Dont use panel and visual widget on same GameObject, otherwise depth sort issues will come.", MessageType.Warning);
            }
            panel.ClipType = (PanelClipType) EditorGUILayout.EnumPopup ("ClipType", panel.ClipType);
            switch (panel.ClipType) {
                case PanelClipType.Range:
                    var clipData = panel.ClipData;
                    var clipPos = panel.ClipPos;
                    clipPos.x = EditorGUILayout.IntField ("Origin X", (int) clipPos.x);
                    clipPos.y = EditorGUILayout.IntField ("Origin Y", (int) clipPos.y);
                    clipData.x = EditorGUILayout.IntField ("Width", (int) clipData.x);
                    clipData.y = EditorGUILayout.IntField ("Height", (int) clipData.y);
                    clipData.z = EditorGUILayout.IntField ("Soft X", (int) clipData.z);
                    clipData.w = EditorGUILayout.IntField ("Soft Y", (int) clipData.w);
                    panel.ClipData = clipData;
                    panel.ClipPos = clipPos;
                    break;
                case PanelClipType.Texture:
                    EditorGUILayout.HelpBox ("Not implemented", MessageType.Warning);
                    break;
            }
            if (GUI.changed) {
                EditorIntegration.UpdateVisuals (target);
            }
        }

        [DrawGizmo (GizmoType.NonSelected | GizmoType.InSelectionHierarchy)]
        static void OnDrawRootGizmo (LguiPanel panel, GizmoType gizmoType) {
            if (panel.IsPanelActive) {
                switch (panel.ClipType) {
                    case PanelClipType.Range:
                        Gizmos.color = Color.green;
                        Gizmos.DrawWireCube ((Vector3) panel.ClipPos, new Vector3 (panel.ClipData.x, panel.ClipData.y, 0f));
                        break;
                }
            }
        }
    }
}