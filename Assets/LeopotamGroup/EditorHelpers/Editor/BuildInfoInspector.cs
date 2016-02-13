//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System;
using System.IO;
using LeopotamGroup.Serialization;
using UnityEditor;
using UnityEngine;

namespace LeopotamGroup.EditorHelpers.UnityEditors {
    static class BuildInfoInspector {
        [PreBuild (true)]
        public static void UpdateBuildInfo () {
            var buildVersion = new BuildInfo
            {
                AppName = PlayerSettings.bundleIdentifier,
                AppVersion = PlayerSettings.bundleVersion
            };
                
            var path = Application.dataPath + "/Resources";
            if (!Directory.Exists (path)) {
                Directory.CreateDirectory (path);
            }
            File.WriteAllText (path + "/" + BuildInfo.AssetName + ".txt", JsonSerialization.SerializeStatic (buildVersion));
            AssetDatabase.Refresh ();
//            Debug.Log ("Build version packed");
        }
    }
}