using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using LeopotamGroup.Serialization;
using UnityEditor;
using UnityEngine;

namespace LeopotamGroup.Localization.UnityEditors {
    sealed class LocalizationUpdater : EditorWindow {
        const string Title = "Locale updater";

        const string EditorPrefsKey = "_lgl_localeAutoUpdaterData";

        const string UrlDefault = "http://localhost";

        Dictionary<string, string> _paths;

        Vector2 _scrollPos;

        string _newUrl;

        [MenuItem ("Window/LeopotamGroupLibrary/Localization updater")]
        static void OpenEditorWindow () {
            GetWindow<LocalizationUpdater> ();
        }

        void OnEnable () {
            titleContent.text = Title;
            _scrollPos = Vector2.zero;
        }

        void Load () {
            try {
                _paths = JsonSerialization.DeserializeStatic<Dictionary<string, string>> (EditorPrefs.GetString (EditorPrefsKey, string.Empty));
            } catch {
                _paths = new Dictionary<string, string> ();
            }
        }

        void Save () {
            if (_paths != null) {
                EditorPrefs.SetString (EditorPrefsKey, JsonSerialization.SerializeStatic (_paths));
            }
        }

        void OnDisable () {
            Save ();
        }

        void OnGUI () {
            if (_paths == null) {
                Load ();
            }

            GUILayout.BeginScrollView (_scrollPos);
            var removed = false;
            foreach (var key in new List<string> (_paths.Keys)) {
                GUILayout.BeginHorizontal ();
                EditorGUILayout.LabelField ("Url:", key);
                if (GUILayout.Button ("Remove")) {
                    _paths.Remove (key);
                    removed = true;
                }
                GUILayout.EndHorizontal ();
                if (!removed) {
                    _paths[key] = EditorGUILayout.TextField ("Resources file:", _paths[key]).Trim ();
                } else {
                    removed = false;
                }
            }
            GUILayout.EndScrollView ();

            GUILayout.BeginHorizontal ();
            if (string.IsNullOrEmpty (_newUrl)) {
                _newUrl = UrlDefault;
            }
            _newUrl = EditorGUILayout.TextField ("New url:", _newUrl).Trim ();
            GUI.enabled = !_paths.ContainsKey (_newUrl);
            if (GUILayout.Button ("Add")) {
                _paths.Add (_newUrl, string.Empty);
                _newUrl = UrlDefault;
            }
            GUI.enabled = true;
            GUILayout.EndHorizontal ();

            if (GUILayout.Button ("Process")) {
                var res = Process (_paths);
                EditorUtility.DisplayDialog (titleContent.text, res ?? "Success", "Close");
            }
        }

        public static string Process (Dictionary<string, string> paths) {
            if (paths == null || paths.Count == 0) {
                return "No data";
            }
            try {
                ServicePointManager.ServerCertificateValidationCallback = delegate {
                    return true;
                };

                using (var www = new WebClient ()) {
                    www.Encoding = Encoding.UTF8;
                    string data;
                    foreach (var pair in paths) {
                        
                        if (!string.IsNullOrEmpty (pair.Value)) {
                            // Dirty hack for url, because standard "publish to web" has huge lag up to 30 minutes.
                            data = www.DownloadString (pair.Key.Replace ("/edit?", "/export?format=csv&"));
                            var path = string.Format ("{0}/Resources/{1}", Application.dataPath, pair.Value);
                            var folder = Path.GetDirectoryName (path);
                            if (!Directory.Exists (folder)) {
                                Directory.CreateDirectory (folder);
                            }
                            File.WriteAllText (path, data, Encoding.UTF8);
                        }
                    }
                }
                AssetDatabase.Refresh ();
                return null;
            } catch (Exception ex) {
                AssetDatabase.Refresh ();
                return ex.ToString ();
            } finally {
                ServicePointManager.ServerCertificateValidationCallback = delegate {
                    return false;
                };
            }
        }
    }
}