//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d License
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace LeopotamGroup.EditorHelpers.UnityEditors {
    [AttributeUsage (AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class PreBuildAttribute : Attribute {
        public bool UseInEditorPlay = false;

        static string _lastBuiltVersion;

        public PreBuildAttribute () : this (false) {
        }

        public PreBuildAttribute (bool useInEditorPlay) {
            UseInEditorPlay = useInEditorPlay;
        }

        [PostProcessScene]
        static void OnBuild () {
            if (PlayerSettings.bundleVersion != _lastBuiltVersion) {
                _lastBuiltVersion = PlayerSettings.bundleVersion;
                var inEditor = Application.isPlaying;

                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                    foreach (var type in assembly.GetTypes()) {
                        foreach (var method in type.GetMethods (BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)) {
                            var attrs = method.GetCustomAttributes (typeof (PreBuildAttribute), false);
                            if (attrs.Length > 0) {
                                if (!inEditor || (attrs[0] as PreBuildAttribute).UseInEditorPlay) {
                                    try {
                                        method.Invoke (null, null);
                                    } catch (Exception ex) {
                                        Debug.LogError (ex);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}