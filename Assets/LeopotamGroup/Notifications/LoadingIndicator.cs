//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using LeopotamGroup.Common;
using UnityEngine;

namespace LeopotamGroup.Notifications {
    /// <summary>
    /// "Im busy, loading..." indicator with background fading.
    /// </summary>
    class LoadingIndicator : UnitySingleton<LoadingIndicator> {
        Texture2D _indicator;

        Texture2D _fade;

        bool _isActive;

        Rect _indicatorRect;

        Rect _screenRect;

        float _alpha;

        const float Size = 128f;

        const float DefaultSpeed = 10f;

        public bool IsActive { get; private set; }

        public void Show (bool state) {
            _alpha = 0f;
            _isActive = state;
        }

        protected override void OnConstruct () {
            base.OnConstruct ();

            var dpi = Screen.dpi > 0f ? Screen.dpi : 160f;

            _screenRect = new Rect (0, 0, Screen.width, Screen.height);

            _indicatorRect = new Rect (
                -Size * 0.5f * dpi / 160f + _screenRect.width * 0.5f - 1,
                -Size * 0.5f * dpi / 160f + _screenRect.height * 0.5f,
                Size * dpi / 160f, Size * dpi / 160f);
        }

        protected virtual float OnProgress (float alpha) {
            return (360 / 15f * (int) (Time.time * DefaultSpeed)) % 360;
        }

        Texture2D GetFadeTexture () {
            if (_fade == null) {
                _fade = new Texture2D (1, 1, TextureFormat.RGBA32, false);
                _fade.SetPixel (0, 0, new Color (0f, 0f, 0f, 0.5f));
                _fade.Apply (false, true);
            }
            return _fade;
        }

        Texture2D GetIndicatorTexture () {
            if (_indicator == null) {
                _indicator = Resources.Load<Texture2D> ("UI/LoadingIndicator");
                if (_indicator == null) {
                    _indicator = Resources.Load<Texture2D> ("UI/DefaultLoadingIndicator");
                }
            }
            return _indicator;
        }

        void OnGUI () {
            if (!_isActive) {
                return;
            }

            _alpha = OnProgress (_alpha);

            var savedDepth = GUI.depth;
            GUI.depth = -9999;

            GUI.DrawTexture (_screenRect, GetFadeTexture (), ScaleMode.StretchToFill);

            var savedColor = GUI.color;
            GUI.color = Color.white;

            var savedMat = GUI.matrix;
            var center = new Vector2 (Screen.width * 0.5f, Screen.height * 0.5f);
            GUIUtility.RotateAroundPivot (_alpha, center);

            GUI.DrawTexture (_indicatorRect, GetIndicatorTexture (), ScaleMode.StretchToFill);

            GUI.matrix = savedMat;
            GUI.color = savedColor;
            GUI.depth = savedDepth;
        }
    }
}