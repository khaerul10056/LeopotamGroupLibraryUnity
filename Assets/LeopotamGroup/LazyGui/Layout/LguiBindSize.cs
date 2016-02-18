//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using LeopotamGroup.LazyGui.Core;
using LeopotamGroup.LazyGui.Widgets;
using UnityEngine;

namespace LeopotamGroup.LazyGui.Layout {
    [ExecuteInEditMode]
    [RequireComponent (typeof (LguiVisualBase))]
    public class LguiBindSize : MonoBehaviour {
        public bool Once = true;

        [Range (0f, 2f)]
        public float Horizontal = 0f;

        [Range (0f, 2f)]
        public float Vertical = 0f;

        LguiVisualBase _visual;

        void OnEnable () {
            _visual = GetComponent <LguiVisualBase> ();
            Validate ();
        }

        void Update () {
            Validate ();
            if (Once && Application.isPlaying) {
                enabled = false;
            }
        }

        public void Validate () {
            var sprite = _visual as LguiSprite;
            if (System.Math.Abs (Horizontal) > 0f) {
                _visual.Width = (int) (LguiSystem.Instance.ScreenHeight * LguiSystem.Instance.Camera.aspect * Horizontal);
                if (sprite != null && sprite.SpriteType.IsTiled ()) {
                    sprite.AlignSizeToOriginal ();
                }
            }
            if (System.Math.Abs (Vertical) > 0f) {
                _visual.Height = (int) (LguiSystem.Instance.ScreenHeight * Vertical);
                if (sprite != null && sprite.SpriteType.IsTiled ()) {
                    sprite.AlignSizeToOriginal ();
                }
            }
        }
    }
}