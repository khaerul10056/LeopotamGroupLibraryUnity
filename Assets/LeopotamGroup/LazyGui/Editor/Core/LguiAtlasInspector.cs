//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using LeopotamGroup.LazyGui.Core;
using LeopotamGroup.LazyGui.UnityEditors;
using LeopotamGroup.LazyGui.Widgets;
using UnityEditor;
using UnityEngine;

namespace LeopotamGroup.LazyGui.Core.UnityEditors {
    [CustomEditor (typeof (LguiAtlas))]
    sealed class LguiAtlasInspector : Editor {
        public override void OnInspectorGUI () {
            EditorGUILayout.HelpBox ("Use context menu at atlas asset for options.\n\n" +
            "Add suffix .slice_A_B_C_D to texture asset name for init slice borders for left (A), top (B), right (C) and bottom (D) sides.\n\n" +
            "All coords - in pixels from each side!\n\n" +
            "For example: button.sliced_10_5_10_5", MessageType.Info);

            EditorGUILayout.Separator ();

            serializedObject.Update ();
            EditorGUILayout.PropertyField (serializedObject.FindProperty ("ColorTexture"));
            EditorGUILayout.PropertyField (serializedObject.FindProperty ("AlphaTexture"));

            EditorGUILayout.Separator ();

            var atlas = target as LguiAtlas;
            EditorGUILayout.LabelField ("Sprites list", EditorStyles.boldLabel);
            foreach (var item in atlas.Sprites) {
                GUILayout.Box (item.Name, GUILayout.ExpandWidth (true));
            }

            if (serializedObject.ApplyModifiedProperties () || (Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "UndoRedoPerformed")) {
                EditorIntegration.UpdateVisuals (target);
            }
        }

        static readonly Regex _slicedMask = new Regex (".sliced_(?<left>\\d+)_(?<top>\\d+)_(?<right>\\d+)_(?<bottom>\\d+)");

