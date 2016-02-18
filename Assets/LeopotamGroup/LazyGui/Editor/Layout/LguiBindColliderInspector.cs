//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using LeopotamGroup.LazyGui.Core;
using UnityEditor;
using UnityEngine;

namespace LeopotamGroup.LazyGui.Layout.UnityEditors {
    [CustomEditor (typeof (LguiBindColliderSize))]
    public class LguiBindColliderInspector : Editor {
        public override void OnInspectorGUI () {
            serializedObject.Update ();
            var matcher = target as LguiBindColliderSize;
            EditorGUILayout.PropertyField (serializedObject.FindProperty ("_target"));
            if (serializedObject.ApplyModifiedProperties () && matcher.Target != null) {
                matcher.SendMessage (LguiConsts.MethodOnLguiVisualSizeChanged, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}