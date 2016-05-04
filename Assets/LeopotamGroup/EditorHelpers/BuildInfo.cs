//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using LeopotamGroup.Serialization;
using UnityEngine;

namespace LeopotamGroup.EditorHelpers {
    /// <summary>
    /// Build info data with availability at runtime on all platforms.
    /// </summary>
    public sealed class BuildInfo {
        /// <summary>
        /// Namespace of app.
        /// </summary>
        [JsonName ("a")]
        public string AppName = InvalidName;

        /// <summary>
        /// Version of app.
        /// </summary>
        [JsonName ("v")]
        public string AppVersion = InvalidVersion;

        static BuildInfo _instance;

        /// <summary>
        /// File name of asset to save BuildInfo data.
        /// </summary>
        public const string AssetName = "BuildInfo";

        /// <summary>
        /// Default (invalid) name of app.
        /// </summary>
        public const string InvalidName = "invalid-name";

        /// <summary>
        /// Default (invalid) version of app.
        /// </summary>
        public const string InvalidVersion = "invalid-version";

        /// <summary>
        /// Get singleton instance of BuildInfo data.
        /// </summary>
        /// <value>The instance.</value>
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