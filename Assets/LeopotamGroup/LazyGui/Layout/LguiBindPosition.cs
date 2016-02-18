//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using LeopotamGroup.LazyGui.Core;
using UnityEngine;

namespace LeopotamGroup.LazyGui.Layout {
    [ExecuteInEditMode]
    public class LguiBindPosition : MonoBehaviour {
        public bool Once = true;

        [Range(0f, 1f)]
        public float Horizontal = 0.5f;

        Transform _cachedTransform;

        void OnEnable () {
            _cachedTransform = transform;
            Validate ();
        }

        void Update () {
            Validate ();
            if (Once && Application.isPlaying) {
                enabled = false;
            }
        }

        public void Validate () {
            Horizontal = Mathf.Clamp01 (Horizontal);
            var cam = LguiSystem.Instance.Camera;
            if (cam.pixelRect.width > 0) {
                _cachedTransform.position = cam.ScreenToWorldPoint (new Vector3 (cam.pixelWidth * Horizontal, cam.pixelHeight * 0.5f, 0f));
            }
        }
    }
}