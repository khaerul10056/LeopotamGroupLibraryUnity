//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d License
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using UnityEngine;

namespace LeopotamGroup.Tweening {
    public abstract class TweeningBase : MonoBehaviour {
        public AnimationCurve Curve = AnimationCurve.Linear (0f, 0f, 1f, 1f);

        public bool IgnoreTimeScale = true;

        public float TweenTime = 1f;

        public int TweenCount = 1;

        public float CurrentTime { get; private set; }

        public float Value { get { return Curve.Evaluate (CurrentTime); } }

        protected int _tweenCount;

        public static T Get<T> (GameObject go) where T : TweeningBase {
            if (go == null) {
                return null;
            }
            var tweener = go.GetComponent<T> ();
            if (tweener == null) {
                tweener = go.AddComponent <T> ();
            }

            return tweener;
        }

        void OnEnable () {
            OnInit ();
            Reset ();
        }

        protected abstract void OnUpdateValue ();

        protected abstract void OnInit ();

        protected virtual void OnReset () {
        }

        public void Reset () {
            _tweenCount = TweenCount;
            CurrentTime = 0f;
            OnReset ();
        }

        void LateUpdate () {
            var deltaTime = IgnoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
            if (deltaTime <= 0f) {
                return;
            }
            if (TweenTime <= 0f) {
                CurrentTime = 0f;
                OnUpdateValue ();
                enabled = false;
                return;
            }
            CurrentTime = Mathf.Clamp01 (CurrentTime + deltaTime / TweenTime);
            OnUpdateValue ();
            if (CurrentTime >= 1f) {
                _tweenCount--;
                if (_tweenCount == 0) {
                    enabled = false;
                    return;
                }
                CurrentTime = 0f;
                if (_tweenCount < 0) {                    
                    _tweenCount = 0;
                }
            }
        }
    }
}