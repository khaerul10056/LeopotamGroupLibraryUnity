//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System;
using System.Collections;
using LeopotamGroup.Common;
using UnityEngine;

namespace LeopotamGroup.FX {
    sealed class FadeManager : UnitySingleton<FadeManager> {
        public bool _fadeAudio = false;

        float _opaque;

        Texture2D _tex;

        Rect _screenRect;

        Rect _texRect;

        Material _mtrl;

        Coroutine _cb;

        protected override void OnConstruct () {
            DontDestroyOnLoad (gameObject);

            gameObject.layer = LayerMask.NameToLayer ("UI");

            _texRect = new Rect (0f, 0f, 1f, 1f);
            _tex = new Texture2D (1, 1, TextureFormat.RGB24, false);
            _tex.SetPixel (0, 0, Color.white);
            _tex.Apply (false, true);
            _mtrl = new Material (Shader.Find ("Unlit/Transparent Colored"));

            SetFade (0f);
        }

        public void SetFade (float opaque, bool fadeAudio = false) {
            StopFade ();
            _opaque = opaque;
            AudioListener.volume = 1f - (fadeAudio ? _opaque : 0f);
        }

        public void StartFadeTo (float toOpaque, float time, Action callback = null, bool fadeAudio = false) {
            StopFade ();

            Globals.IsUILocked = true;
            _fadeAudio = fadeAudio;

            _cb = StartCoroutine (OnFadeStarted (toOpaque, time, callback));
        }

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
            if ((int)_screenRect.width != Screen.width || (int)_screenRect.height != Screen.height) {
                _screenRect = new Rect (-Screen.width * 0.5f, -Screen.height * 0.5f, Screen.width, Screen.height);
            }

            var color = Color.Lerp (Color.clear, Color.black, _opaque);
            Graphics.DrawTexture (_screenRect, _tex, _texRect, 0, 0, 0, 0, color, _mtrl);
        }
    }
}