//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace LeopotamGroup.LazyGui.Core {
    public static class LguiMeshTools {
        static readonly List<Vector3> _cacheV = new List<Vector3> (4096);

        static readonly List<Vector2> _cacheUV = new List<Vector2> (4096);

        static readonly List<int> _cacheT = new List<int> (4096);

        static readonly List<Color> _cacheC = new List<Color> (4096);

        static Rect _vRect;

        static Rect _uvRect;

        static Rect _uvRect2;

        public static Mesh GetNewMesh () {
            var mesh = new Mesh ();
            mesh.MarkDynamic ();
            mesh.hideFlags = HideFlags.DontSave;
            return mesh;
        }

        public static void PrepareBuffer () {
            _cacheV.Clear ();
            _cacheUV.Clear ();
            _cacheC.Clear ();
            _cacheT.Clear ();
        }

        public static void FillBuffer (ref Rect v, ref Rect uv, ref Color color) {
            FillBufferT ();
            FillBufferVC (ref v, ref color);
            _cacheUV.Add (new Vector2 (uv.xMin, uv.yMin));
            _cacheUV.Add (new Vector2 (uv.xMin, uv.yMax));
            _cacheUV.Add (new Vector2 (uv.xMax, uv.yMax));
            _cacheUV.Add (new Vector2 (uv.xMax, uv.yMin));
        }

        public static void FillBuffer (ref Rect v, ref Vector2 uv0, ref Vector2 uv1, ref Vector2 uv2, ref Vector2 uv3, ref Color color) {
            FillBufferT ();
            FillBufferVC (ref v, ref color);
            _cacheUV.Add (uv0);
            _cacheUV.Add (uv1);
            _cacheUV.Add (uv2);
            _cacheUV.Add (uv3);
        }

        static void FillBufferVC (ref Rect v, ref Color color) {
            _cacheC.Add (color);
            _cacheC.Add (color);
            _cacheC.Add (color);
            _cacheC.Add (color);

            _cacheV.Add (new Vector3 (v.xMin, v.yMin, 0f));
            _cacheV.Add (new Vector3 (v.xMin, v.yMax, 0f));
            _cacheV.Add (new Vector3 (v.xMax, v.yMax, 0f));
            _cacheV.Add (new Vector3 (v.xMax, v.yMin, 0f));
        }

        static void FillBufferT () {
            var vOffset = _cacheV.Count;
            _cacheT.Add (vOffset);
            _cacheT.Add (vOffset + 1);
            _cacheT.Add (vOffset + 2);
            _cacheT.Add (vOffset + 2);
            _cacheT.Add (vOffset + 3);
            _cacheT.Add (vOffset);
        }

        public static void GetBuffers (Mesh mesh) {
            mesh.Clear (true);
            mesh.SetVertices (_cacheV);
            mesh.SetUVs (0, _cacheUV);
            mesh.SetColors (_cacheC);
            mesh.SetTriangles (_cacheT, 0);
            mesh.RecalculateBounds ();
        }

        public static void FillSimpleSprite (Mesh mesh, int width, int height, Color color, SpriteData spriteData) {
            if (mesh == null) {
                return;
            }
            PrepareBuffer ();
            if (spriteData != null) {
                var halfW = 0.5f * width;
                var halfH = 0.5f * height;
                _vRect.Set (-halfW, -halfH, width, height);
                FillBuffer (ref _vRect, ref spriteData.UV, ref color);
            }
            GetBuffers (mesh);
        }

        public static void FillSlicedTiledSprite (Mesh mesh, int width, int height, Color color, SpriteData spriteData, Vector2 texSize, bool isHorTiled, bool isVerTiled, bool fillCenter) {
            if (mesh == null) {
                return;
            }
            PrepareBuffer ();
            if (spriteData != null && width > 0 && height > 0) {
                var leftBorderV = (int) (spriteData.Borders.xMin * texSize.x);
                var rightBorderV = (int) (spriteData.Borders.xMax * texSize.x);
                var topBorderV = (int) (spriteData.Borders.yMax * texSize.y);
                var bottomBorderV = (int) (spriteData.Borders.yMin * texSize.y);

                var halfW = width >> 1;
                var halfH = height >> 1;

                int centerWidthV;
                int horTileCount;

                int centerHeightV;
                int verTileCount;

                if (isHorTiled) {
                    centerWidthV = (int) ((spriteData.UV.xMax - spriteData.UV.xMin - spriteData.Borders.xMax - spriteData.Borders.xMin) * texSize.x);
                    horTileCount = Mathf.Max (0, Mathf.FloorToInt ((width - leftBorderV - rightBorderV) / (float) centerWidthV));
                } else {
                    centerWidthV = 0;
                    horTileCount = 0;
                }

                if (isVerTiled) {
                    centerHeightV = (int) ((spriteData.UV.yMax - spriteData.UV.yMin - spriteData.Borders.yMax - spriteData.Borders.yMin) * texSize.y);
                    verTileCount = Mathf.Max (0, Mathf.FloorToInt ((height - bottomBorderV - topBorderV) / (float) centerHeightV));
                } else {
                    centerHeightV = 0;
                    verTileCount = 0;
                }

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
            }
            GetBuffers (mesh);
        }
    }
}