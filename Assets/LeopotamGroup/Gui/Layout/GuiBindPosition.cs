//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using LeopotamGroup.Common;
using LeopotamGroup.Gui.Common;
using LeopotamGroup.Gui.Widgets;
using UnityEngine;

namespace LeopotamGroup.Gui.Layout {
    /// <summary>
    /// Bind transform position to target GuiWidget or screen borders.
    /// </summary>
    [ExecuteInEditMode]
    public sealed class GuiBindPosition : MonoBehaviourBase {
        /// <summary>
        /// Binding will be done only once, then component will be disabled.
        /// </summary>
        public bool Once = true;

        /// <summary>
        /// Target GuiWidget to binding. If null - screen borders will be used.
        /// </summary>
        public GuiWidget Target = null;

        /// <summary>
        /// Horizontal multiplier.
        /// </summary>
        [Range (0f, 1f)]
        public float Horizontal = 0.5f;

        /// <summary>
        /// Vertical multiplier.
        /// </summary>
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

        /// <summary>
        /// Force revalidate position.
        /// </summary>
        public void Validate () {
            Horizontal = Mathf.Clamp01 (Horizontal);
            Vertical = Mathf.Clamp01 (Vertical);
            Vector3 pos;
            if (Target != null) {
                pos = new Vector3 ((Horizontal - 0.5f) * Target.Width, (Vertical - 0.5f) * Target.Height, 0f);
                pos = Target.transform.TransformPoint (pos);
            } else {
                var cam = GuiSystem.Instance.Camera;
                pos = cam.ScreenToWorldPoint (new Vector3 (cam.pixelWidth * Horizontal, cam.pixelHeight * Vertical, 0f));
            }

            if (transform.parent != null) {
                pos = transform.parent.InverseTransformPoint (pos);
            }
            pos.z = transform.localPosition.z;
            transform.localPosition = pos;
        }
    }
}