//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System;
using UnityEngine;

namespace LeopotamGroup.LazyGui.Core {
    public static class LguiMeshTools {
        // MaxLength = 2 ^ BufferAmount
        const int BufferAmount = 14;

        static int _activeRectID;

        static Vector3[] _activeV;

        static Vector2[] _activeUV;

        static Color[] _activeC;

        static int[] _activeT;

        static Rect _vRect;

        static Rect _uvRect;

        static Rect _uvRect2;

        static readonly Vector3[][] _cacheV;

        static readonly Vector2[][] _cacheUV;

        static readonly Color[][] _cacheC;

        static readonly int[][] _cacheT;

        static readonly Vector3 _zeroV = Vector3.zero;

        static LguiMeshTools () {
            _cacheV = new Vector3[BufferAmount][];
            _cacheUV = new Vector2[BufferAmount][];
            _cacheC = new Color[BufferAmount][];
            _cacheT = new int[BufferAmount][];

            int count;
            for (int i = 0, len = 1; i < BufferAmount; i++, len <<= 1) {
                count = len << 2;
                _cacheV[i] = new Vector3[count];
                _cacheUV[i] = new Vector2[count];
                _cacheC[i] = new Color[count];
                var tris = new int[len * 6];

                for (int t = 0, tOffset = 0, vOffset = 0; t < len; t++, vOffset += 4) {
                    tris[tOffset++] = vOffset;
                    tris[tOffset++] = vOffset + 1;
                    tris[tOffset++] = vOffset + 2;
                    tris[tOffset++] = vOffset + 2;
                    tris[tOffset++] = vOffset + 3;
                    tris[tOffset++] = vOffset;
                }
                _cacheT[i] = tris;
            }
        }

        public static Mesh GetNewMesh () {
            var mesh = new Mesh ();
            mesh.hideFlags = HideFlags.DontSave;
            return mesh;
        }

        public static bool PrepareBuffer (int count) {
            _activeRectID = 0;

            for (int i = 0, maxValue = 1; i < BufferAmount; i++, maxValue += maxValue) {
                if (count <= maxValue) {
                    _activeV = _cacheV[i];
                    _activeUV = _cacheUV[i];
                    _activeC = _cacheC[i];
                    _activeT = _cacheT[i];
                    return true;
                }
            }
            _activeT = null;
            return false;
        }

        public static bool FillBuffer (ref Rect v, ref Rect uv, ref Color color) {
            if (_activeT != null) {
                var offset = _activeRectID << 2;
                _activeV[offset] = new Vector3 (v.xMin, v.yMin, 0f);
                _activeUV[offset] = new Vector2 (uv.xMin, uv.yMin);
                _activeC[offset++] = color;
                _activeV[offset] = new Vector3 (v.xMin, v.yMax, 0f);
                _activeUV[offset] = new Vector2 (uv.xMin, uv.yMax);
                _activeC[offset++] = color;
                _activeV[offset] = new Vector3 (v.xMax, v.yMax, 0f);
                _activeUV[offset] = new Vector2 (uv.xMax, uv.yMax);
                _activeC[offset++] = color;
                _activeV[offset] = new Vector3 (v.xMax, v.yMin, 0f);
                _activeUV[offset] = new Vector2 (uv.xMax, uv.yMin);
                _activeC[offset++] = color;
                _activeRectID++;
                return true;
            }
            return false;
        }

        public static bool FillBuffer (ref Rect v, ref Vector2 uv0, ref Vector2 uv1, ref Vector2 uv2, ref Vector2 uv3, ref Color color) {
            if (_activeT != null) {
                var offset = _activeRectID << 2;
                _activeV[offset] = new Vector3 (v.xMin, v.yMin, 0f);
                _activeUV[offset] = uv0;
                _activeC[offset++] = color;
                _activeV[offset] = new Vector3 (v.xMin, v.yMax, 0f);
                _activeUV[offset] = uv1;
                _activeC[offset++] = color;
                _activeV[offset] = new Vector3 (v.xMax, v.yMax, 0f);
                _activeUV[offset] = uv2;
                _activeC[offset++] = color;
                _activeV[offset] = new Vector3 (v.xMax, v.yMin, 0f);
                _activeUV[offset] = uv3;
                _activeC[offset++] = color;
                _activeRectID++;
                return true;
            }
            return false;
        }

