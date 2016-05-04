//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System.IO;
using LeopotamGroup.Serialization;
using UnityEditor;
using UnityEngine;

namespace LeopotamGroup.EditorHelpers.UnityEditors {
    /// <summary>
    /// Build info on build processing helper.
    /// </summary>
    static class BuildInfoInspector {
        [PreBuild]
        static void UpdateBuildInfo () {
            var buildVersion = new BuildInfo {
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