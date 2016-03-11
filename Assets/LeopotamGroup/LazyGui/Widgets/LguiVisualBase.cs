//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using LeopotamGroup.LazyGui.Core;
using LeopotamGroup.LazyGui.Layout;
using UnityEngine;

namespace LeopotamGroup.LazyGui.Widgets {
    [ExecuteInEditMode]
    public abstract class LguiVisualBase : LguiWidgetBase {
        public ChangeType VisualChanges { get; private set; }

        public virtual void AddVisualChanges (ChangeType changes) {
            VisualChanges |= changes;
        }

        public Color Color {
            get { return _color; }
            set {
                if (value != _color) {
                    _color = value;
                    AddVisualChanges (ChangeType.Color);
                }
            }
        }

        public int Width {
            get { return _width; }
            set {
                if (value >= 0 && value != _width) {
                    _width = value;
                    AddVisualChanges (ChangeType.Geometry);
                    SendMessage (LguiConsts.MethodOnLguiVisualSizeChanged, SendMessageOptions.DontRequireReceiver);
                }
            }
        }

        public int Height {
            get { return _height; }
            set {
                if (value >= 0 && value != _height) {
                    _height = value;
                    AddVisualChanges (ChangeType.Geometry);
                    SendMessage (LguiConsts.MethodOnLguiVisualSizeChanged, SendMessageOptions.DontRequireReceiver);
                }
            }
        }

        public bool IsVisible { get { return _meshRenderer != null && _meshRenderer.enabled; } }

        [HideInInspector]
        [SerializeField]
        int _width = 64;

        [HideInInspector]
        [SerializeField]
        int _height = 64;

        [HideInInspector]
        [SerializeField]
        Color _color = Color.white;

        public int Depth {
            get { return _depth; }
            set {
                if (value != _depth) {
                    _depth = value;
                    AddVisualChanges (ChangeType.Depth);
                }
            }
        }

        [HideInInspector]
        [SerializeField]
        int _depth;

        Bounds _bounds;

        protected LguiPanel _visualPanel;

        protected MeshRenderer _meshRenderer;

        void LateUpdate () {
            if (VisualChanges != ChangeType.None) {
                var changes = VisualChanges;
                VisualChanges = ChangeType.None;
                UpdateVisuals (changes);
            }
        }

        protected override bool UpdateVisuals (ChangeType changes) {
            if (!base.UpdateVisuals (changes)) {
                return false;
            }
            if (_visualPanel == null || ((changes & ChangeType.Panel) != ChangeType.None)) {
                _visualPanel = GetComponentInParent<LguiPanel> ();
                if (_visualPanel == null) {
                    _visualPanel = CachedTransform.root.gameObject.AddComponent<LguiPanel> ();
                    _visualPanel.InitPhysics ();
                }
                while (!_visualPanel.IsPanelActive && _visualPanel.CachedTransform.parent != null) {
                    _visualPanel = _visualPanel.CachedTransform.parent.GetComponentInParent<LguiPanel> ();
                }
            }

            if (_meshRenderer == null) {
                return false;
            }

            var pos = CachedTransform.position;

            if (_visualPanel.ClipType != PanelClipType.None) {
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

            if ((changes & ChangeType.Depth) != ChangeType.None) {
                CachedTransform.position = _visualPanel.GetDepth (this);
            }
            return true;
        }

        protected virtual void OnLguiPanelChanged () {
            AddVisualChanges (ChangeType.Panel);
        }
    }
}