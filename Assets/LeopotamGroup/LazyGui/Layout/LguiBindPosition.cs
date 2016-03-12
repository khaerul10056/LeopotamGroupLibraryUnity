//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using LeopotamGroup.Common;
using LeopotamGroup.LazyGui.Core;
using UnityEngine;

namespace LeopotamGroup.LazyGui.Layout {
    [ExecuteInEditMode]
    public class LguiBindPosition : MonoBehaviourBase {
        public bool Once = true;

        [Range (0f, 1f)]
        public float Horizontal = 0.5f;

        [Range (0f, 1f)]
        public float Vertical = 0.5f;

        void OnEnable () {
            Validate ();
        }

        void LateUpdate () {
            Validate ();
            if (Once && Application.isPlaying) {
                enabled = false;
            }
        }

        public void Validate () {
            var cam = LguiSystem.Instance.Camera;
            Horizontal = Mathf.Clamp01 (Horizontal);
            Vertical = Mathf.Clamp01 (Vertical);
            if (cam.pixelRect.width > 0) {
                transform.position =
                    cam.ScreenToWorldPoint (new Vector3 (cam.pixelWidth * Horizontal, cam.pixelHeight * Vertical, 0f));
            }
        }
    }
}