//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using LeopotamGroup.LazyGui.UnityEditors;
using UnityEditor;
using UnityEngine;

namespace LeopotamGroup.LazyGui.Layout.UnityEditor {
    [CustomEditor (typeof (LguiBindSize))]
    sealed class LguiBindSizeInspector : Editor {
        public override void OnInspectorGUI () {
            serializedObject.Update ();
            EditorGUILayout.PropertyField (serializedObject.FindProperty ("Once"));
            EditorGUILayout.PropertyField (serializedObject.FindProperty ("Horizontal"));
            EditorGUILayout.PropertyField (serializedObject.FindProperty ("Vertical"));
            if (serializedObject.ApplyModifiedProperties () || (Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "UndoRedoPerformed")) {
                EditorIntegration.UpdateVisuals (target);
                (target as LguiBindSize).Validate ();
            }
        }
    }
}