//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using LeopotamGroup.Gui.Common;
using LeopotamGroup.Gui.Common.UnityEditors;
using LeopotamGroup.Gui.Layout;
using LeopotamGroup.Gui.Widgets;
using UnityEditor;
using UnityEngine;

namespace LeopotamGroup.Gui.UnityEditors {
    [InitializeOnLoad]
    public static class EditorIntegration {
        static EditorIntegration () {
            EditorApplication.hierarchyWindowItemOnGUI += OnDrawHierarchyItemIcon;
        }

        public static bool IsUndo () {
            return Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "UndoRedoPerformed";
        }

        static Texture2D _whiteTexture;

        static readonly Color _panelColor = new Color (1f, 0.5f, 0.5f);

        static readonly Color _receiverColor = new Color (0.5f, 1f, 0.5f);

        static readonly Color _spriteColor = new Color (0.5f, 1f, 1f);

        static readonly Color _labelColor = new Color (1f, 1f, 0.5f);

        static void OnDrawHierarchyItemIcon (int instanceID, Rect selectionRect) {
            try {
                GuiWidget w;
                GameObject obj;
                if (Event.current.type == EventType.DragPerform) {
                    foreach (var go in DragAndDrop.objectReferences) {
                        obj = go as GameObject;
                        if (obj != null && obj.activeSelf) {
                            foreach (var item in obj.GetComponentsInChildren <GuiWidget> ()) {
                                item.ResetPanel ();
                            }
                        }
                    }
                    return;
                }

                obj = EditorUtility.InstanceIDToObject (instanceID) as GameObject;
                if (obj != null) {
                    var panel = obj.GetComponent <GuiPanel> ();
                    if (panel != null) {
                        DrawHierarchyLabel (selectionRect, "PNL: " + panel.Depth, Color.black, _panelColor);
                    } else {
                        var receiver = obj.GetComponent <GuiEventReceiver> ();
                        if (receiver != null) {
                            DrawHierarchyLabel (selectionRect, "RCV: " + receiver.Depth, Color.black, _receiverColor);
                        } else {
                            w = obj.GetComponent <GuiWidget> ();
                            if (w != null) {
                                if (w as GuiSprite) {
                                    DrawHierarchyLabel (selectionRect, "SPR: " + w.Depth, Color.black, _spriteColor);
                                }
                                if (w is GuiLabel) {
                                    DrawHierarchyLabel (selectionRect, "LBL: " + w.Depth, Color.black, _labelColor);
                                }

                            }
                        }
                    }
                }
            } catch (Exception ex) {
                Debug.LogWarning (ex);
            }
        }

        static void DrawHierarchyLabel (Rect rect, string text, Color textColor, Color backColor) {
            if (_whiteTexture == null) {
                _whiteTexture = new Texture2D (1, 1, TextureFormat.RGB24, false);
                _whiteTexture.hideFlags = HideFlags.HideAndDontSave;
                _whiteTexture.SetPixel (0, 0, Color.white);
                _whiteTexture.Apply (false);
            }
            rect.x += rect.width - 50;
            var oldColor = GUI.color;
            GUI.color = backColor;
            GUI.DrawTexture (rect, _whiteTexture, ScaleMode.StretchToFill);
            GUI.color = textColor;
            EditorGUI.LabelField (rect, text);
            GUI.color = oldColor;
        }

        [MenuItem ("GameObject/LeopotamGroup.Gui/GuiSystem", false, 1)]
        static void CreateGuiSystem () {
            GuiSystem.Instance.Validate ();
        }

        [MenuItem ("GameObject/LeopotamGroup.Gui/Widgets/Sprite", false, 1)]
        static void CreateWidgetSprite () {
            SearchWindow.Open<GuiAtlas> ("Select atlas", "t:prefab", null, assetPath => {
                var spr = WidgetFactory.CreateWidgetSprite ();
                Undo.RegisterCreatedObjectUndo (spr.gameObject, "leopotamgroup.gui.create-sprite");
                if (!string.IsNullOrEmpty (assetPath)) {
                    spr.SpriteAtlas = AssetDatabase.LoadAssetAtPath<GuiAtlas> (assetPath);
                    var sprNames = spr.SpriteAtlas.GetSpriteNames ();
                    spr.SpriteName = sprNames != null && sprNames.Length > 0 ? sprNames[0] : null;
                    spr.ResetSize ();
                }
                FixWidgetParent (spr);
                UpdateVisuals (spr);
            });
        }

        [MenuItem ("GameObject/LeopotamGroup.Gui/Widgets/Label", false, 1)]
        static void CreateWidgetLabel () {
            SearchWindow.Open<Font> ("Select font", "t:font", null, assetPath => {
                var label = WidgetFactory.CreateWidgetLabel ();
                Undo.RegisterCreatedObjectUndo (label.gameObject, "leopotamgroup.gui.create-label");
                label.Font = string.IsNullOrEmpty (assetPath) ?
                    Resources.GetBuiltinResource<Font> ("Arial.ttf") : AssetDatabase.LoadAssetAtPath<Font> (assetPath);
                FixWidgetParent (label);
                UpdateVisuals (label);
            });
        }

