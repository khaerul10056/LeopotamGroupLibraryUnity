//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System;
using LeopotamGroup.Gui.Common;
using LeopotamGroup.Gui.UnityEditors;
using UnityEditor;
using UnityEngine;

namespace LeopotamGroup.Gui.Widgets.UnityEditors {
    [CanEditMultipleObjects]
    [CustomEditor (typeof (GuiSprite))]
    sealed class GuiSpriteInspector : Editor {
        public override void OnInspectorGUI () {
            serializedObject.Update ();
            var sprite = target as GuiSprite;

            var propSpriteAtlas = serializedObject.FindProperty ("_spriteAtlas");
            var propSpriteName = serializedObject.FindProperty ("_spriteName");
            var propType = serializedObject.FindProperty ("_spriteType");
            var atlasName = string.Format ("Atlas: <{0}>", sprite.SpriteAtlas != null ? sprite.SpriteAtlas.name : "Empty");
            if (GUILayout.Button (atlasName)) {
                SearchWindow.Open<GuiAtlas> ("Select atlas", "t:prefab", sprite.SpriteAtlas, assetPath => {
                    // If not canceled.
                    if (assetPath != null) {
                        // None.
                        if (assetPath == string.Empty) {
                            sprite.SpriteAtlas = null;
                            propSpriteName.stringValue = null;
                        } else {
                            sprite.SpriteAtlas = AssetDatabase.LoadAssetAtPath<GuiAtlas> (assetPath);
                        }
                        propSpriteAtlas.objectReferenceValue = sprite.SpriteAtlas;
                    }
                });
            }

            if (sprite.SpriteAtlas != null) {
                var spriteList = sprite.SpriteAtlas.GetSpriteNames ();
                var id = Array.IndexOf (spriteList, sprite.SpriteName);
                id = EditorGUILayout.Popup ("Sprite", id, spriteList);
                if (id >= 0 && id < spriteList.Length) {
                    propSpriteName.stringValue = spriteList[id];
                }
            }


            EditorGUILayout.PropertyField (propType, new GUIContent ("Type"));

            var type = (GuiSpriteType) propType.enumValueIndex;
            if (type != GuiSpriteType.Simple) {
                EditorGUILayout.PropertyField (serializedObject.FindProperty ("_isSpriteCenterFilled"), new GUIContent ("Fill center"));
            }

            EditorGUILayout.Separator ();

            var widthProp = serializedObject.FindProperty ("_width");
            var heightProp = serializedObject.FindProperty ("_height");
            EditorGUILayout.PropertyField (widthProp);
            if (widthProp.intValue < 0) {
                widthProp.intValue = 0;
            }

            EditorGUILayout.PropertyField (heightProp);
            if (heightProp.intValue < 0) {
                heightProp.intValue = 0;
            }

            bool needUpdate = false;
            if (GUILayout.Button ("Reset size to original")) {
                Undo.RecordObject (sprite, "leopotamgroup.gui.sprite.set-original-size");
                sprite.ResetSize ();
                needUpdate = true;
            }
            if (GUILayout.Button ("Align size to original")) {
                Undo.RecordObject (sprite, "leopotamgroup.gui.sprite.align-original-size");
                sprite.AlignSizeToOriginal ();
                needUpdate = true;
            }

            GuiWidget s;

            if (GUILayout.Button ("Bake scale to widget size")) {
                Undo.RecordObject (sprite, "leopotamgroup.gui.sprite.bake-scale-size");
                foreach (var item in targets) {
                    s = item as GuiSprite;
                    if (s != null) {
                        s.BakeScale ();
                    }
                }
                SceneView.RepaintAll ();
            }

            EditorGUILayout.Separator ();

            EditorGUILayout.IntSlider (serializedObject.FindProperty ("_depth"), -49, 49);
            EditorGUILayout.PropertyField (serializedObject.FindProperty ("_color"));

            if (needUpdate || serializedObject.ApplyModifiedProperties () || EditorIntegration.IsUndo ()) {
                EditorIntegration.UpdateVisuals (target);
            }
        }

        public override bool HasPreviewGUI () {
            return true;
        }

        static readonly GUIStyle _textStyle = new GUIStyle ()
        {
            alignment = TextAnchor.LowerCenter,
            fontSize = 16,
            normal = new GUIStyleState ()
            {
                textColor = Color.white
            }
        };

        public override void OnPreviewGUI (Rect r, GUIStyle background) {
            base.OnPreviewGUI (r, background);
            var sprite = target as GuiSprite;
            if (sprite.SpriteAtlas != null) {
                var sprData = sprite.SpriteAtlas.GetSpriteData (sprite.SpriteName);
                if (sprData != null) {
                    var c = r.center;
                    var size = Mathf.Min (r.width, r.height);
                    r.Set (c.x - size * 0.5f, c.y - size * 0.5f, size, size);
                    var uvRect = new Rect (sprData.CornerX, sprData.CornerY, sprData.CornerW, sprData.CornerH);
                    GUI.DrawTextureWithTexCoords (r, sprite.SpriteAtlas.ColorTexture, uvRect);

                    var caption = sprite.GetOriginalSize ().ToString ();
                    GUI.color = Color.black;
                    GUI.Label (r, caption, _textStyle);
                    var r2 = r;
                    r2.position -= Vector2.one;
                    GUI.color = Color.yellow;
                    GUI.Label (r2, caption, _textStyle);

                    Handles.color = Color.yellow;
                    var offset = r.x + r.width * sprData.BorderL / sprData.CornerW;
                    Handles.DrawAAPolyLine (4f, new Vector3 (offset, r.y), new Vector3 (offset, r.yMax));
                    offset = r.xMax - r.width * sprData.BorderR / sprData.CornerW;
                    Handles.DrawAAPolyLine (4f, new Vector3 (offset, r.y), new Vector3 (offset, r.yMax));

                    offset = r.y + r.height * sprData.BorderT / sprData.CornerH;
                    Handles.DrawAAPolyLine (4f, new Vector3 (r.x, offset), new Vector3 (r.xMax, offset));
                    offset = r.yMax - r.height * sprData.BorderB / sprData.CornerH;
                    Handles.DrawAAPolyLine (4f, new Vector3 (r.x, offset), new Vector3 (r.xMax, offset));
                }
            }
        }
    }
}