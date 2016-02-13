//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using LeopotamGroup.Serialization;
using UnityEngine;

namespace LeopotamGroup.EditorHelpers {
    public sealed class BuildInfo {
        [JsonName ("a")]
        public string AppName = InvalidName;

        [JsonName ("v")]
        public string AppVersion = InvalidVersion;

        static BuildInfo _instance;

        public const string AssetName = "BuildInfo";

        public const string InvalidName = "invalid-name";

        public const string InvalidVersion = "invalid-version";

        public static BuildInfo Instance {
            get {
                if (_instance == null) {
                    var asset = Resources.Load<TextAsset> (AssetName);
                    if (asset != null) {
                        try {
                            _instance = JsonSerialization.DeserializeStatic<BuildInfo> (asset.text);
                        } catch {
                            _instance = null;
                        }
                        Resources.UnloadAsset (asset);
                    }
                    if (_instance == null) {
                        _instance = new BuildInfo ();
                    }
                }
                return _instance;
            }
        }

        public override string ToString () {
            return string.Format ("{0}/{1}", AppName, AppVersion);
        }
    }
}