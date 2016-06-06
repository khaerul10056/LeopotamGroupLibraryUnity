//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System;
using System.Collections.Generic;
using LeopotamGroup.Common;
using LeopotamGroup.Serialization;
using UnityEngine;

namespace LeopotamGroup.Localization {
    /// <summary>
    /// Localizer - helper for localization. Supports dynamic overriding of localization tokens with rollback.
    /// </summary>
    public static class Localizer {
        /// <summary>
        /// Current language.
        /// </summary>
        /// <value>The language.</value>
        public static string Language {
            get { return _language; }
            set {
                if (!string.IsNullOrEmpty (value) && _language != value) {
                    SetLanguage (value);
                    RelocalizeUI ();
                }
            }
        }

        const string HeaderToken = "KEY";

        const string DefaultStaticSourcePath = "Localization";

        const string DefaultLanguage = "English";

        const string SettingsKey = "lg.locale";

        const string OnLocalizeMethodName = "OnLocalize";

        static readonly Dictionary<string, string[]> _statics = new Dictionary<string, string[]> (64);

        static readonly Dictionary<string, string[]> _dynamics = new Dictionary<string, string[]> (64);

        static string[] _header;

        static string _language;

        static int _langID;

        static Localizer () {
            _header = null;
            _statics.Clear ();
            UnloadDynamics ();
            AddStaticSource (DefaultStaticSourcePath);
            SetLanguage (PlayerPrefs.GetString (SettingsKey, DefaultLanguage));
        }

        static void SetLanguage (string lang) {
            _language = lang;
            _langID = _header != null ? Array.IndexOf (_header, _language) : -1;
            PlayerPrefs.SetString (SettingsKey, _language);
        }

        static string LoadAsset (string assetPath) {
            var asset = Resources.Load<TextAsset> (assetPath);
            string data = null;
            if (asset != null) {
                data = asset.text;
                Resources.UnloadAsset (asset);
            }
            return data;
        }

        static void LoadData (string data, Dictionary<string, string[]> storage) {
            CsvSerialization.DeserializeStatic (data, storage);
            if (!storage.ContainsKey (HeaderToken)) {
                storage.Clear ();
                return;
            }

            foreach (var item in storage) {
                for (int i = 0, iMax = item.Value.Length; i < iMax; i++) {
                    item.Value[i] = item.Value[i].Replace ("\\n", "\n");
                }
            }

            _header = storage[HeaderToken];
            if (!string.IsNullOrEmpty (_language)) {
                var langID = Array.IndexOf (_header, _language);
                if (_langID != -1 && langID != _langID) {
#if UNITY_EDITOR
                    Debug.LogWarning ("Invalid languages order in source, skipping.");
#endif
                    return;
                }
                if (_langID == -1) {
                    _langID = langID;
                }
            }
        }

        /// <summary>
        /// Get localization for token.
        /// </summary>
        /// <param name="token">Localization token.</param>
        public static string Get (string token) {
            if (_langID == -1) {
                return token;
            }
            return _dynamics.ContainsKey (token) ?
                _dynamics[token][_langID] : (_statics.ContainsKey (token) ? _statics[token][_langID] : token);
        }

        /// <summary>
        /// Add non-unloadable localization source.
        /// </summary>
        /// <returns>The static source.</returns>
        /// <param name="sourcePath">Source path.</param>
        public static void AddStaticSource (string sourcePath) {
            var data = LoadAsset (sourcePath);
            if (!string.IsNullOrEmpty (data)) {
                LoadData (data, _statics);
            }
        }

        /// <summary>
        /// Add non-unloadable localization source. Can overrides loaded tokens and be removed by UnloadDynamics call.
        /// </summary>
        /// <returns>The dynamic source.</returns>
        /// <param name="sourcePath">Source path.</param>
        public static void AddDynamicSource (string sourcePath) {
            var data = LoadAsset (sourcePath);
            if (!string.IsNullOrEmpty (data)) {
                LoadData (data, _dynamics);
            }
        }

        /// <summary>
        /// Unload all dynamics localization sources.
        /// </summary>
        /// <returns>The dynamics.</returns>
        public static void UnloadDynamics () {
            _dynamics.Clear ();
        }

        /// <summary>
        /// Raise "OnLocalize" message on all active GameObjects.
        /// </summary>
        /// <returns>The user interface.</returns>
        public static void RelocalizeUI () {
            Extensions.BroadcastToAll (OnLocalizeMethodName);
        }
    }
}