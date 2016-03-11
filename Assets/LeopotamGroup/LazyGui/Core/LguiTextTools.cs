//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System;
using UnityEngine;

namespace LeopotamGroup.LazyGui.Core {
    public static class LguiTextTools {
        static Rect _vRect;

        static Vector2 _uv0;

        static Vector2 _uv1;

        static Vector2 _uv2;

        static Vector2 _uv3;

        public static void FillText (Mesh mesh, int width, int height, Color color, Alignment align, Font font, string text, int fontSize, float lineHgt) {
            if (mesh == null) {
                return;
            }
            mesh.Clear ();
            if (font == null || string.IsNullOrEmpty (text)) {
                return;
            }

            font.RequestCharactersInTexture (text, fontSize, FontStyle.Normal);

            var textLength = text.Length;

            LguiMeshTools.PrepareBuffer ();

            CharacterInfo ch;

            var lineHeight = (int)(fontSize * lineHgt);

            // TODO: Parsing bb-codes
            var style = FontStyle.Normal;

            int lineWidth;
            int j;
            int len;

            var line = 0;
            var i = 0;
            var pos = Vector3.zero;

            while (true) {
                if (i >= textLength) {
                    LguiMeshTools.GetBuffers (mesh);
                    return;
                }
                len = text.IndexOf ('\n', i);
                len = len == -1 ? textLength - i : len - i;

                // Calculate line width.
                lineWidth = 0;
                for (j = 0; j < len; j++) {
                    font.GetCharacterInfo (text[i + j], out ch, fontSize, style);
                    lineWidth += ch.advance;
                }

                pos.y = -line * lineHeight;
                switch (align) {
                    case Alignment.Center:
                        pos.x = -lineWidth * 0.5f;
                        break;
                    case Alignment.Right:
                        pos.x = width * 0.5f - lineWidth;
                        break;
                    default:
                        pos.x = -0.5f * width;
                        break;
                }
                for (j = 0; j < len; j++) {
                    font.GetCharacterInfo (text[i], out ch, fontSize, style);
                    _uv0 = ch.uvBottomLeft;
                    _uv1 = ch.uvTopLeft;
                    _uv2 = ch.uvTopRight;
                    _uv3 = ch.uvBottomRight;
                    _vRect.Set (pos.x + ch.minX, pos.y + ch.minY, ch.glyphWidth, ch.glyphHeight);
                    LguiMeshTools.FillBuffer (ref _vRect, ref _uv0, ref _uv1, ref _uv2, ref _uv3, ref color);
                    pos.x += ch.advance;
                    i++;
                }
                i++;
                line++;
            }
        }
    }
}