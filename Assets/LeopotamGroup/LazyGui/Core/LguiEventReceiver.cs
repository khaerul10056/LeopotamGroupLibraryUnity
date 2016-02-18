//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using UnityEngine;

namespace LeopotamGroup.LazyGui.Core {
    public class LguiEventReceiver : MonoBehaviour {
        public OnTouchEventHandler OnPress = new OnTouchEventHandler ();

        public OnTouchEventHandler OnClick = new OnTouchEventHandler ();

        public OnTouchEventHandler OnDrag = new OnTouchEventHandler ();

        public OnTouchEventHandler OnStateChanged = new OnTouchEventHandler ();

        public bool IsColliderActivityAttached = true;

        void OnEnable () {
            if (IsColliderActivityAttached) {
                UpdateColliderState (true);
            }
            if (OnStateChanged != null) {
                OnStateChanged.Invoke (this, new TouchEventArg (true, Vector2.zero, Vector2.zero));
            }
        }

        void OnDisable () {
            if (IsColliderActivityAttached) {
                UpdateColliderState (false);
            }
            if (OnStateChanged != null) {
                OnStateChanged.Invoke (this, new TouchEventArg (false, Vector2.zero, Vector2.zero));
            }
        }

        protected void UpdateColliderState (bool state) {
            var col = GetComponent<Collider> ();
            if (col != null && col.enabled != state) {
                col.enabled = state;
            }
        }

        public void RaisePressEvent (TouchEventArg tea) {
//            Debug.LogFormat ("{0} press: {1}", gameObject.name, tea.State);
            if (OnPress != null) {
                OnPress.Invoke (this, tea);
            }
        }

        public void RaiseClickEvent (TouchEventArg tea) {
//                Debug.LogFormat ("{0} click", gameObject.name);
            if (OnClick != null) {
                OnClick.Invoke (this, tea);
            }
        }

        public void RaiseDragEvent (TouchEventArg tea) {
//                Debug.LogFormat ("{0} drag: {1}", gameObject.name, tea.Delta);
            if (OnDrag != null) {
                OnDrag.Invoke (this, tea);
            }
        }
    }
}