        [MenuItem ("GameObject/LeopotamGroup.Gui/Widgets/Button", false, 1)]
        static void CreateWidgetButton () {
            SearchWindow.Open<GuiAtlas> ("Select atlas", "t:prefab", null, assetPath => {
                var button = WidgetFactory.CreateWidgetButton ();
                Undo.RegisterCreatedObjectUndo (button.gameObject, "leopotamgroup.gui.create-btn");
                FixWidgetParent (button);
                if (!string.IsNullOrEmpty (assetPath)) {
                    var spr = button.GetComponentInChildren<GuiSprite> ();
                    spr.SpriteAtlas = AssetDatabase.LoadAssetAtPath<GuiAtlas> (assetPath);
                    var sprNames = spr.SpriteAtlas.GetSpriteNames ();
                    spr.SpriteName = sprNames != null && sprNames.Length > 0 ? sprNames[0] : null;
                    spr.ResetSize ();
                    button.Width = spr.Width;
                    button.Height = spr.Height;
                    UpdateVisuals (spr);
                }
                UpdateVisuals (button);
            });
        }

        [MenuItem ("GameObject/LeopotamGroup.Gui/Layout/Panel", false, 1)]
        static void CreateWidgetPanel () {
            FixWidgetParent (WidgetFactory.CreateWidgetPanel ());
        }

        [MenuItem ("GameObject/LeopotamGroup.Gui/Layout/BindPosition", false, 1)]
        static void CreateLayoutBindPosition () {
            var bind = WidgetFactory.CreateLayoutBindPosition (Selection.activeGameObject);
            Undo.RegisterCreatedObjectUndo (bind.gameObject, "leopotamgroup.gui.create-bind-pos");
            FixWidgetParent (bind);
        }

        [MenuItem ("GameObject/LeopotamGroup.Gui/Layout/BindSize", false, 1)]
        static void CreateLayoutBindSize () {
            var bind = WidgetFactory.CreateLayoutBindSize (Selection.activeGameObject);
            Undo.RegisterCreatedObjectUndo (bind.gameObject, "leopotamgroup.gui.create-bind-size");
            FixWidgetParent (bind);
        }

        static void FixWidgetParent (MonoBehaviour widget) {
            if (Selection.activeGameObject != null && !AssetDatabase.Contains (Selection.activeGameObject)) {
                widget.transform.SetParent (Selection.activeGameObject.transform, false);
            }
            Selection.activeGameObject = widget.gameObject;
        }

        [MenuItem ("Assets/LeopotamGroup.Gui/Create new atlas asset", false, 1)]
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
                name = "GuiAtlas";
            }

            var asset = new GameObject ();
            asset.AddComponent <GuiAtlas> ();
            PrefabUtility.CreatePrefab (AssetDatabase.GenerateUniqueAssetPath (string.Format ("{0}/{1}.prefab", path, name)), asset);
            UnityEngine.Object.DestroyImmediate (asset);
            AssetDatabase.Refresh ();
        }

        [MenuItem ("GameObject/LeopotamGroup.Gui/Layout/BindPosition", true)]
        static bool CanCreateLayoutBindPosition () {
            return Selection.activeGameObject != null && !AssetDatabase.Contains (Selection.activeGameObject);
        }

        [MenuItem ("GameObject/LeopotamGroup.Gui/Layout/BindSize", true)]
        static bool CanCreateLayoutBindSize () {
            return Selection.activeGameObject != null && !AssetDatabase.Contains (Selection.activeGameObject);
        }

        [MenuItem ("Assets/LeopotamGroup.Gui/Update atlas from folder...", true)]
        static bool CanBeAutoPacked () {
            var sel = Selection.activeGameObject;
            return sel != null && AssetDatabase.Contains (sel) && sel.GetComponent <GuiAtlas> () != null;
        }

        [MenuItem ("Assets/LeopotamGroup.Gui/Update atlas from folder...", false, 1)]
        static void UpdateAtlas () {
            var res = GuiAtlasInspector.BakeAtlas (Selection.activeGameObject.GetComponent <GuiAtlas> ());
            EditorUtility.DisplayDialog ("Atlas autopacker", res ?? "Completed", "Close");
        }

        public static void UpdateVisuals (UnityEngine.Object obj) {
            if (obj == null) {
                return;
            }
            var widget = obj as GuiWidget;
            if (widget != null) {
                widget.UpdateVisuals (GuiDirtyType.All);
//                widget.SendMessage (GuiConsts.MethodOnLguiVisualSizeChanged, SendMessageOptions.DontRequireReceiver);
            } else {
                var panel = obj as GuiPanel;
                if (panel != null) {
                    panel.UpdateVisuals ();
                } else {
//                    Debug.LogWarning ("Updating non-gui object", obj);
                    EditorUtility.SetDirty (obj);
                }
            }
            EditorApplication.RepaintHierarchyWindow ();
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
            var pos = win.position;
            pos.position = (new Vector2 (Screen.currentResolution.width, Screen.currentResolution.height) - win.minSize) * 0.5f;
            win.position = pos; 
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