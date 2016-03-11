//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System.Collections.Generic;
using LeopotamGroup.LazyGui.Core;
using LeopotamGroup.LazyGui.Widgets;
using UnityEngine;

namespace LeopotamGroup.LazyGui.Layout {
    [ExecuteInEditMode]
    [RequireComponent (typeof (Rigidbody))]
    public sealed class LguiPanel : MonoBehaviour {
        public PanelClipType ClipType {
            get { return _clipType; }
            set {
                if (value != _clipType) {
                    _clipType = value;
                    IsChanged = true;
                }
            }
        }

        public Vector4 ClipData {
            get { return _clipData; }
            set {
                if (value.x > 0 && value.y > 0 && value.z > 0 && value.w > 0 && value != _clipData) {
                    _clipData = value;
                    _clipDataRaw = new Vector4 (1f / _clipData.x * _clipData.z * 2f, 1f / _clipData.y * _clipData.w * 2f, _clipData.z, _clipData.w);
                    IsChanged = true;
                }
            }
        }

        public Vector2 ClipPos {
            get { return _clipPos; }
            set {
                if (value != _clipPos) {
                    _clipPos = value;
                    IsChanged = true;
                }
            }
        }

        public bool IsChanged { get; set; }

        public bool IsPanelActive { get; private set; }

        public Transform CachedTransform {
            get {
                if (_cachedTransform == null) {
                    _cachedTransform = transform;
                }
                return _cachedTransform;
            }
        }

        Transform _cachedTransform;

        [HideInInspector]
        [SerializeField]
        PanelClipType _clipType = PanelClipType.None;

        [HideInInspector]
        [SerializeField]
        Vector4 _clipData = new Vector4 (100f, 100f, 10f, 10f);

        Vector4 _clipDataRaw;

        [HideInInspector]
        [SerializeField]
        Vector2 _clipPos;

        readonly Dictionary<LguiAtlas, Material> _cache = new Dictionary<LguiAtlas, Material> ();

        readonly Dictionary<Font, Material> _fontCache = new Dictionary<Font, Material> ();

        const float DepthSlice = 0.5f;

        void Awake () {
            var rb = GetComponent<Rigidbody> ();
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        void OnEnable () {
            _clipDataRaw = new Vector4 (1f / _clipData.x * _clipData.z * 2f, 1f / _clipData.y * _clipData.w * 2f, _clipData.z, _clipData.w);
            IsChanged = true;
            IsPanelActive = true;
        }

        void OnDisable () {
            IsPanelActive = false;
            BroadcastMessage (LguiConsts.MethodOnLguiPanelChanged, SendMessageOptions.DontRequireReceiver);
        }

        void LateUpdate () {
            if (IsChanged) {
                IsChanged = false;
                UpdateVisuals ();
            }
        }

        void UpdateVisuals () {
            foreach (var texPair in _cache) {
                UpdateMaterial (texPair.Value);
            }
            foreach (var texPair in _fontCache) {
                UpdateMaterial (texPair.Value);
            }
            BroadcastMessage (LguiConsts.MethodOnLguiPanelChanged, SendMessageOptions.DontRequireReceiver);
        }

        void UpdateMaterial (Material mtrl) {
            switch (_clipType) {
                case PanelClipType.Range:
                    mtrl.EnableKeyword (LguiConsts.ShaderKeyWordClipRange);
                    mtrl.SetVector (LguiConsts.ShaderParamClipData, _clipDataRaw);
                    mtrl.SetVector (LguiConsts.ShaderParamClipTrans, _clipPos);
                    break;
                default:
                    mtrl.DisableKeyword (LguiConsts.ShaderKeyWordClipRange);
                    break;
            }
        }

        public void InitPhysics () {
            var rb = GetComponent<Rigidbody> ();
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        public Vector3 GetDepth (LguiVisualBase sprite) {
            var pos = CachedTransform.InverseTransformPoint (sprite.CachedTransform.position);
            pos.z = -DepthSlice * sprite.Depth;
            return CachedTransform.TransformPoint (pos);
        }

        public Material GetMaterial (LguiAtlas atlas) {
            if (atlas == null) {
                return null;
            }
            Material mtrl;
            if (!_cache.ContainsKey (atlas)) {
                mtrl = new Material (Shader.Find ("LeopotamGroup/LazyGui/Standard"));
                mtrl.mainTexture = atlas.ColorTexture;
                mtrl.SetTexture ("_AlphaTex", atlas.AlphaTexture);
                mtrl.hideFlags = HideFlags.DontSave | HideFlags.HideInInspector;
                _cache[atlas] = mtrl;
            } else {
                mtrl = _cache[atlas];
            }
            UpdateMaterial (mtrl);
            return mtrl;
        }

        public Material GetFontMaterial (Font font) {
            if (font == null) {
                return null;
            }
            Material mtrl;
            if (!_fontCache.ContainsKey (font)) {
                mtrl = new Material (Shader.Find ("LeopotamGroup/LazyGui/Font"));
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