        public static void GetBuffers (Mesh mesh) {
            if (_activeT == null || mesh == null) {
                return;
            }

            for (int i = _activeRectID * 4; i < _activeV.Length; i++) {
                _activeV[i] = _zeroV;
            }
            mesh.MarkDynamic ();
            mesh.vertices = _activeV;
            mesh.uv = _activeUV;
            mesh.colors = _activeC;
            mesh.triangles = _activeT;
            mesh.RecalculateBounds ();
        }

        public static void FillSimpleSprite (Mesh mesh, int width, int height, Color color, SpriteData spriteData) {
            if (mesh == null) {
                return;
            }
            mesh.Clear ();
            if (spriteData == null) {
                return;
            }

            var halfW = 0.5f * width;
            var halfH = 0.5f * height;

            PrepareBuffer (1);
            _vRect.Set (-halfW, -halfH, width, height);
            FillBuffer (ref _vRect, ref spriteData.UV, ref color);

            GetBuffers (mesh);
        }

        public static void FillSlicedTiledSprite (Mesh mesh, int width, int height, Color color, SpriteData spriteData, Vector2 texSize, bool isHorTiled, bool isVerTiled, bool fillCenter) {
            if (mesh == null) {
                return;
            }
            mesh.Clear ();
            if (spriteData == null || width == 0 || height == 0) {
                return;
            }

            var leftBorderV = (int) (spriteData.Borders.xMin * texSize.x);
            var rightBorderV = (int) (spriteData.Borders.xMax * texSize.x);
            var topBorderV = (int) (spriteData.Borders.yMax * texSize.y);
            var bottomBorderV = (int) (spriteData.Borders.yMin * texSize.y);

            var halfW = (int) (0.5f * width);
            var halfH = (int) (0.5f * height);

            var rectCount = 4;

            int centerWidthV;
            int horTileCount;

            int centerHeightV;
            int verTileCount;

            if (isHorTiled) {
                centerWidthV = (int) ((spriteData.UV.xMax - spriteData.UV.xMin - spriteData.Borders.xMax - spriteData.Borders.xMin) * texSize.x);
                horTileCount = Mathf.Max (0, Mathf.FloorToInt ((width - leftBorderV - rightBorderV) / (float) centerWidthV));
                rectCount += horTileCount * 2;
            } else {
                centerWidthV = 0;
                horTileCount = 0;
                rectCount += 2;
            }

            if (isVerTiled) {
                centerHeightV = (int) ((spriteData.UV.yMax - spriteData.UV.yMin - spriteData.Borders.yMax - spriteData.Borders.yMin) * texSize.y);
                verTileCount = Mathf.Max (0, Mathf.FloorToInt ((height - bottomBorderV - topBorderV) / (float) centerHeightV));
                rectCount += verTileCount * 2;
            } else {
                centerHeightV = 0;
                verTileCount = 0;
                rectCount += 2;
            }

            // center
            if (fillCenter) {
                if (isHorTiled && isVerTiled) {
                    rectCount += horTileCount * verTileCount;
                } else {
                    rectCount++;
                }
            }

            PrepareBuffer (rectCount);

            // top-bottom sides
            _uvRect.Set (spriteData.UV.xMin + spriteData.Borders.xMin, spriteData.UV.yMax - spriteData.Borders.yMax, spriteData.UV.width - spriteData.Borders.xMin - spriteData.Borders.xMax, spriteData.Borders.yMax);
            _uvRect2.Set (spriteData.UV.xMin + spriteData.Borders.xMin, spriteData.UV.yMin, spriteData.UV.width - spriteData.Borders.xMin - spriteData.Borders.xMax, spriteData.Borders.yMin);
            if (isHorTiled) {
                for (var i = 0; i < horTileCount; i++) {
                    _vRect.Set (-halfW + leftBorderV + i * centerWidthV, halfH - topBorderV, centerWidthV, topBorderV);
                    FillBuffer (ref _vRect, ref _uvRect, ref color);
                    _vRect.Set (-halfW + leftBorderV + i * centerWidthV, -halfH, centerWidthV, bottomBorderV);
                    FillBuffer (ref _vRect, ref _uvRect2, ref color);
                }
            } else {
                _vRect.Set (-halfW + leftBorderV, halfH - topBorderV, width - rightBorderV - leftBorderV, topBorderV);
                FillBuffer (ref _vRect, ref _uvRect, ref color);
                _vRect.Set (-halfW + leftBorderV, -halfH, width - rightBorderV - leftBorderV, bottomBorderV);
                FillBuffer (ref _vRect, ref _uvRect2, ref color);
            }

            // left-right sides
            _uvRect.Set (spriteData.UV.xMin, spriteData.UV.yMin + spriteData.Borders.yMin, spriteData.Borders.xMin, spriteData.UV.height - spriteData.Borders.yMin - spriteData.Borders.yMax);
            _uvRect2.Set (spriteData.UV.xMax - spriteData.Borders.xMax, spriteData.UV.yMin + spriteData.Borders.yMin, spriteData.Borders.xMax, spriteData.UV.height - spriteData.Borders.yMin - spriteData.Borders.yMax);
            if (isVerTiled) {
                for (var i = 0; i < verTileCount; i++) {
                    _vRect.Set (-halfW, -halfH + bottomBorderV + i * centerHeightV, leftBorderV, centerHeightV);
                    FillBuffer (ref _vRect, ref _uvRect, ref color);
                    _vRect.Set (halfW - rightBorderV, -halfH + bottomBorderV + i * centerHeightV, rightBorderV, centerHeightV);
                    FillBuffer (ref _vRect, ref _uvRect2, ref color);
                }
            } else {
                _vRect.Set (-halfW, -halfH + bottomBorderV, leftBorderV, height - bottomBorderV - topBorderV);
                FillBuffer (ref _vRect, ref _uvRect, ref color);
                _vRect.Set (halfW - rightBorderV, -halfH + bottomBorderV, rightBorderV, height - bottomBorderV - topBorderV);
                FillBuffer (ref _vRect, ref _uvRect2, ref color);
            }

            // center
            if (fillCenter) {
                var rectCenter = new Rect (spriteData.UV.xMin + spriteData.Borders.xMin, spriteData.UV.yMin + spriteData.Borders.yMin, spriteData.UV.width - spriteData.Borders.xMin - spriteData.Borders.xMax, spriteData.UV.height - spriteData.Borders.yMin - spriteData.Borders.yMax);
                if (isHorTiled && isVerTiled) {
                    for (var y = 0; y < verTileCount; y++) {
                        for (var x = 0; x < horTileCount; x++) {
                            _vRect.Set (-halfW + leftBorderV + x * centerWidthV, -halfH + bottomBorderV + y * centerHeightV, centerWidthV, centerHeightV);
                            FillBuffer (ref _vRect, ref rectCenter, ref color);
                        }
                    }
                } else {
                    _vRect.Set (-halfW + leftBorderV, -halfH + bottomBorderV, width - rightBorderV - leftBorderV, height - topBorderV - bottomBorderV);
                    FillBuffer (ref _vRect, ref rectCenter, ref color);
                }
            }

            // left-top corner
            _vRect.Set (-halfW, halfH - topBorderV, leftBorderV, topBorderV);
            _uvRect.Set (spriteData.UV.xMin, spriteData.UV.yMax - spriteData.Borders.yMax, spriteData.Borders.xMin, spriteData.Borders.yMax);
            FillBuffer (ref _vRect, ref _uvRect, ref color);

            // right-top corner
            _vRect.Set (halfW - rightBorderV, halfH - topBorderV, rightBorderV, topBorderV);
            _uvRect.Set (spriteData.UV.xMax - spriteData.Borders.xMax, spriteData.UV.yMax - spriteData.Borders.yMax, spriteData.Borders.xMax, spriteData.Borders.yMax);
            FillBuffer (ref _vRect, ref _uvRect, ref color);

            // right-bottom corner
            _vRect.Set (halfW - rightBorderV, -halfH, rightBorderV, bottomBorderV);
            _uvRect.Set (spriteData.UV.xMax - spriteData.Borders.xMax, spriteData.UV.yMin, spriteData.Borders.xMax, spriteData.Borders.yMin);
            FillBuffer (ref _vRect, ref _uvRect, ref color);

            // left-bottom corner
            _vRect.Set (-halfW, -halfH, leftBorderV, bottomBorderV);
            _uvRect.Set (spriteData.UV.xMin, spriteData.UV.yMin, spriteData.Borders.xMin, spriteData.Borders.yMin);
            FillBuffer (ref _vRect, ref _uvRect, ref color);

            GetBuffers (mesh);
        }
    }
}