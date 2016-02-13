//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d License
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using LeopotamGroup.Common;
using UnityEngine;

namespace LeopotamGroup.Notifications {
    /// <summary>
    /// Toast manager (system popups).
    /// </summary>
    sealed class ToastManager : UnitySingleton<ToastManager> {
        float _toastShowTime;

        string _toastText;

        GUIStyle _toastStyle;

        float _toastScale;

        Rect _toastRect;

        const float ToastWidth = 400f;

        const float ToastHeight = 80f;

        const float ToastDelayTime = 3f;

        readonly Color ToastBackgroundColor = new Color32 (20, 20, 20, 160);

        protected override void OnConstruct () {
            _toastShowTime = float.NegativeInfinity;
            _toastScale = (Screen.dpi > 0f ? Screen.dpi : 160f) / 160f;
            _toastRect = new Rect (
                (Screen.width - ToastWidth * _toastScale) * 0.5f,
                (Screen.height - ToastHeight * _toastScale),
                ToastWidth * _toastScale,
                ToastHeight * _toastScale);
        }

        public void Show (string toastText) {
            _toastText = toastText;
            _toastShowTime = string.IsNullOrEmpty (_toastText) ? Time.realtimeSinceStartup + ToastDelayTime : Time.realtimeSinceStartup;
        }

        void OnGUI () {
            if ((_toastShowTime + ToastDelayTime) < Time.realtimeSinceStartup) {
                return;
            }

            if (_toastStyle == null) {
                _toastStyle = new GUIStyle (GUI.skin.box);

                var texture = new Texture2D (1, 1, TextureFormat.RGBA32, false);
                texture.SetPixel (0, 0, ToastBackgroundColor);
                texture.Apply ();
                _toastStyle.normal.background = texture;

                _toastStyle.normal.textColor = Color.white;
                _toastStyle.fontSize = (int) (24f * _toastScale);
                _toastStyle.fontStyle = FontStyle.Bold;
                _toastStyle.alignment = TextAnchor.MiddleCenter;
                _toastStyle.wordWrap = true;
            }

            GUI.Box (_toastRect, _toastText, _toastStyle);
        }
    }
}