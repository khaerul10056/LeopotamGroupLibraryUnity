//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using LeopotamGroup.LazyGui.Core;
using UnityEngine;

namespace LeopotamGroup.LazyGui.Widgets {
    [ExecuteInEditMode]
    [RequireComponent (typeof (MeshFilter))]
    [RequireComponent (typeof (MeshRenderer))]
    public class LguiSprite : LguiVisualBase {
        public LguiAtlas SpriteAtlas {
            get { return _spriteAtlas; }
            set {
                if (value != _spriteAtlas) {
                    _spriteAtlas = value;
                    AddVisualChanges (ChangeType.Geometry);
                }
            }
        }

        public string SpriteName {
            get { return _spriteName; }
            set {
                if (value != _spriteName) {
                    _spriteName = value;
                    AddVisualChanges (ChangeType.Geometry);
                }
            }
        }

        public SpriteType SpriteType {
            get { return _spriteType; }
            set {
                if (value != _spriteType) {
                    _spriteType = value;
                    AddVisualChanges (ChangeType.Geometry);
                }
            }
        }

        public bool IsSpriteCenterFilled {
            get { return _isSpriteCenterFilled; }
            set {
                if (value != _isSpriteCenterFilled) {
                    _isSpriteCenterFilled = value;
                    AddVisualChanges (ChangeType.Geometry);
                }
            }
        }

        [HideInInspector]
        [SerializeField]
        LguiAtlas _spriteAtlas;

        [HideInInspector]
        [SerializeField]
        string _spriteName;

        [HideInInspector]
        [SerializeField]
        SpriteType _spriteType = SpriteType.Simple;

        [HideInInspector]
        [SerializeField]
        bool _isSpriteCenterFilled = true;

        MeshFilter _meshFilter;

        void Awake () {
            _meshFilter = GetComponent<MeshFilter> ();
            _meshFilter.sharedMesh = null;
        }

        void OnEnable () {
            if (_meshFilter == null) {
                _meshFilter = GetComponent<MeshFilter> ();
            }
            _meshFilter.hideFlags = HideFlags.HideInInspector;

            if (_meshFilter.sharedMesh == null) {
                _meshFilter.sharedMesh = LguiMeshTools.GetNewMesh ();
            }

            _meshRenderer = GetComponent <MeshRenderer> ();
            _meshRenderer.hideFlags = HideFlags.HideInInspector;

            AddVisualChanges (ChangeType.All);
        }

        void OnDisable () {
            _meshRenderer.enabled = false;
            _meshFilter = null;
            _meshRenderer = null;
            _visualPanel = null;
        }

        public void SetSizeToOriginal () {
            if (SpriteAtlas != null && !string.IsNullOrEmpty (SpriteName)) {
                var sprData = SpriteAtlas.GetSpriteData (SpriteName);
                Width = (int) (sprData.UV.width * SpriteAtlas.ColorTexture.width);
                Height = (int) (sprData.UV.height * SpriteAtlas.ColorTexture.height);
            }
        }

