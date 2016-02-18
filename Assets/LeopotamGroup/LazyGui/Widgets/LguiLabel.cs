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
    public class LguiLabel : LguiVisualBase {
        public Font Font {
            get { return _font; }
            set {
                if (value != _font) {
                    _font = value;
                    AddVisualChanges (ChangeType.Geometry);
                }
            }
        }

        public int FontSize {
            get { return _fontSize; }
            set {
                if (Mathf.Abs (value) != _fontSize) {
                    _fontSize = Mathf.Abs (value);
                    AddVisualChanges (ChangeType.Geometry);
                }
            }
        }

        public FontStyle FontStyle {
            get { return _fontStyle; }
            set {
                if (value != _fontStyle) {
                    _fontStyle = value;
                    AddVisualChanges (ChangeType.Geometry);
                }
            }
        }

        public string Text {
            get { return _text; }
            set {
                if (value != _text) {
                    _text = value;
                    AddVisualChanges (ChangeType.Geometry);
                }
            }
        }

        public Alignment Alignment {
            get { return _alignment; }
            set {
                if (value != _alignment) {
                    _alignment = value;
                    AddVisualChanges (ChangeType.Geometry);
                }
            }
        }

        public float LineHeight {
            get { return _lineHeight; }
            set {
                if (LineHeight > 0 && System.Math.Abs (value - _lineHeight) > 0f) {
                    _lineHeight = value;
                    AddVisualChanges (ChangeType.Geometry);
                }
            }
        }

        [HideInInspector]
        [SerializeField]
        Alignment _alignment = Alignment.Center;

        [HideInInspector]
        [SerializeField]
        Font _font;

        [HideInInspector]
        [SerializeField]
        int _fontSize = 32;

        [HideInInspector]
        [SerializeField]
        FontStyle _fontStyle = FontStyle.Normal;

        [Multiline (5)]
        [HideInInspector]
        [SerializeField]
        string _text;

        [HideInInspector]
        [SerializeField]
        float _lineHeight = 2f;

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

            _meshRenderer = GetComponent<MeshRenderer> ();
            _meshRenderer.hideFlags = HideFlags.HideInInspector;

            AddVisualChanges (ChangeType.All);

            Font.textureRebuilt += OnFontTextureRebuilt;
        }

        void OnDisable () {
            Font.textureRebuilt -= OnFontTextureRebuilt;
            _meshRenderer.enabled = false;
            _meshFilter = null;
            _meshRenderer = null;
            _visualPanel = null;
        }

        protected override bool UpdateVisuals (ChangeType changes) {
            if (!base.UpdateVisuals (changes)) {
                return false;
            }

            if ((changes & (ChangeType.Geometry | ChangeType.Panel | ChangeType.Color)) != ChangeType.None) {
                if (Font != null) {
                    _meshRenderer.sharedMaterial = _visualPanel.GetFontMaterial (Font);

                    if ((changes & (ChangeType.Geometry | ChangeType.Color)) != ChangeType.None) {
                        LguiTextTools.FillText (_meshFilter.sharedMesh, Width, Height, Color, Alignment, Font, Text, FontSize, FontStyle, LineHeight);
                    }
                }
            }
            return true;
        }

        void OnFontTextureRebuilt (Font changedFont) {
            if (changedFont == Font) {
                AddVisualChanges (ChangeType.All);
            }
        }
    }
}