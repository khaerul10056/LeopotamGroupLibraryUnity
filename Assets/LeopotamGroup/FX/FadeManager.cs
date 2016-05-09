//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System;
using System.Collections;
using LeopotamGroup.Common;
using UnityEngine;

namespace LeopotamGroup.FX {
    /// <summary>
    /// Fade manager.
    /// </summary>
    sealed class FadeManager : UnitySingleton<FadeManager> {
        /// <summary>
        /// Callback for extensions.
        /// </summary>
        public event Action OnRender = delegate {};

        bool _fadeAudio;

        float _opaque;

        Texture2D _dummyTex;

        Rect _screenRect;

        Rect _dummyTexRect;

        Material _mtrl;

        Coroutine _cb;

        protected override void OnConstruct () {
            DontDestroyOnLoad (gameObject);

            gameObject.layer = LayerMask.NameToLayer ("UI");

            _mtrl = new Material (Shader.Find ("Hidden/LeopotamGroup/FX/ScreenFade"));

            // Graphics.DrawTexture requires any texture.
            _dummyTex = new Texture2D (1, 1, TextureFormat.RGB24, false);
            _dummyTexRect = new Rect (0f, 0f, 1f, 1f);

            SetFade (0f);
        }

        /// <summary>
        /// Set current fade status.
        /// </summary>
        /// <param name="opaque">Opaque value [0, 1], 0 - full transparent, 1 - full opaque.</param>
        /// <param name="fadeAudio">Fade audio sources too.</param>
        public void SetFade (float opaque, bool fadeAudio = false) {
            StopFade ();
            _opaque = opaque;
            AudioListener.volume = 1f - (fadeAudio ? _opaque : 0f);
        }

        /// <summary>
        /// Start fading to target fade status.
        /// </summary>
        /// <param name="toOpaque">Target opaque status.</param>
        /// <param name="time">Time of fading.</param>
        /// <param name="callback">Optional callback on end of fading.</param>
        /// <param name="fadeAudio">Fade audio too.</param>
        public void StartFadeTo (float toOpaque, float time, Action callback = null, bool fadeAudio = false) {
            StopFade ();

            Globals.IsUILocked = true;
            _fadeAudio = fadeAudio;

            _cb = StartCoroutine (OnFadeStarted (toOpaque, time, callback));
        }

        /// <summary>
        /// Stop fading process.
        /// </summary>
        /// <returns>The fade.</returns>
        public void StopFade () {
            if (_cb != null) {
                StopCoroutine (_cb);
                _cb = null;
            }
            Globals.IsUILocked = false;
        }

        IEnumerator OnFadeStarted (float toOpaque, float time, Action callback) {
            var t = 0f;
            var start = _opaque;
            while (t < 1f) {
                _opaque = Mathf.Lerp (start, toOpaque, t);
                if (_fadeAudio) {
                    AudioListener.volume = 1f - _opaque;
                }
                yield return null;
                t += Time.deltaTime / time;
            }
            _opaque = toOpaque;

            if (_fadeAudio) {
                AudioListener.volume = 1f - _opaque;
            }

            _cb = null;
            Globals.IsUILocked = false;

            if (callback != null) {
                callback ();
            }
        }

        void OnRenderObject () {
            if (_opaque <= 0f) {
                return;
            }
            if ((int) _screenRect.width != Screen.width || (int) _screenRect.height != Screen.height) {
                _screenRect = new Rect (-Screen.width * 0.5f, -Screen.height * 0.5f, Screen.width, Screen.height);
            }

            var color = Color.Lerp (Color.clear, Color.black, _opaque);
            Graphics.DrawTexture (_screenRect, _dummyTex, _dummyTexRect, 0, 0, 0, 0, color, _mtrl);

            OnRender ();
        }
    }
}