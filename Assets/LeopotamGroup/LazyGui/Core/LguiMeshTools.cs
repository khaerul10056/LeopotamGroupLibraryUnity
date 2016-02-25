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

        static readonly List<Color> _cacheC = new List<Color> (4096);

        static readonly List<int> _cacheT = new List<int> (4096);

        static Rect _vRect;

        static Rect _vRect2;

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
            if (mesh != null) {
                mesh.Clear (true);
                if (_cacheV.Count <= 65535) {
                    mesh.SetVertices (_cacheV);
                    mesh.SetUVs (0, _cacheUV);
                    mesh.SetColors (_cacheC);
                    mesh.SetTriangles (_cacheT, 0);
                } else {
                    Debug.LogWarning ("Too many vertices", mesh);
                }
                mesh.RecalculateBounds ();
            }
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
                _uvRect.Set (spriteData.CornerX, spriteData.CornerY, spriteData.CornerW, spriteData.CornerH);
                FillBuffer (ref _vRect, ref _uvRect, ref color);
            }
            GetBuffers (mesh);
        }

        public static void FillSlicedTiledSprite (
            Mesh mesh, int width, int height, Color color, SpriteData sd,
            Vector2 texSize, bool isHorTiled, bool isVerTiled, bool fillCenter) {
            if (mesh == null) {
                return;
            }
            PrepareBuffer ();
            if (sd != null && width > 0 && height > 0) {
                var leftBorderV = (int) (sd.BorderL * texSize.x);
                var rightBorderV = (int) (sd.BorderR * texSize.x);
                var topBorderV = (int) (sd.BorderT * texSize.y);
                var bottomBorderV = (int) (sd.BorderB * texSize.y);
                var cW = sd.CenterWidth;
                var cH = sd.CenterHeight;
                var bR = sd.CornerX + sd.CornerW - sd.BorderR;
                var bT = sd.CornerY + sd.CornerH - sd.BorderT;

                var halfW = width >> 1;
                var halfH = height >> 1;

                int centerWidthV;
                int horTileCount;

                int centerHeightV;
                int verTileCount;

                if (isHorTiled) {
                    centerWidthV = (int) (cW * texSize.x);
                    horTileCount = Mathf.Max (0, Mathf.FloorToInt ((width - leftBorderV - rightBorderV) / (float) centerWidthV));
                } else {
                    centerWidthV = width - rightBorderV - leftBorderV;
                    horTileCount = 1;
                }

                if (isVerTiled) {
                    centerHeightV = (int) (cH * texSize.y);
                    verTileCount = Mathf.Max (0, Mathf.FloorToInt ((height - bottomBorderV - topBorderV) / (float) centerHeightV));
                } else {
                    centerHeightV = height - topBorderV - bottomBorderV;
                    verTileCount = 1;
                }

                // top-bottom sides
                if (sd.BorderT > 0 || sd.BorderB > 0) {
                    _uvRect.Set (sd.CornerX + sd.BorderL, bT, cW, sd.BorderT);
                    _uvRect2.Set (sd.CornerX + sd.BorderL, sd.CornerY, cW, sd.BorderB);
                    _vRect.Set (-halfW + leftBorderV, halfH - topBorderV, centerWidthV, topBorderV);
                    _vRect2.Set (-halfW + leftBorderV, -halfH, centerWidthV, bottomBorderV);
                    for (var i = 0; i < horTileCount; i++) {
                        FillBuffer (ref _vRect, ref _uvRect, ref color);
                        FillBuffer (ref _vRect2, ref _uvRect2, ref color);
                        _vRect.x += centerWidthV;
                        _vRect2.x += centerWidthV;
                    }
                }

                // left-right sides
                if (sd.BorderL > 0 || sd.BorderR > 0) {
                    _uvRect.Set (sd.CornerX, sd.CornerY + sd.BorderB, sd.BorderL, cH);
                    _uvRect2.Set (bR, sd.CornerY + sd.BorderB, sd.BorderR, cH);
                    _vRect.Set (-halfW, -halfH + bottomBorderV, leftBorderV, centerHeightV);
                    _vRect2.Set (halfW - rightBorderV, -halfH + bottomBorderV, rightBorderV, centerHeightV);
                    for (var i = 0; i < verTileCount; i++) {
                        FillBuffer (ref _vRect, ref _uvRect, ref color);
                        FillBuffer (ref _vRect2, ref _uvRect2, ref color);
                        _vRect.y += centerHeightV;
                        _vRect2.y += centerHeightV;
                    }
                }

                // center
                if (fillCenter) {
                    _uvRect.Set (sd.CornerX + sd.BorderL, sd.CornerY + sd.BorderB, cW, cH);
                    _vRect.Set (0, -halfH + bottomBorderV, centerWidthV, centerHeightV);
                    for (var y = 0; y < verTileCount; y++) {
                        _vRect.x = -halfW + leftBorderV;
                        for (var x = 0; x < horTileCount; x++) {
                            FillBuffer (ref _vRect, ref _uvRect, ref color);
                            _vRect.x += centerWidthV;
                        }
                        _vRect.y += centerHeightV;
                    }
                }

                // left-top corner
                if (sd.BorderL > 0 && sd.BorderT > 0) {
                    _vRect.Set (-halfW, halfH - topBorderV, leftBorderV, topBorderV);
                    _uvRect.Set (sd.CornerX, bT, sd.BorderL, sd.BorderT);
                    FillBuffer (ref _vRect, ref _uvRect, ref color);
                }

                // right-top corner
                if (sd.BorderR > 0 && sd.BorderT > 0) {
                    _vRect.Set (halfW - rightBorderV, halfH - topBorderV, rightBorderV, topBorderV);
                    _uvRect.Set (bR, bT, sd.BorderR, sd.BorderT);
                    FillBuffer (ref _vRect, ref _uvRect, ref color);
                }

                // right-bottom corner
                if (sd.BorderR > 0 && sd.BorderB > 0) {
                    _vRect.Set (halfW - rightBorderV, -halfH, rightBorderV, bottomBorderV);
                    _uvRect.Set (bR, sd.CornerY, sd.BorderR, sd.BorderB);
                    FillBuffer (ref _vRect, ref _uvRect, ref color);
                }

                // left-bottom corner
                if (sd.BorderL > 0 && sd.BorderB > 0) {
                    _vRect.Set (-halfW, -halfH, leftBorderV, bottomBorderV);
                    _uvRect.Set (sd.CornerX, sd.CornerY, sd.BorderL, sd.BorderB);
                    FillBuffer (ref _vRect, ref _uvRect, ref color);
                }
            }
            GetBuffers (mesh);
        }
    }
}