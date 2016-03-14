//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System;
using UnityEngine;
using System.Collections.Generic;

namespace LeopotamGroup.LazyGui.Core {
    public static class LguiTextTools {
        static Vector3 _v0;

        static Vector3 _v1;

        static Vector3 _v2;

        static Vector3 _v3;

        static UIVertex _uiV;

        static Color _c;

        static Vector2 _uv0;

        static Vector2 _uv1;

        static Vector2 _uv2;

        static Vector2 _uv3;

        static readonly List<UIVertex> _verts = new List<UIVertex> (4096);

        static TextGenerationSettings _settings = new TextGenerationSettings
        {
            fontStyle = FontStyle.Normal,
            pivot = new Vector2 (0.5f, 0.5f),
            richText = true,
            resizeTextMinSize = 1,
            scaleFactor = 1f,
            horizontalOverflow = HorizontalWrapMode.Wrap,
            verticalOverflow = VerticalWrapMode.Truncate,
            generateOutOfBounds = false,
            updateBounds = false,
            resizeTextForBestFit = true,
            alignByGeometry = false
        };

        static readonly TextGenerator _generator = new TextGenerator ();

        public static void FillText (Mesh mesh, int width, int height, string text, Color color, TextAnchor align, Font font, int fontSize, float lineHgt, SpriteEffect effect = SpriteEffect.None, Vector2? effectValue = null, Color? effectColor = null) {
            if (mesh == null) {
                return;
            }
            mesh.Clear ();
            if (font == null || string.IsNullOrEmpty (text)) {
                return;
            }
            _settings.textAnchor = align;
            _settings.font = font;
            _settings.fontSize = fontSize;
            _settings.resizeTextMaxSize = fontSize;
            _settings.generationExtents = new Vector2 (width, height);
            _settings.color = color;
            _settings.lineSpacing = lineHgt;
            _generator.Invalidate ();
            if (!_generator.Populate (text, _settings)) {
                return;
            }

            _generator.GetVertices (_verts);

            LguiMeshTools.PrepareBuffer (effect, effectValue, effectColor);

            for (int i = 0, iMax = _verts.Count - 4; i < iMax;) {
                _uiV = _verts[i++];
                _c = _uiV.color;
                _v0 = _uiV.position;
                _uv0 = _uiV.uv0;

                _uiV = _verts[i++];
                _v1 = _uiV.position;
                _uv1 = _uiV.uv0;

                _uiV = _verts[i++];
                _v2 = _uiV.position;
                _uv2 = _uiV.uv0;

                _uiV = _verts[i++];
                _v3 = _uiV.position;
                _uv3 = _uiV.uv0;
                LguiMeshTools.FillBuffer (ref _v0, ref _v1, ref _v2, ref _v3, ref _uv0, ref _uv1, ref _uv2, ref _uv3, ref _c);
            }

            LguiMeshTools.GetBuffers (mesh);
        }
    }
}