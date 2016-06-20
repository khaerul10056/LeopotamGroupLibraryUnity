//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace LeopotamGroup.Gui.Common {
    /// <summary>
    /// Text tools helper.
    /// </summary>
    public static class GuiTextTools {
        static Vector3 _v0;

        static Vector3 _v1;

        static Vector3 _v2;

        static Vector3 _v3;

        static UIVertex _uiV;

        static Vector2 _uv0;

        static Vector2 _uv1;

        static Vector2 _uv2;

        static Vector2 _uv3;

        static Color _c;

        static readonly List<UIVertex> _verts = new List<UIVertex> (4096);

        static TextGenerationSettings _settings = new TextGenerationSettings
        {
            fontStyle = FontStyle.Normal,
            pivot = new Vector2 (0.5f, 0.5f),
            richText = true,
            scaleFactor = 1f,
            verticalOverflow = VerticalWrapMode.Truncate,
            horizontalOverflow = HorizontalWrapMode.Wrap,
            generateOutOfBounds = false,
            updateBounds = false,
            resizeTextMinSize = 0,
            resizeTextForBestFit = false,
            alignByGeometry = false
        };

        static readonly TextGenerator _generator = new TextGenerator ();

        /// <summary>
        /// Fill label mesh.
        /// </summary>
        /// <param name="mesh">Mesh.</param>
        /// <param name="width">Label width.</param>
        /// <param name="height">Label height.</param>
        /// <param name="text">Label text.</param>
        /// <param name="color">Color.</param>
        /// <param name="align">Text align.</param>
        /// <param name="font">Font.</param>
        /// <param name="fontSize">Font size.</param>
        /// <param name="lineHgt">Line height multiplier.</param>
        /// <param name="effect">Effect.</param>
        /// <param name="effectValue">Effect value.</param>
        /// <param name="effectColor">Effect color.</param>
        public static void FillText (Mesh mesh, int width, int height, string text, Color color, TextAnchor align, Font font, int fontSize, float lineHgt, GuiFontEffect effect = GuiFontEffect.None, Vector2? effectValue = null, Color? effectColor = null) {
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
            _settings.lineSpacing = lineHgt;
            _settings.color = color;
            _generator.Invalidate ();
            if (!_generator.Populate (text, _settings)) {
                return;
            }

            _generator.GetVertices (_verts);

            GuiMeshTools.PrepareBuffer (effect, effectValue, effectColor);

            for (int i = 0, iMax = _verts.Count - 4, charID = 0; i < iMax; charID++) {
                if (text[charID] == ' ') {
                    i += 4;
                    continue;
                }
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
                GuiMeshTools.FillBuffer (ref _v0, ref _v1, ref _v2, ref _v3, ref _uv0, ref _uv1, ref _uv2, ref _uv3, ref _c);
            }

            GuiMeshTools.GetBuffers (mesh, false);
            mesh.bounds = new Bounds (Vector3.zero, new Vector3 (width, height, 0f));
        }
    }
}