        public void AlignSizeToOriginal () {
            if (SpriteAtlas != null && !string.IsNullOrEmpty (SpriteName)) {
                var sprData = SpriteAtlas.GetSpriteData (SpriteName);
                int srcWidthBorder;
                int srcWidthCenter;
                int srcHeightBorder;
                int srcHeightCenter;
                switch (SpriteType) {
                    case SpriteType.TiledHorizontal:
                        srcWidthBorder = (int) ((sprData.Borders.xMin + sprData.Borders.xMax) * SpriteAtlas.ColorTexture.width);
                        srcWidthCenter = (int) ((sprData.UV.width - sprData.Borders.xMin - sprData.Borders.xMax) * SpriteAtlas.ColorTexture.width);
                        Width = Mathf.RoundToInt ((Width - srcWidthBorder) / (float) srcWidthCenter) * srcWidthCenter + srcWidthBorder;
                        break;
                    case SpriteType.TiledVertical:
                        srcHeightBorder = (int) ((sprData.Borders.yMin + sprData.Borders.yMax) * SpriteAtlas.ColorTexture.height);
                        srcHeightCenter = (int) ((sprData.UV.height - sprData.Borders.yMin - sprData.Borders.yMax) * SpriteAtlas.ColorTexture.height);
                        Height = Mathf.RoundToInt ((Height - srcHeightBorder) / (float) srcHeightCenter) * srcHeightCenter + srcHeightBorder;
                        break;
                    case SpriteType.TiledBoth:
                        srcWidthBorder = (int) ((sprData.Borders.xMin + sprData.Borders.xMax) * SpriteAtlas.ColorTexture.width);
                        srcHeightBorder = (int) ((sprData.Borders.yMin + sprData.Borders.yMax) * SpriteAtlas.ColorTexture.height);
                        srcWidthCenter = (int) ((sprData.UV.width - sprData.Borders.xMin - sprData.Borders.xMax) * SpriteAtlas.ColorTexture.width);
                        srcHeightCenter = (int) ((sprData.UV.height - sprData.Borders.yMin - sprData.Borders.yMax) * SpriteAtlas.ColorTexture.height);
                        Width = Mathf.RoundToInt ((Width - srcWidthBorder) / (float) srcWidthCenter) * srcWidthCenter + srcWidthBorder;
                        Height = Mathf.RoundToInt ((Height - srcHeightBorder) / (float) srcHeightCenter) * srcHeightCenter + srcHeightBorder;
                        break;
                    default:
                        var srcWidth = (int) (sprData.UV.width * SpriteAtlas.ColorTexture.width);
                        var srcHeight = (int) (sprData.UV.height * SpriteAtlas.ColorTexture.height);
                        Width = Mathf.RoundToInt (Width / (float) srcWidth) * srcWidth;
                        Height = Mathf.RoundToInt (Height / (float) srcHeight) * srcHeight;
                        break;
                }
            }
        }

        protected override bool UpdateVisuals (ChangeType changes) {
            if (!base.UpdateVisuals (changes)) {
                return false;
            }

            if ((changes & (ChangeType.Geometry | ChangeType.Panel | ChangeType.Color)) != ChangeType.None) {
                if (SpriteAtlas != null && SpriteAtlas.ColorTexture != null) {
                    _meshRenderer.sharedMaterial = _visualPanel.GetMaterial (SpriteAtlas);
                    if ((changes & (ChangeType.Geometry | ChangeType.Color)) != ChangeType.None) {
                        var sprData = SpriteAtlas.GetSpriteData (SpriteName);
                        var texSize = new Vector2 (SpriteAtlas.ColorTexture.width, SpriteAtlas.ColorTexture.height);
                        switch (SpriteType) {
                            case SpriteType.Sliced:
                                LguiMeshTools.FillSlicedTiledSprite (_meshFilter.sharedMesh, Width, Height, Color, sprData, texSize, false, false, IsSpriteCenterFilled);
                                break;
                            case SpriteType.TiledBoth:
                                LguiMeshTools.FillSlicedTiledSprite (_meshFilter.sharedMesh, Width, Height, Color, sprData, texSize, true, true, IsSpriteCenterFilled);
                                break;
                            case SpriteType.TiledHorizontal:
                                LguiMeshTools.FillSlicedTiledSprite (_meshFilter.sharedMesh, Width, Height, Color, sprData, texSize, true, false, IsSpriteCenterFilled);
                                break;
                            case SpriteType.TiledVertical:
                                LguiMeshTools.FillSlicedTiledSprite (_meshFilter.sharedMesh, Width, Height, Color, sprData, texSize, false, true, IsSpriteCenterFilled);
                                break;
                            default:
                                LguiMeshTools.FillSimpleSprite (_meshFilter.sharedMesh, Width, Height, Color, sprData);
                                break;
                        }
                    }
                } else {
                    _meshFilter.sharedMesh.Clear ();
                }
            }
            return true;
        }
    }
}