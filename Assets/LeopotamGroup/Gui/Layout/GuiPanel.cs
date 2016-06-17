//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System.Collections.Generic;
using LeopotamGroup.Common;
using LeopotamGroup.Gui.Common;
using UnityEngine;
using System;

namespace LeopotamGroup.Gui.Layout {
    /// <summary>
    /// Container for holding GuiWidget-s.
    /// </summary>
    [ExecuteInEditMode]
    public sealed class GuiPanel : MonoBehaviourBase {
        /// <summary>
        /// Will be raised on panel paramters changing (even on OnDisable).
        /// </summary>
        public event Action<GuiPanel> OnChanged = delegate {};

        /// <summary>
        /// Clipping type of children. Not realized yet.
        /// </summary>
        public GuiPanelClipType ClipType {
            get { return _clipType; }
            set {
                if (value != _clipType) {
                    _clipType = value;
                    _isChanged = true;
                }
            }
        }

        /// <summary>
        /// Data for clipping children. not realized yet.
        /// </summary>
        public Vector4 ClipData {
            get { return _clipData; }
            set {
                if (value.x > 0 && value.y > 0 && value.z > 0 && value.w > 0 && value != _clipData) {
                    _clipData = value;
                    _clipDataRaw = new Vector4 (1f / _clipData.x * _clipData.z * 2f, 1f / _clipData.y * _clipData.w * 2f, _clipData.z, _clipData.w);
                    _isChanged = true;
                }
            }
        }

        /// <summary>
        /// Position for clipping children. not realized yet.
        /// </summary>
        public Vector2 ClipPos {
            get { return _clipPos; }
            set {
                if (value != _clipPos) {
                    _clipPos = value;
                    _isChanged = true;
                }
            }
        }

        /// <summary>
        /// Order of panel for proper sorting. Positive values brings panel closer to camera.
        /// </summary>
        /// <value>The depth.</value>
        public int Depth {
            get { return _depth; }
            set {
                if (value != _depth) {
                    _depth = value;
                    _isChanged = true;
                }
            }
        }

        [HideInInspector]
        [SerializeField]
        int _depth;

        [HideInInspector]
        [SerializeField]
        GuiPanelClipType _clipType = GuiPanelClipType.None;

        [HideInInspector]
        [SerializeField]
        Vector4 _clipData = new Vector4 (100f, 100f, 10f, 10f);

        [HideInInspector]
        [SerializeField]
        Vector2 _clipPos;

        readonly Dictionary<GuiAtlas, Material> _mtrlCache = new Dictionary<GuiAtlas, Material> ();

        readonly Dictionary<Font, Material> _fontCache = new Dictionary<Font, Material> ();

        bool _isChanged;

        Vector4 _clipDataRaw;

        void OnEnable () {
            _clipDataRaw = new Vector4 (1f / _clipData.x * _clipData.z * 2f, 1f / _clipData.y * _clipData.w * 2f, _clipData.z, _clipData.w);
            _isChanged = true;
        }

        void OnDisable () {
            OnChanged (null);
        }

        void LateUpdate () {
            if (_isChanged) {
                _isChanged = false;
                UpdateVisuals ();
            }
        }

        void UpdateMaterial (Material mtrl) {
            switch (_clipType) {
                case GuiPanelClipType.Range:
                    mtrl.EnableKeyword (GuiConsts.ShaderKeyWordClipRange);
                    mtrl.SetVector (GuiConsts.ShaderParamClipData, _clipDataRaw);
                    mtrl.SetVector (GuiConsts.ShaderParamClipTrans, _clipPos);
                    break;
                default:
                    mtrl.DisableKeyword (GuiConsts.ShaderKeyWordClipRange);
                    break;
            }
        }

        /// <summary>
        /// Force revalidate internal atlases, fonts and children.
        /// </summary>
        public void UpdateVisuals () {
            var guiCamTrans = GuiSystem.Instance.Camera.transform;
            var pos = guiCamTrans.InverseTransformPoint (_cachedTransform.position);
            pos.z = -_depth * 50f;
            _cachedTransform.position = guiCamTrans.TransformPoint (pos);

            foreach (var texPair in _mtrlCache) {
                UpdateMaterial (texPair.Value);
            }
            foreach (var texPair in _fontCache) {
                UpdateMaterial (texPair.Value);
            }
            OnChanged (this);
        }

        /// <summary>
        /// Reset materials cache. All dependent widgets should be invalidate manually!
        /// </summary>
        public void ResetMaterialCache () {
            foreach (var item in _mtrlCache) {
                DestroyImmediate (item.Value);
            }
            _mtrlCache.Clear ();
        }

        /// <summary>
        /// Get cached material for GuiAtlas.
        /// </summary>
        /// <param name="atlas">GuiAtlas instance.</param>
        public Material GetMaterial (GuiAtlas atlas) {
            if (atlas == null) {
                return null;
            }
            Material mtrl;
            if (!_mtrlCache.ContainsKey (atlas)) {
                mtrl = new Material (Shader.Find (atlas.AlphaTexture != null ? "LeopotamGroup/Gui/Standard" : "LeopotamGroup/Gui/Opaque"));
                mtrl.mainTexture = atlas.ColorTexture;
                mtrl.SetTexture ("_AlphaTex", atlas.AlphaTexture);
                mtrl.hideFlags = HideFlags.DontSave | HideFlags.HideInInspector;
                _mtrlCache[atlas] = mtrl;
            } else {
                mtrl = _mtrlCache[atlas];
            }
            UpdateMaterial (mtrl);
            return mtrl;
        }

        /// <summary>
        /// Get cached font material.
        /// </summary>
        /// <param name="font">Font instance.</param>
        public Material GetFontMaterial (Font font) {
            if (font == null) {
                return null;
            }
            Material mtrl;
            if (!_fontCache.ContainsKey (font)) {
                mtrl = new Material (Shader.Find ("LeopotamGroup/Gui/Font"));
                mtrl.mainTexture = font.material.mainTexture;
                mtrl.hideFlags = HideFlags.DontSave | HideFlags.HideInInspector;
                _fontCache[font] = mtrl;
            } else {
                mtrl = _fontCache[font];
            }
            UpdateMaterial (mtrl);
            return mtrl;
        }
    }
}