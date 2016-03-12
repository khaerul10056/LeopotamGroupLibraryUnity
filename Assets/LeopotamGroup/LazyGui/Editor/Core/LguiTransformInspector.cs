//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using LeopotamGroup.LazyGui.UnityEditors;
using LeopotamGroup.LazyGui.Widgets;
using UnityEditor;
using UnityEngine;

namespace LeopotamGroup.LazyGui.Core.UnityEditors {
    [CanEditMultipleObjects]
    [CustomEditor (typeof (Transform))]
    sealed class LguiTransformInspector : Editor {
        public override void OnInspectorGUI () {
            bool isFound = false;
            foreach (var item in targets) {
                if ((item as Transform).GetComponent <LguiVisualBase> ()) {
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
            if (!isFound) {
                EditorGUILayout.PropertyField (prop.FindPropertyRelative ("z"));
            }
            GUILayout.EndHorizontal ();
            if (isReset) {
                prop.FindPropertyRelative ("x").floatValue = 0f;
                prop.FindPropertyRelative ("y").floatValue = 0f;
                if (!isFound) {
                    prop.FindPropertyRelative ("z").floatValue = 0f;
                }
            }
        }

        void DrawRotation (bool isFound) {
            var prop = serializedObject.FindProperty ("m_LocalRotation");
            GUILayout.BeginHorizontal ();
            var isReset = GUILayout.Button ("R", GUILayout.Width (20f));
            var angles = (serializedObject.targetObject as Transform).localEulerAngles;
            if (!isFound) {
                var newX = WrapAngle (EditorGUILayout.FloatField ("X", angles.x));
                var newY = WrapAngle (EditorGUILayout.FloatField ("Y", angles.y));
                var newZ = WrapAngle (EditorGUILayout.FloatField ("Z", angles.z));
                var dirtyX = false;
                var dirtyY = false;
                var dirtyZ = false;

                if (Mathf.Abs (newX - angles.x) > 0f) {
                    dirtyX = true;
                }
                if (Mathf.Abs (newY - angles.y) > 0f) {
                    dirtyY = true;
                }
                if (Mathf.Abs (newZ - angles.z) > 0f) {
                    dirtyZ = true;
                }
                if (dirtyX || dirtyY || dirtyZ) {
                    Undo.RecordObjects (serializedObject.targetObjects, "lgui.transform-rotate");
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
            } else {
                var newZ = WrapAngle (EditorGUILayout.FloatField ("Z", angles.z));
                if (Mathf.Abs (newZ - angles.z) > 0f) {
                    Undo.RecordObjects (serializedObject.targetObjects, "lgui.transform-rotate");
                    Transform tr;
                    Vector3 v;
                    foreach (var item in targets) {
                        tr = item as Transform;
                        v = tr.localRotation.eulerAngles;
                        v.z = newZ;
                        tr.localEulerAngles = v;
                    }
                }
            }

            GUILayout.EndHorizontal ();
            if (isReset) {
                if (!isFound) {
                    prop.FindPropertyRelative ("x").floatValue = 0f;
                    prop.FindPropertyRelative ("y").floatValue = 0f;
                }
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
            GUILayout.BeginHorizontal ();
            var isReset = GUILayout.Button ("S", GUILayout.Width (20f));
            EditorGUILayout.PropertyField (prop.FindPropertyRelative ("x"));
            EditorGUILayout.PropertyField (prop.FindPropertyRelative ("y"));
            if (!isFound) {
                EditorGUILayout.PropertyField (prop.FindPropertyRelative ("z"));
            }
            GUILayout.EndHorizontal ();
            if (isReset) {
                prop.FindPropertyRelative ("x").floatValue = 1f;
                prop.FindPropertyRelative ("y").floatValue = 1f;
                if (!isFound) {
                    prop.FindPropertyRelative ("z").floatValue = 1f;
                }
            }
        }
    }
}