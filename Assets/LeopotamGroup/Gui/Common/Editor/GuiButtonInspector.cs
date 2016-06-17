//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using LeopotamGroup.Gui.Widgets;
using UnityEditor;

namespace LeopotamGroup.Gui.Common.UnityEditors {
    [CustomEditor (typeof (GuiButton))]
    sealed class GuiButtonInspector : GuiEventReceiverInspector {
        public override void OnInspectorGUI () {
            serializedObject.Update ();

            EditorGUILayout.PropertyField (serializedObject.FindProperty ("Visuals"));
            EditorGUILayout.PropertyField (serializedObject.FindProperty ("EnableColor"));
            EditorGUILayout.PropertyField (serializedObject.FindProperty ("ActiveColor"));
            EditorGUILayout.PropertyField (serializedObject.FindProperty ("DisableColor"));
            EditorGUILayout.PropertyField (serializedObject.FindProperty ("ScaleOnPress"));
            EditorGUILayout.PropertyField (serializedObject.FindProperty ("TweenTime"));

            base.OnInspectorGUI ();
        }
    }
}