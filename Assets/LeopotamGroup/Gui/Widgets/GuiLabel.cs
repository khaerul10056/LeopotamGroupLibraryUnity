//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using LeopotamGroup.Gui.Common;
using UnityEngine;

namespace LeopotamGroup.Gui.Widgets {
    /// <summary>
    /// Label widget.
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent (typeof (MeshFilter))]
    [RequireComponent (typeof (MeshRenderer))]
    public sealed class GuiLabel : GuiWidget {
        /// <summary>
        /// Font of label.
        /// </summary>
        public Font Font {
            get { return _font; }
            set {
                if (value != _font) {
                    _font = value;
                    SetDirty (GuiDirtyType.Geometry);
                }
            }
        }

        /// <summary>
        /// Font size of label.
        /// </summary>
        public int FontSize {
            get { return _fontSize; }
            set {
                if (Mathf.Abs (value) != _fontSize) {
                    _fontSize = Mathf.Abs (value);
                    SetDirty (GuiDirtyType.Geometry);
                }
            }
        }

        /// <summary>
        /// Text of label.
        /// </summary>
        public string Text {
            get { return _text; }
            set {
                if (value != _text) {
                    _text = value;
                    SetDirty (GuiDirtyType.Geometry);
                }
            }
        }

        /// <summary>
        /// Text alignment.
        /// </summary>
        public TextAnchor Alignment {
            get { return _alignment; }
            set {
                if (value != _alignment) {
                    _alignment = value;
                    SetDirty (GuiDirtyType.Geometry);
                }
            }
        }

        /// <summary>
        /// Overloading font line height.
        /// </summary>
        public float LineHeight {
            get { return _lineHeight; }
            set {
                if (LineHeight > 0 && System.Math.Abs (value - _lineHeight) > 0f) {
                    _lineHeight = value;
                    SetDirty (GuiDirtyType.Geometry);
                }
            }
        }

        /// <summary>
        /// Effect of label rendering.
        /// </summary>
        public GuiFontEffect Effect {
            get { return _effect; }
            set {
                if (_effect != value) {
                    _effect = value;
                    SetDirty (GuiDirtyType.Geometry);
                }
            }
        }

        /// <summary>
        /// Parameters of rendering effect.
        /// </summary>
        public Vector2 EffectValue {
            get { return _effectValue; }
            set {
                if (_effectValue != value) {
                    _effectValue = value;
                    SetDirty (GuiDirtyType.Geometry);
                }
            }
        }

        /// <summary>
        /// Color of rendering effect.
        /// </summary>
        public Color EffectColor {
            get { return _effectColor; }
            set {
                if (_effectColor != value) {
                    _effectColor = value;
                    SetDirty (GuiDirtyType.Geometry);
                }
            }
        }

        [HideInInspector]
        [SerializeField]
        TextAnchor _alignment = TextAnchor.MiddleCenter;

        [HideInInspector]
        [SerializeField]
        Font _font;

        [HideInInspector]
        [SerializeField]
        int _fontSize = 32;

        [Multiline (5)]
        [HideInInspector]
        [SerializeField]
        string _text;

        [HideInInspector]
        [SerializeField]
        float _lineHeight = 1f;

        [HideInInspector]
        [SerializeField]
        GuiFontEffect _effect = GuiFontEffect.None;

        [HideInInspector]
        [SerializeField]
        Vector2 _effectValue = Vector2.one;

        [HideInInspector]
        [SerializeField]
        Color _effectColor = Color.black;

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

            _meshRenderer = GetComponent<MeshRenderer> ();
            _meshRenderer.hideFlags = HideFlags.HideInInspector;

            SetDirty (GuiDirtyType.All);

            Font.textureRebuilt += OnFontTextureRebuilt;
        }

        protected override void OnDisable () {
            Font.textureRebuilt -= OnFontTextureRebuilt;
            _meshRenderer.enabled = false;
            _meshFilter = null;
            _meshRenderer = null;
            base.OnDisable ();
        }

        void OnFontTextureRebuilt (Font changedFont) {
            if (changedFont == Font) {
                SetDirty (GuiDirtyType.All);
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
                if (Font != null) {
                    _meshRenderer.sharedMaterial = _visualPanel.GetFontMaterial (Font);

                    if ((changes & GuiDirtyType.Geometry) != GuiDirtyType.None) {
                        GuiTextTools.FillText (_meshFilter.sharedMesh, Width, Height, Text, Color, Alignment, Font, FontSize, LineHeight, _effect, _effectValue, _effectColor);
                    }
                }
            }
            return true;
        }
    }
}