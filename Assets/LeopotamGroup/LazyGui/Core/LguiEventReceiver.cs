//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using LeopotamGroup.Common;
using UnityEngine;

namespace LeopotamGroup.LazyGui.Core {
    public class LguiEventReceiver : MonoBehaviourBase {
        public OnTouchEventHandler OnPress = new OnTouchEventHandler ();

        public OnTouchEventHandler OnClick = new OnTouchEventHandler ();

        public OnTouchEventHandler OnDrag = new OnTouchEventHandler ();

        public OnTouchEventHandler OnEnableChanged = new OnTouchEventHandler ();

        void OnEnable () {
            UpdateColliderState (true);
            if (OnEnableChanged != null) {
                OnEnableChanged.Invoke (this, new TouchEventArg (true, Vector2.zero, Vector2.zero));
            }
        }

        void OnDisable () {
            UpdateColliderState (false);
            if (OnEnableChanged != null) {
                OnEnableChanged.Invoke (this, new TouchEventArg (false, Vector2.zero, Vector2.zero));
            }
        }

        protected void UpdateColliderState (bool state) {
            var col = GetComponent<Collider> ();
            if (col != null && col.enabled != state) {
                col.enabled = state;
            }
        }

        public void RaisePressEvent (TouchEventArg tea) {
            if (OnPress != null) {
                OnPress.Invoke (this, tea);
            }
        }

        public void RaiseClickEvent (TouchEventArg tea) {
            if (OnClick != null) {
                OnClick.Invoke (this, tea);
            }
        }

        public void RaiseDragEvent (TouchEventArg tea) {
            if (OnDrag != null) {
                OnDrag.Invoke (this, tea);
            }
        }
    }
}