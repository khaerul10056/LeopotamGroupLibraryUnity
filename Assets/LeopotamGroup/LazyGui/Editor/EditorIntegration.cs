//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using LeopotamGroup.LazyGui.Core;
using LeopotamGroup.LazyGui.Core.UnityEditors;
using LeopotamGroup.LazyGui.Layout;
using LeopotamGroup.LazyGui.Widgets;
using UnityEditor;
using UnityEngine;

namespace LeopotamGroup.LazyGui.UnityEditors {
    sealed class EditorIntegration : MonoBehaviour {
        [MenuItem ("GameObject/LazyGui/Create/GuiSystem", false, 1)]
        static void CreateGuiSystem () {
            LguiSystem.Instance.Validate ();
        }

        [MenuItem ("GameObject/LazyGui/Create/Widgets/Sprite", false, 1)]
        static void CreateWidgetSprite () {
            SearchWindow.Open<LguiAtlas> ("Select atlas", "t:prefab", null, assetPath => {
                var spr = WidgetFactory.CreateWidgetSprite ();
                Undo.RegisterCreatedObjectUndo (spr.gameObject, "lgui.create-sprite");
                if (!string.IsNullOrEmpty (assetPath)) {
                    spr.SpriteAtlas = AssetDatabase.LoadAssetAtPath<LguiAtlas> (assetPath);
                    var sprNames = spr.SpriteAtlas.GetSpriteNames ();
                    spr.SpriteName = sprNames != null && sprNames.Length > 0 ? sprNames[0] : null;
                    spr.ResetSize ();
                }
                FixWidgetParent (spr);
                UpdateVisuals (spr);
            });
        }

        [MenuItem ("GameObject/LazyGui/Create/Widgets/Label", false, 1)]
        static void CreateWidgetLabel () {
            SearchWindow.Open<Font> ("Select font", "t:font", null, assetPath => {
                var label = WidgetFactory.CreateWidgetLabel ();
                Undo.RegisterCreatedObjectUndo (label.gameObject, "lgui.create-label");
                label.Font = string.IsNullOrEmpty (assetPath) ?
                    Resources.GetBuiltinResource<Font> ("Arial.ttf") : AssetDatabase.LoadAssetAtPath<Font> (assetPath);
                FixWidgetParent (label);
                UpdateVisuals (label);
            });
        }

        [MenuItem ("GameObject/LazyGui/Create/Widgets/Button", false, 1)]
        static void CreateWidgetButton () {
            SearchWindow.Open<LguiAtlas> ("Select atlas", "t:prefab", null, assetPath => {
                var button = WidgetFactory.CreateWidgetButton ();
                Undo.RegisterCreatedObjectUndo (button.gameObject, "lgui.create-btn");
                FixWidgetParent (button);
                if (!string.IsNullOrEmpty (assetPath)) {
                    var spr = button.GetComponentInChildren<LguiSprite> ();
                    spr.SpriteAtlas = AssetDatabase.LoadAssetAtPath<LguiAtlas> (assetPath);
                    var sprNames = spr.SpriteAtlas.GetSpriteNames ();
                    spr.SpriteName = sprNames != null && sprNames.Length > 0 ? sprNames[0] : null;
                    spr.ResetSize ();
                    UpdateVisuals (spr);
                }
                UpdateVisuals (button);
            });
        }

        [MenuItem ("GameObject/LazyGui/Create/Layout/Panel", false, 1)]
        static void CreateWidgetPanel () {
            FixWidgetParent (WidgetFactory.CreateWidgetPanel ());
        }

        [MenuItem ("GameObject/LazyGui/Create/Layout/BindPosition", false, 1)]
        static void CreateLayoutBindPosition () {
            var bind = WidgetFactory.CreateLayoutBindPosition ();
            Undo.RegisterCreatedObjectUndo (bind.gameObject, "lgui.create-bind-pos");
            FixWidgetParent (bind);
        }

        [MenuItem ("GameObject/LazyGui/Add/Layout/Panel", false, 1)]
        static void AddLayoutPanel () {
            Selection.activeGameObject.AddComponent <LguiPanel> ();
        }

        [MenuItem ("GameObject/LazyGui/Add/Layout/Panel", true)]
        static bool AddLayoutPanelValidation () {
            var isValid = Selection.activeGameObject != null && !AssetDatabase.Contains (Selection.activeGameObject) && Selection.activeGameObject.GetComponent <LguiPanel> () == null;
            if (!isValid) {
                EditorUtility.DisplayDialog ("Cant add LguiPanel", "Component can be added to selected GameObject at scene if no exists LguiPanel component on it.", "Cancel");
            }
            return isValid;
        }

        [MenuItem ("GameObject/LazyGui/Add/Layout/BindPosition", false, 1)]
        static void AddLayoutBindPosition () {
            Selection.activeGameObject.AddComponent <LguiBindPosition> ();
        }

        [MenuItem ("GameObject/LazyGui/Add/Layout/BindPosition", true)]
        static bool AddLayoutBindPositionValidation () {
            var isValid = Selection.activeGameObject != null && !AssetDatabase.Contains (Selection.activeGameObject) && Selection.activeGameObject.GetComponent <LguiBindPosition> () == null;
            if (!isValid) {
                EditorUtility.DisplayDialog ("Cant add LguiBindPosition", "Component can be added to selected GameObject at scene if no exists LguiBindPosition components on it.", "Cancel");
            }
            return isValid;
        }

