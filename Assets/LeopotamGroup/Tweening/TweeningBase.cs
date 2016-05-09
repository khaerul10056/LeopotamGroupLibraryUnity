//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using UnityEngine;
using LeopotamGroup.Common;

namespace LeopotamGroup.Tweening {
    /// <summary>
    /// Tweening base class.
    /// </summary>
    public abstract class TweeningBase : MonoBehaviour {
        /// <summary>
        /// Curve of tweening values.
        /// </summary>
        public AnimationCurve Curve = AnimationCurve.Linear (0f, 0f, 1f, 1f);

        /// <summary>
        /// Should tweener ignore time scale or not.
        /// </summary>
        public bool IgnoreTimeScale = true;

        /// <summary>
        /// Timee of tweening.
        /// </summary>
        public float TweenTime = 1f;

        /// <summary>
        /// Tweening process count, endless if 0.
        /// </summary>
        public int TweenCount = 1;

        /// <summary>
        /// Get current normalized time of tweening.
        /// </summary>
        public float CurrentTime { get; private set; }

        /// <summary>
        /// Gets tweened value at current time.
        /// </summary>
        /// <value>The value.</value>
        public float Value { get { return Curve.Evaluate (CurrentTime); } }

        /// <summary>
        /// Internal state of tween count.
        /// </summary>
        protected int _tweenCount;

        /// <summary>
        /// Helper for initialization of tweeners from code.
        /// </summary>
        /// <param name="go">Holder of tweener.</param>
        /// <typeparam name="T">Type of tweener.</typeparam>
        public static T Get<T> (GameObject go) where T : TweeningBase {
            if (go == null) {
                return null;
            }
            return go.EnsureGetComponent<T> ();
        }

        void OnEnable () {
            OnInit ();
            Reset ();
        }

        /// <summary>
        /// Will be raised on each update of tween value (time).
        /// </summary>
        protected abstract void OnUpdateValue ();

        /// <summary>
        /// Will be raised on first init of tweener.
        /// </summary>
        protected abstract void OnInit ();

        /// <summary>
        /// Will be raised on reset tweener to start values.
        /// </summary>
        protected virtual void OnReset () {
        }

        /// <summary>
        /// Reset tweener state to start value.
        /// </summary>
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