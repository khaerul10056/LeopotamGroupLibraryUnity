//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System;
using LeopotamGroup.Common;
using LeopotamGroup.Gui.Common;
using LeopotamGroup.Gui.Layout;
using UnityEngine;

namespace LeopotamGroup.Gui.Widgets {
    /// <summary>
    /// Base gui widget.
    /// </summary>
    [ExecuteInEditMode]
    public abstract class GuiWidget : MonoBehaviourBase {
        /// <summary>
        /// Will be raised on each geometry update.
        /// </summary>
        public event Action<GuiWidget> OnGeometryUpdated = delegate {};

        /// <summary>
        /// Cached dirty state.
        /// </summary>
        /// <value>The state of the dirty.</value>
        public GuiDirtyType DirtyState { get; private set; }

        /// <summary>
        /// Enable dirty state for specified types.
        /// </summary>
        /// <param name="changes">Specified dirty types.</param>
        public virtual void SetDirty (GuiDirtyType changes) {
            DirtyState |= changes;
        }

        /// <summary>
        /// Color of widget.
        /// </summary>
        public Color Color {
            get { return _color; }
            set {
                if (value != _color) {
                    _color = value;
                    SetDirty (GuiDirtyType.Geometry);
                }
            }
        }

        /// <summary>
        /// Width of widget.
        /// </summary>
        public int Width {
            get { return _width; }
            set {
                if (value >= 0 && value != _width) {
                    _width = value;
                    SetDirty (GuiDirtyType.Geometry);
                }
            }
        }

        /// <summary>
        /// height of widget.
        /// </summary>
        public int Height {
            get { return _height; }
            set {
                if (value >= 0 && value != _height) {
                    _height = value;
                    SetDirty (GuiDirtyType.Geometry);
                }
            }
        }

        /// <summary>
        /// Order of widgets inside GuiPanel for proper sorting. Positive values brings panel closer to camera.
        /// </summary>
        public int Depth {
            get { return _depth; }
            set {
                if (value != _depth) {
                    _depth = value;
                    SetDirty (GuiDirtyType.Depth);
                }
            }
        }

        /// <summary>
        /// Is visual widget visible.
        /// </summary>
        public bool IsVisible { get { return _meshRenderer != null && _meshRenderer.enabled; } }

        const float DepthSlice = 0.5f;

        [HideInInspector]
        [SerializeField]
        int _width = 64;

        [HideInInspector]
        [SerializeField]
        int _height = 64;

        [HideInInspector]
        [SerializeField]
        Color _color = Color.white;

        [HideInInspector]
        [SerializeField]
        int _depth;

        Bounds _bounds;

        protected GuiPanel _visualPanel;

        protected MeshRenderer _meshRenderer;

        protected virtual void OnEnable () {
            SetDirty (GuiDirtyType.All);
        }

        protected virtual void OnDisable () {
            ResetPanel ();
        }

        void OnPanelChanged (GuiPanel panel) {
            if (_visualPanel != panel) {
                ResetPanel ();
            }
        }

        void LateUpdate () {
            if (DirtyState != GuiDirtyType.None) {
                var changes = DirtyState;
                DirtyState = GuiDirtyType.None;
                UpdateVisuals (changes);
            }
        }

        /// <summary>
        /// Force reset cached parent panel reference.
        /// </summary>
        public void ResetPanel () {
            SetDirty (GuiDirtyType.Panel);
            if (_visualPanel != null) {
                _visualPanel.OnChanged -= OnPanelChanged;
                _visualPanel = null;
            }
        }

        /// <summary>
        /// Force revalidate visuals.
        /// </summary>
        /// <returns>true, if visuals were updated.</returns>
        /// <param name="changes">What should be revalidate.</param>
        public virtual bool UpdateVisuals (GuiDirtyType changes) {
            if (_visualPanel == null) {
                _visualPanel = FindPanel (this);
                _visualPanel.OnChanged += OnPanelChanged;
            }

            if (_meshRenderer == null) {
                return false;
            }

            var pos = transform.position;

            if (_visualPanel.ClipType != GuiPanelClipType.None) {
                // Clipping by height.
                var halfHeight = _height * 0.5f;
                var halfClipHeight = _visualPanel.ClipData.y * 0.5f;
                if ((pos.y + halfHeight) <= (_visualPanel.ClipPos.x - halfClipHeight)) {
                    _meshRenderer.enabled = false;
                    return false;
                }
                if ((pos.y - halfHeight) >= (_visualPanel.ClipPos.y + halfClipHeight)) {
                    _meshRenderer.enabled = false;
                    return false;
                }

                // Clipping by width.
                var halfWidth = _width * 0.5f;
                var halfClipWidth = _visualPanel.ClipData.x * 0.5f;
                if ((pos.x + halfWidth) <= (_visualPanel.ClipPos.x - halfClipWidth)) {
                    _meshRenderer.enabled = false;
                    return false;
                }
                if ((pos.x - halfWidth) >= (_visualPanel.ClipPos.x + halfClipWidth)) {
                    _meshRenderer.enabled = false;
                    return false;
                }
            }

            if (!_meshRenderer.enabled) {
                _meshRenderer.enabled = true;
            }

            if ((changes & GuiDirtyType.Depth) != GuiDirtyType.None) {
                var localPos = _cachedTransform.localPosition;
                localPos.z = -DepthSlice * Depth;
                _cachedTransform.localPosition = localPos;
            }

            if ((changes & GuiDirtyType.Geometry) != GuiDirtyType.None) {
                OnGeometryUpdated (this);
            }
            return true;
        }

        /// <summary>
        /// Bake transform scale to widget size.
        /// </summary>
        public void BakeScale () {
            var scale = transform.localScale;
            Width = (int) (Width * scale.x);
            Height = (int) (Height * scale.y);
            transform.localScale = Vector3.one;
        }

        /// <summary>
        /// Find closest parent panel for widget.
        /// </summary>
        /// <returns>The panel.</returns>
        /// <param name="widget">Widget.</param>
        public static GuiPanel FindPanel (GuiWidget widget) {
            if (widget == null) {
                return null;
            }
            var panel = widget.GetComponentInParent<GuiPanel> ();
            if (panel == null) {
                panel = widget.transform.root.gameObject.AddComponent<GuiPanel> ();
            }
            return panel;
        }
    }
}