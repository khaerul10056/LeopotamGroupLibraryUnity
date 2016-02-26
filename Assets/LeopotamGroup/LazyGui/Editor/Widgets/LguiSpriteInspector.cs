//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System;
using LeopotamGroup.LazyGui.Core;
using LeopotamGroup.LazyGui.UnityEditors;
using UnityEditor;
using UnityEngine;

namespace LeopotamGroup.LazyGui.Widgets.UnityEditors {
    [CustomEditor (typeof (LguiSprite))]
    sealed class LguiSpriteInspector : Editor {
        public override void OnInspectorGUI () {
            serializedObject.Update ();
            var sprite = target as LguiSprite;

            var propSpriteAtlas = serializedObject.FindProperty ("_spriteAtlas");
            var propSpriteName = serializedObject.FindProperty ("_spriteName");
            var propType = serializedObject.FindProperty ("_spriteType");
            var atlasName = string.Format ("Atlas: <{0}>", sprite.SpriteAtlas != null ? sprite.SpriteAtlas.name : "Empty");
            if (GUILayout.Button (atlasName)) {
                SearchWindow.Open<LguiAtlas> ("Select atlas", "t:prefab", sprite.SpriteAtlas, assetPath => {
                    // If not canceled.
                    if (assetPath != null) {
                        // None.
                        if (assetPath == string.Empty) {
                            sprite.SpriteAtlas = null;
                            propSpriteName.stringValue = null;
                        } else {
                            sprite.SpriteAtlas = AssetDatabase.LoadAssetAtPath<LguiAtlas> (assetPath);
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

            var type = (SpriteType) propType.enumValueIndex;
            if (type != SpriteType.Simple) {
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
                Undo.RecordObject (sprite, "lguisprite.set-original-size");
                sprite.ResetSize ();
                needUpdate = true;
            }
            if (GUILayout.Button ("Align size to original")) {
                Undo.RecordObject (sprite, "lguisprite.align-original-size");
                sprite.AlignSizeToOriginal ();
                needUpdate = true;
            }

            EditorGUILayout.Separator ();

            EditorGUILayout.PropertyField (serializedObject.FindProperty ("_depth"));
            EditorGUILayout.PropertyField (serializedObject.FindProperty ("_color"));

            if (needUpdate || serializedObject.ApplyModifiedProperties () || (Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "UndoRedoPerformed")) {
                EditorIntegration.UpdateVisuals (target);
            }
        }

        public override bool HasPreviewGUI () {
            return true;
        }

        static readonly GUIStyle _inspectorLabel = new GUIStyle ()
        {
            alignment = TextAnchor.LowerCenter,
            fontSize = 18,
            normal = new GUIStyleState ()
            {
                textColor = Color.white
            }
        };

        public override void OnPreviewGUI (Rect r, GUIStyle background) {
            base.OnPreviewGUI (r, background);
            var sprite = target as LguiSprite;
            if (sprite.SpriteAtlas != null) {
                var sprData = sprite.SpriteAtlas.GetSpriteData (sprite.SpriteName);
                if (sprData != null) {
                    var c = r.center;
                    var size = Mathf.Min (r.width, r.height);
                    r.Set (c.x - size * 0.5f, c.y - size * 0.5f, size, size);
                    var uvRect = new Rect (sprData.CornerX, sprData.CornerY, sprData.CornerW, sprData.CornerH);
                    GUI.DrawTextureWithTexCoords (r, sprite.SpriteAtlas.ColorTexture, uvRect);

                    var caption = string.Format ("size: {0}", sprite.GetOriginalSize ());
                    GUI.color = Color.black;
                    GUI.Label (r, caption, _inspectorLabel);
                    r.position -= Vector2.one;
                    GUI.color = Color.white;
                    GUI.Label (r, caption, _inspectorLabel);
                }
            }
        }
    }
}