        [MenuItem ("GameObject/LazyGui/Add/Layout/BindSize", false, 1)]
        static void AddLayoutBindSize () {
            Selection.activeGameObject.AddComponent <LguiBindSize> ();
        }

        [MenuItem ("GameObject/LazyGui/Add/Layout/BindSize", true)]
        static bool AddLayoutBindSizeValidation () {
            var isValid = Selection.activeGameObject != null && !AssetDatabase.Contains (Selection.activeGameObject) && Selection.activeGameObject.GetComponent <LguiBindSize> () == null;
            if (!isValid) {
                EditorUtility.DisplayDialog ("Cant add LguiBindSize", "Component can be added to selected GameObject at scene if no exists LguiBindSize components on it.", "Cancel");
            }
            return isValid;
        }

        static void FixWidgetParent (MonoBehaviour widget) {
            if (Selection.activeGameObject != null && !AssetDatabase.Contains (Selection.activeGameObject)) {
                widget.transform.SetParent (Selection.activeGameObject.transform, false);
            }
            Selection.activeGameObject = widget.gameObject;
        }

        [MenuItem ("Assets/LazyGui/Create new atlas asset", false, 1)]
        static void CreateAtlas () {
            string path = AssetDatabase.GetAssetPath (Selection.activeObject);

            string name;
            if (!string.IsNullOrEmpty (path) && AssetDatabase.Contains (Selection.activeObject)) {
                var isFolder = AssetDatabase.IsValidFolder (path);
                if (!isFolder) {
                    path = Path.GetDirectoryName (path);
                }
                name = Path.GetFileNameWithoutExtension (path);
            } else {
                path = "Assets";
                name = "LguiAtlas";
            }

            var asset = new GameObject ();
            asset.AddComponent <LguiAtlas> ();
            PrefabUtility.CreatePrefab (AssetDatabase.GenerateUniqueAssetPath (string.Format ("{0}/{1}.prefab", path, name)), asset);
            DestroyImmediate (asset);
            AssetDatabase.Refresh ();
        }

        [MenuItem ("Assets/LazyGui/Update atlas from folder...", true)]
        static bool CanBeAutoPacked () {
            var sel = Selection.activeGameObject;
            return sel != null && AssetDatabase.Contains (sel) && sel.GetComponent <LguiAtlas> () != null;
        }

        [MenuItem ("Assets/LazyGui/Update atlas from folder...", false, 1)]
        static void UpdateAtlas () {
            var res = LguiAtlasInspector.BakeAtlas (Selection.activeGameObject.GetComponent <LguiAtlas> ());
            EditorUtility.DisplayDialog ("Atlas autopacker", res ?? "Completed", "Close");
        }

        public static void UpdateVisuals (UnityEngine.Object obj) {
            var widget = obj as LguiWidgetBase;
            if (widget != null) {
                widget.SendMessage ("UpdateVisuals", ChangeType.All, SendMessageOptions.DontRequireReceiver);
                widget.SendMessage (LguiConsts.MethodOnLguiVisualSizeChanged, SendMessageOptions.DontRequireReceiver);
            } else {
                EditorUtility.SetDirty (obj);
            }
        }

        public static void SetLabelWidth (float width) {
            EditorGUIUtility.labelWidth = width;
        }
    }

    class SearchWindow : EditorWindow {
        Action<string> _cb;

        GUIContent[] _foundItems;

        string[] _foundItemPaths;

        int _defaultID;

        bool _needToClose;

        public static void Open<T> (string title, string filter, T active, Action<string> cb) where T: UnityEngine.Object {
            var win = GetWindow <SearchWindow> ();
            win.minSize = new Vector2 (800, 600);
            win.titleContent = new GUIContent (title);
            win._cb = cb;
            win.Search<T> (filter, active);
            win.ShowUtility ();
        }

        void Update () {
            if (_needToClose) {
                Close ();
                return;
            }
        }

        void OnGUI () {
            if (_foundItems != null) {
                var id = GUILayout.SelectionGrid (_defaultID, _foundItems, 3);
                if (id != _defaultID) {
                    Finish (_foundItemPaths[id]);
                }
            }
        }

        void OnLostFocus () {
            Finish (null);
        }

        void Finish (string path) {
            if (_cb != null) {
                _cb (path);
                _cb = null;
            }
            _needToClose = true;
            Repaint ();
        }

        void Search<T> (string filter, T active) where T: UnityEngine.Object {
            var prefabs = AssetDatabase.FindAssets (filter);
            var nameList = new List<GUIContent> ();
            var pathList = new List<string> ();
            _defaultID = -1;
            nameList.Add (new GUIContent ("None"));
            pathList.Add (string.Empty);
            foreach (var item in prefabs) {
                var path = AssetDatabase.GUIDToAssetPath (item);
                var asset = AssetDatabase.LoadAssetAtPath<T> (path);
                if (asset != null) {
                    nameList.Add (new GUIContent (asset.name, AssetDatabase.GetCachedIcon (path)));
                    pathList.Add (path);
                    if (asset == active) {
                        _defaultID = nameList.Count - 1;
                    }
                }
            }
            _foundItems = nameList.ToArray ();
            _foundItemPaths = pathList.ToArray ();
        }
    }
}