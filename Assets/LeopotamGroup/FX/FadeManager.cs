//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d License
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

        Texture2D _blackTexture;

        Rect _screenRect;

        Coroutine _cb;

        protected override void OnConstruct () {
            DontDestroyOnLoad (gameObject);

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

        void OnGUI () {
            if (_opaque <= 0f || Event.current.type != EventType.Repaint) {
                return;
            }

            if ((int) _screenRect.width != Screen.width || (int) _screenRect.height != Screen.height) {
                _screenRect = new Rect (0, 0, Screen.width, Screen.height);
            }

            if (_blackTexture == null) {
                _blackTexture = new Texture2D (1, 1);
                _blackTexture.SetPixel (0, 0, Color.black);
                _blackTexture.Apply (false, true);
            }

            var saveColor = GUI.color;
            var saveDepth = GUI.depth;
            GUI.depth = -9999;
            GUI.color = new Color (0, 0, 0, _opaque);
            GUI.DrawTexture (_screenRect, _blackTexture);

            GUI.depth = saveDepth;
            GUI.color = saveColor;
        }
    }
}