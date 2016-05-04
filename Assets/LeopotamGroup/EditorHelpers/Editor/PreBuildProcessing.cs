//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace LeopotamGroup.EditorHelpers.UnityEditors {
    /// <summary>
    /// Pre build attribute. Methods with PreBuild attribute will be executed during Compilation / Playing / Building.
    /// </summary>
    [InitializeOnLoad]
    [AttributeUsage (AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class PreBuildAttribute : Attribute {
        static string _lastBuiltVersion;

        static PreBuildAttribute () {
            if (PlayerSettings.bundleVersion != _lastBuiltVersion) {
                _lastBuiltVersion = PlayerSettings.bundleVersion;

                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies ()) {
                    foreach (var type in assembly.GetTypes ()) {
                        foreach (var method in type.GetMethods (
                            BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)) {
                            var attrs = method.GetCustomAttributes (typeof (PreBuildAttribute), false);
                            if (attrs.Length > 0) {
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