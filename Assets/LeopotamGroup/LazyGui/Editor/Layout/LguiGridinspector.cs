//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using UnityEditor;
using UnityEngine;

namespace LeopotamGroup.LazyGui.Layout.UnityEditor {
    [CustomEditor (typeof (LguiGrid))]
    sealed class LguiGridinspector : Editor {
        public override void OnInspectorGUI () {
            base.OnInspectorGUI ();
            if (GUILayout.Button ("Validate now")) {
                (target as LguiGrid).Validate ();
            }
        }
    }
}