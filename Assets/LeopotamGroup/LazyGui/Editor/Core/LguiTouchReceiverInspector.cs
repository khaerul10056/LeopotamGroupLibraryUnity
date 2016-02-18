//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using LeopotamGroup.LazyGui.UnityEditors;
using UnityEditor;
using UnityEngine;

namespace LeopotamGroup.LazyGui.Core.UnityEditors {
    [CustomEditor (typeof (LguiEventReceiver))]
    sealed class LguiTouchReceiverInspector : Editor {
        public override void OnInspectorGUI () {
            serializedObject.Update ();

            EditorGUILayout.PropertyField (serializedObject.FindProperty ("OnPress"));
            EditorGUILayout.PropertyField (serializedObject.FindProperty ("OnClick"));
            EditorGUILayout.PropertyField (serializedObject.FindProperty ("OnDrag"));
            EditorGUILayout.PropertyField (serializedObject.FindProperty ("OnStateChanged"));

            if (serializedObject.ApplyModifiedProperties () || (Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "UndoRedoPerformed")) {
                EditorIntegration.UpdateVisuals (target);
            }
        }
    }
}