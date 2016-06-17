//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using LeopotamGroup.Common;
using UnityEngine;

namespace LeopotamGroup.Gui.Common {
    /// <summary>
    /// Touch event receiver.
    /// </summary>
    public class GuiEventReceiver : MonoBehaviourBase {
        /// <summary>
        /// Will be raised on press / release event.
        /// </summary>
        public OnGuiTouchEventHandler OnPress = new OnGuiTouchEventHandler ();

        /// <summary>
        /// Will be raised on click event.
        /// </summary>
        public OnGuiTouchEventHandler OnClick = new OnGuiTouchEventHandler ();

        /// <summary>
        /// Will be raised on drag event.
        /// </summary>
        public OnGuiTouchEventHandler OnDrag = new OnGuiTouchEventHandler ();

        /// <summary>
        /// Width of touch zone.
        /// </summary>
        [HideInInspector]
        public int Width = 1;

        /// <summary>
        /// Height of touch zone.
        /// </summary>
        [HideInInspector]
        public int Height = 1;

        /// <summary>
        /// Order inside panel. Positive values brings panel closer to camera.
        /// </summary>
        [HideInInspector]
        public int Depth = 0;

        /// <summary>
        /// Get cached global order in camera space.
        /// </summary>
        public float GlobalDepthOrder { get; private set; }

        protected virtual void OnEnable () {
            GuiSystem.Instance.AddEventReceiver (this);
        }

        protected virtual void OnDisable () {
            if (GuiSystem.IsInstanceCreated ()) {
                GuiSystem.Instance.RemoveEventReceiver (this);
            }
        }

        void LateUpdate () {
            GlobalDepthOrder = GuiSystem.Instance.Camera.transform.InverseTransformPoint (_cachedTransform.TransformPoint (0f, 0f, Depth * 0.5f)).z;
        }

        /// <summary>
        /// Raise OnPress event manually.
        /// </summary>
        /// <param name="tea">Tea.</param>
        public void RaisePressEvent (GuiTouchEventArg tea) {
            if (OnPress != null) {
                OnPress.Invoke (this, tea);
            }
        }

        /// <summary>
        /// Raise OnClick event manually.
        /// </summary>
        /// <param name="tea">Tea.</param>
        public void RaiseClickEvent (GuiTouchEventArg tea) {
            if (OnClick != null) {
                OnClick.Invoke (this, tea);
            }
        }

        /// <summary>
        /// Raise OnDrag event manually.
        /// </summary>
        /// <param name="tea">Tea.</param>
        public void RaiseDragEvent (GuiTouchEventArg tea) {
            if (OnDrag != null) {
                OnDrag.Invoke (this, tea);
            }
        }

        /// <summary>
        /// Check if 2d point placed inside current touch zone.
        /// </summary>
        /// <param name="x">X coordinate of point.</param>
        /// <param name="y">Y coordinate of point.</param>
        public bool IsPointInside (float x, float y) {
            var pos = _cachedTransform.InverseTransformPoint (x, y, 0f);
            var halfWidth = Width * 0.5f;
            var halfHeight = Height * 0.5f;
            return pos.x >= -halfWidth && pos.x <= halfWidth && pos.y >= -halfHeight && pos.y <= halfHeight;
        }
    }
}