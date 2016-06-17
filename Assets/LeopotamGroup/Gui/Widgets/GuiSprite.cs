//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using LeopotamGroup.Gui.Common;
using LeopotamGroup.Math;
using UnityEngine;

namespace LeopotamGroup.Gui.Widgets {
    /// <summary>
    /// Sprite widget.
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent (typeof (MeshFilter))]
    [RequireComponent (typeof (MeshRenderer))]
    public sealed class GuiSprite : GuiWidget {
        /// <summary>
        /// Sprite atlas.
        /// </summary>
        /// <value>The sprite atlas.</value>
        public GuiAtlas SpriteAtlas {
            get { return _spriteAtlas; }
            set {
                if (value != _spriteAtlas) {
                    _spriteAtlas = value;
                    SetDirty (GuiDirtyType.Geometry);
                }
            }
        }

        /// <summary>
        /// Sprite name inside GuiAtlas.
        /// </summary>
        /// <value>The name of the sprite.</value>
        public string SpriteName {
            get { return _spriteName; }
            set {
                if (value != _spriteName) {
                    _spriteName = value;
                    SetDirty (GuiDirtyType.Geometry);
                }
            }
        }

        /// <summary>
        /// Rendering type of sprite.
        /// </summary>
        /// <value>The type of the sprite.</value>
        public GuiSpriteType SpriteType {
            get { return _spriteType; }
            set {
                if (value != _spriteType) {
                    _spriteType = value;
                    SetDirty (GuiDirtyType.Geometry);
                }
            }
        }

        /// <summary>
        /// Is center part will be rendered for non "Simple" sprite type.
        /// </summary>
        /// <value><c>true</c> if this instance is sprite center filled; otherwise, <c>false</c>.</value>
        public bool IsSpriteCenterFilled {
            get { return _isSpriteCenterFilled; }
            set {
                if (value != _isSpriteCenterFilled) {
                    _isSpriteCenterFilled = value;
                    SetDirty (GuiDirtyType.Geometry);
                }
            }
        }

        [HideInInspector]
        [SerializeField]
        GuiAtlas _spriteAtlas;

        [HideInInspector]
        [SerializeField]
        string _spriteName;

        [HideInInspector]
        [SerializeField]
        GuiSpriteType _spriteType = GuiSpriteType.Simple;

        [HideInInspector]
        [SerializeField]
        bool _isSpriteCenterFilled = true;

        MeshFilter _meshFilter;

        protected override void Awake () {
            base.Awake ();
            _meshFilter = GetComponent<MeshFilter> ();
            _meshFilter.sharedMesh = null;
        }

        protected override void OnEnable () {
            base.OnEnable ();
            if (_meshFilter == null) {
                _meshFilter = GetComponent<MeshFilter> ();
            }
            _meshFilter.hideFlags = HideFlags.HideInInspector;

            if (_meshFilter.sharedMesh == null) {
                _meshFilter.sharedMesh = GuiMeshTools.GetNewMesh ();
            }

            _meshRenderer = GetComponent <MeshRenderer> ();
            _meshRenderer.hideFlags = HideFlags.HideInInspector;
        }

        protected override void OnDisable () {
            _meshRenderer.enabled = false;
            _meshFilter = null;
            _meshRenderer = null;
            base.OnDisable ();
        }

        /// <summary>
        /// Get original sprite size from GuiAtlas.
        /// </summary>
        /// <returns>The original size.</returns>
        public Vector2i GetOriginalSize () {
            if (SpriteAtlas != null && !string.IsNullOrEmpty (SpriteName)) {
                var sprData = SpriteAtlas.GetSpriteData (SpriteName);
                return new Vector2i (
                    (int) (sprData.CornerW * SpriteAtlas.ColorTexture.width),
                    (int) (sprData.CornerH * SpriteAtlas.ColorTexture.height));
            }
            return Vector2i.zero;
        }

        /// <summary>
        /// Reset sprite size to original at GuiAtlas.
        /// </summary>
        public void ResetSize () {
            var size = GetOriginalSize ();
            if (size != Vector2i.zero) {
                Width = size.x;
                Height = size.y;
            }
        }

        /// <summary>
        /// Align tiled sprites for 100% filling internal part.
        /// </summary>
        public void AlignSizeToOriginal () {
            if (SpriteAtlas != null && !string.IsNullOrEmpty (SpriteName)) {
                var sprData = SpriteAtlas.GetSpriteData (SpriteName);
                int srcWidthBorder;
                int srcWidthCenter;
                int srcHeightBorder;
                int srcHeightCenter;
                switch (SpriteType) {
                    case GuiSpriteType.TiledHorizontal:
                        srcWidthBorder = (int) ((sprData.BorderL + sprData.BorderR) * SpriteAtlas.ColorTexture.width);
                        srcWidthCenter = (int) (sprData.CenterWidth * SpriteAtlas.ColorTexture.width);
                        Width = Mathf.RoundToInt ((Width - srcWidthBorder) / (float) srcWidthCenter) * srcWidthCenter + srcWidthBorder;
                        break;
                    case GuiSpriteType.TiledVertical:
                        srcHeightBorder = (int) ((sprData.BorderT + sprData.BorderB) * SpriteAtlas.ColorTexture.height);
                        srcHeightCenter = (int) (sprData.CenterHeight * SpriteAtlas.ColorTexture.height);
                        Height = Mathf.RoundToInt ((Height - srcHeightBorder) / (float) srcHeightCenter) * srcHeightCenter + srcHeightBorder;
                        break;
                    case GuiSpriteType.TiledBoth:
                        srcWidthBorder = (int) ((sprData.BorderL + sprData.BorderR) * SpriteAtlas.ColorTexture.width);
                        srcHeightBorder = (int) ((sprData.BorderT + sprData.BorderB) * SpriteAtlas.ColorTexture.height);
                        srcWidthCenter = (int) (sprData.CenterWidth * SpriteAtlas.ColorTexture.width);
                        srcHeightCenter = (int) (sprData.CenterHeight * SpriteAtlas.ColorTexture.height);
                        Width = Mathf.RoundToInt ((Width - srcWidthBorder) / (float) srcWidthCenter) * srcWidthCenter + srcWidthBorder;
                        Height = Mathf.RoundToInt ((Height - srcHeightBorder) / (float) srcHeightCenter) * srcHeightCenter + srcHeightBorder;
                        break;
                    default:
                        var srcWidth = (int) (sprData.CornerW * SpriteAtlas.ColorTexture.width);
                        var srcHeight = (int) (sprData.CornerH * SpriteAtlas.ColorTexture.height);
                        Width = Mathf.RoundToInt (Width / (float) srcWidth) * srcWidth;
                        Height = Mathf.RoundToInt (Height / (float) srcHeight) * srcHeight;
                        break;
                }
            }
        }

        /// <summary>
        /// Force revalidate visuals.
        /// </summary>
        /// <returns>true, if visuals were updated.</returns>
        /// <param name="changes">What should be revalidate.</param>
        public override bool UpdateVisuals (GuiDirtyType changes) {
            if (!base.UpdateVisuals (changes)) {
                return false;
            }

            if ((changes & (GuiDirtyType.Geometry | GuiDirtyType.Panel)) != GuiDirtyType.None) {
                if (SpriteAtlas != null && SpriteAtlas.ColorTexture != null) {
                    _meshRenderer.sharedMaterial = _visualPanel.GetMaterial (SpriteAtlas);
                    if ((changes & GuiDirtyType.Geometry) != GuiDirtyType.None) {
                        var sprData = SpriteAtlas.GetSpriteData (SpriteName);
                        if (SpriteType == GuiSpriteType.Simple) {
                            GuiMeshTools.FillSimpleSprite (_meshFilter.sharedMesh, Width, Height, Color, sprData);
                        } else {
                            var texSize = new Vector2 (SpriteAtlas.ColorTexture.width, SpriteAtlas.ColorTexture.height);
                            var isHorTiled = SpriteType == GuiSpriteType.TiledBoth || SpriteType == GuiSpriteType.TiledHorizontal;
                            var isVerTiled = SpriteType == GuiSpriteType.TiledBoth || SpriteType == GuiSpriteType.TiledVertical;
                            GuiMeshTools.FillSlicedTiledSprite (
                                _meshFilter.sharedMesh, Width, Height, Color, sprData,
                                texSize, isHorTiled, isVerTiled, IsSpriteCenterFilled);
                        }
                    }
                } else {
                    _meshFilter.sharedMesh.Clear (true);
                }
            }
            return true;
        }
    }
}