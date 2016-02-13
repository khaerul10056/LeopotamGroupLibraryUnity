//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d License
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using UnityEditor;
using UnityEngine;

namespace LeopotamGroup.EditorHelpers.UnityEditors {
    [CustomPropertyDrawer (typeof (EnumFlagsAttribute))]
    sealed class EnumFlagsAttributeDrawer : PropertyDrawer {
        public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
            property.intValue = EditorGUI.MaskField (position, label, property.intValue, property.enumNames);
        }
    }
}