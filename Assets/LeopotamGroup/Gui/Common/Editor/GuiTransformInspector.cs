//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using LeopotamGroup.Gui.Layout;
using LeopotamGroup.Gui.UnityEditors;
using LeopotamGroup.Gui.Widgets;
using UnityEditor;
using UnityEngine;

namespace LeopotamGroup.Gui.Common.UnityEditors {
    [CanEditMultipleObjects]
    [CustomEditor (typeof (Transform))]
    sealed class GuiTransformInspector : Editor {
        public override void OnInspectorGUI () {
            bool isFound = false;
            Transform tr;
            foreach (var item in targets) {
                tr = item as Transform;
                if (tr.GetComponent <GuiWidget> () || tr.GetComponent <GuiPanel> () || tr.GetComponent<GuiEventReceiver> ()) {
                    isFound = true;
                    break;
                }
            }

            serializedObject.Update ();
            EditorIntegration.SetLabelWidth (15f);
            DrawPosition (isFound);
            DrawRotation (isFound);
            DrawScale (isFound);

            serializedObject.ApplyModifiedProperties ();
        }

        void DrawPosition (bool isFound) {
            var prop = serializedObject.FindProperty ("m_LocalPosition");
            GUILayout.BeginHorizontal ();
            var isReset = GUILayout.Button ("P", GUILayout.Width (20f));
            EditorGUILayout.PropertyField (prop.FindPropertyRelative ("x"));
            EditorGUILayout.PropertyField (prop.FindPropertyRelative ("y"));
            GUI.enabled = !isFound;
            EditorGUILayout.PropertyField (prop.FindPropertyRelative ("z"));
            GUI.enabled = true;
            GUILayout.EndHorizontal ();
            if (isReset) {
                prop.FindPropertyRelative ("x").floatValue = 0f;
                prop.FindPropertyRelative ("y").floatValue = 0f;
                prop.FindPropertyRelative ("z").floatValue = 0f;
            }
        }

        void DrawRotation (bool isFound) {
            var prop = serializedObject.FindProperty ("m_LocalRotation");
            GUILayout.BeginHorizontal ();
            var isReset = GUILayout.Button ("R", GUILayout.Width (20f));
            var angles = (serializedObject.targetObject as Transform).localEulerAngles;
            GUI.enabled = !isFound;
            var newX = WrapAngle (EditorGUILayout.FloatField ("X", angles.x));
            var newY = WrapAngle (EditorGUILayout.FloatField ("Y", angles.y));
            GUI.enabled = true;
            var newZ = WrapAngle (EditorGUILayout.FloatField ("Z", angles.z));

            var dirtyX = Mathf.Abs (newX - angles.x) > 0f;
            var dirtyY = Mathf.Abs (newY - angles.y) > 0f;
            var dirtyZ = Mathf.Abs (newZ - angles.z) > 0f;

            if (dirtyX || dirtyY || dirtyZ) {
                Undo.RecordObjects (serializedObject.targetObjects, "leopotamgroup.gui.transform-rotate");
                Transform tr;
                Vector3 v;
                foreach (var item in targets) {
                    tr = item as Transform;
                    v = tr.localRotation.eulerAngles;
                    if (dirtyX) {
                        v.x = newX;
                    }
                    if (dirtyY) {
                        v.y = newY;
                    }
                    if (dirtyZ) {
                        v.z = newZ;
                    }
                    tr.localEulerAngles = v;
                }
            }

            GUILayout.EndHorizontal ();
            if (isReset) {
                prop.FindPropertyRelative ("x").floatValue = 0f;
                prop.FindPropertyRelative ("y").floatValue = 0f;
                prop.FindPropertyRelative ("z").floatValue = 0f;
            }
        }

        static float WrapAngle (float angle) {
            while (angle > 180f) {
                angle -= 360f;
            }
            while (angle < -180f) {
                angle += 360f;
            }
            return angle;
        }

        void DrawScale (bool isFound) {
            var prop = serializedObject.FindProperty ("m_LocalScale");

            var fieldX = prop.FindPropertyRelative ("x");
            var fieldY = prop.FindPropertyRelative ("y");
            var fieldZ = prop.FindPropertyRelative ("z");

            // fix invalid z scale.
            if (isFound && fieldZ.floatValue != 1f) {
                fieldZ.floatValue = 1f;
            }

            GUILayout.BeginHorizontal ();
            var isReset = GUILayout.Button ("S", GUILayout.Width (20f));
            EditorGUILayout.PropertyField (fieldX);
            EditorGUILayout.PropertyField (fieldY);

//            Debug.Log (string.Format ("x: {0} / {1}, y: {2} / {3}", oldX, serializedObject.FindPropertyRelative ("x").floatValue, oldY, fieldY.floatValue));



            GUI.enabled = !isFound;
            EditorGUILayout.PropertyField (fieldZ);
            GUI.enabled = true;
            GUILayout.EndHorizontal ();
            if (isReset) {
                fieldX.floatValue = 1f;
                fieldY.floatValue = 1f;
            }
        }
    }
}