        public static string BakeAtlas (LguiAtlas atlas) {
            if (!AssetDatabase.Contains (atlas)) {
                return "Atlas should be saved as asset";
            }
            var fileName = EditorUtility.OpenFolderPanel ("Select sprites folder", Application.dataPath, string.Empty);
            if (string.IsNullOrEmpty (fileName)) {
                return "Sprite folder not selected";
            }
            try {
                if (atlas.ColorTexture != null && AssetDatabase.Contains (atlas.ColorTexture)) {
                    var t = atlas.ColorTexture;
                    atlas.ColorTexture = null;
                    AssetDatabase.DeleteAsset (AssetDatabase.GetAssetPath (t));
                }
            } catch {
            }
            try {
                if (atlas.AlphaTexture != null && AssetDatabase.Contains (atlas.AlphaTexture)) {
                    var t = atlas.AlphaTexture;
                    atlas.AlphaTexture = null;
                    AssetDatabase.DeleteAsset (AssetDatabase.GetAssetPath (t));
                }
            } catch {
            }

            var sprites = new List<Texture2D> ();

            foreach (var spriteFileName in Directory.GetFiles (fileName, "*.png", SearchOption.TopDirectoryOnly)) {
                var spriteTex = new Texture2D (2, 2);
                spriteTex.LoadImage (File.ReadAllBytes (spriteFileName));
                spriteTex.Apply ();
                spriteTex.name = Path.GetFileNameWithoutExtension (spriteFileName);
                sprites.Add (spriteTex);
            }

            var atlasTex = new Texture2D (2, 2, TextureFormat.ARGB32, true);
            var rects = atlasTex.PackTextures (sprites.ToArray (), sprites.Count > 1 ? 2 : 0, 2048);

            var atlasColor = new Texture2D (atlasTex.width, atlasTex.height, TextureFormat.RGB24, false);
            var atlasAlpha = new Texture2D (atlasTex.width, atlasTex.height, TextureFormat.RGB24, false);
            var srcColors = atlasTex.GetPixels32 ();
            DestroyImmediate (atlasTex);
            atlasTex = null;

            var dstAlphas = new Color32[srcColors.Length];
            Color32 c;
            for (int i = 0; i < srcColors.Length; i++) {
                c = srcColors[i];
                dstAlphas[i] = new Color32 (c.a, c.a, c.a, 255);
            }
            atlasColor.SetPixels32 (srcColors);
            atlasAlpha.SetPixels32 (dstAlphas);

            try {
                var srcAtlasPath = AssetDatabase.GetAssetPath (atlas);
                var isRepeated = Path.GetFileNameWithoutExtension (srcAtlasPath).Contains (".repeated");
                EditorUtility.DisplayProgressBar ("Save and import atlas data", "Color data processing...", 1f);
                var atlasPath = Path.ChangeExtension (srcAtlasPath, "color.png");
                File.WriteAllBytes (Path.Combine (Path.Combine (Application.dataPath, ".."), atlasPath), atlasColor.EncodeToPNG ());
                DestroyImmediate (atlasColor);
                atlasTex = FixAtlasImport (atlasPath, isRepeated);

                EditorUtility.DisplayProgressBar ("Save and import atlas data", "Alpha data processing...", 1f);
                atlasPath = Path.ChangeExtension (srcAtlasPath, "alpha.png");
                File.WriteAllBytes (Path.Combine (Path.Combine (Application.dataPath, ".."), atlasPath), atlasAlpha.EncodeToPNG ());
                DestroyImmediate (atlasAlpha);
                var alphaT = FixAtlasImport (atlasPath, isRepeated);

                atlas.ColorTexture = atlasTex;
                atlas.AlphaTexture = alphaT;

                var atlasSprites = new List<SpriteData> (sprites.Count);
                float atlasWidth = atlasTex.width;
                float atlasHeight = atlasTex.height;
                for (int i = 0, iMax = sprites.Count; i < iMax; i++) {
                    var sprData = new SpriteData ();
                    var sprName = sprites[i].name;
                    // slicing
                    var match = _slicedMask.Match (sprName.ToLowerInvariant ());
                    if (match.Success) {
                        sprName = sprName.Replace (match.Value, string.Empty);
                        sprData.BorderL = int.Parse (match.Groups["left"].Value) / atlasWidth;
                        sprData.BorderT = int.Parse (match.Groups["top"].Value) / atlasHeight;
                        sprData.BorderR = int.Parse (match.Groups["right"].Value) / atlasWidth;
                        sprData.BorderB = int.Parse (match.Groups["bottom"].Value) / atlasHeight;
                    } else {
                        sprData.BorderL = 0;
                        sprData.BorderT = 0;
                        sprData.BorderR = 0;
                        sprData.BorderB = 0;
                    }

                    sprData.Name = sprName;
                    sprData.CornerX =rects[i].x;
                    sprData.CornerY =rects[i].y;
                    sprData.CornerW =rects[i].width;
                    sprData.CornerH =rects[i].height;
                    atlasSprites.Add (sprData);
                }

                atlas.Sprites = atlasSprites.ToArray ();

                for (var i = sprites.Count - 1; i >= 0; i--) {
                    DestroyImmediate (sprites[i]);
                    sprites.RemoveAt (i);
                }

                EditorUtility.SetDirty (atlas);

                AssetDatabase.SaveAssets ();
                AssetDatabase.Refresh ();

                atlas.RefreshCache ();

                foreach (var vis in FindObjectsOfType <LguiVisualBase>()) {
                    vis.AddVisualChanges (ChangeType.Geometry);
                    EditorUtility.SetDirty (vis);
                }
            } finally {
                EditorUtility.ClearProgressBar ();
            }

            return null;
        }

        static Texture2D FixAtlasImport (string atlasPath, bool isRepeated) {
            AssetDatabase.ImportAsset (atlasPath, ImportAssetOptions.ForceUpdate);
            var atlasTex = AssetDatabase.LoadAssetAtPath<Texture2D> (atlasPath);
            var splatSettings = AssetImporter.GetAtPath (atlasPath) as TextureImporter;
            splatSettings.wrapMode = isRepeated ? TextureWrapMode.Repeat : TextureWrapMode.Clamp;
            splatSettings.anisoLevel = 2;
            splatSettings.filterMode = FilterMode.Bilinear;
            splatSettings.alphaIsTransparency = true;
            splatSettings.textureFormat = TextureImporterFormat.AutomaticCompressed;
            splatSettings.compressionQuality = 100;
            splatSettings.mipmapEnabled = false;
            splatSettings.textureType = TextureImporterType.Advanced;
            AssetDatabase.ImportAsset (atlasPath, ImportAssetOptions.ForceUpdate);
            return atlasTex;
        }
    }
}