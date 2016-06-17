//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using LeopotamGroup.Gui.UnityEditors;
using UnityEditor;

namespace LeopotamGroup.Gui.Layout.UnityEditors {
    [CustomEditor (typeof (GuiPanel))]
    sealed class GuiPanelInspector : Editor {
        public override void OnInspectorGUI () {
            serializedObject.Update ();

            EditorGUILayout.IntSlider (serializedObject.FindProperty ("_depth"), -10, 10);

            if (serializedObject.ApplyModifiedProperties () || EditorIntegration.IsUndo()) {
                EditorIntegration.UpdateVisuals (target);
            }
        }
    }
}