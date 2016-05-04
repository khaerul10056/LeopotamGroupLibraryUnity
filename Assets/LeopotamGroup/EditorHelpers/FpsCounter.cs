//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using LeopotamGroup.Common;
using UnityEngine;

namespace LeopotamGroup.EditorHelpers {
    /// <summary>
    /// Fps counter.
    /// </summary>
    sealed class FpsCounter : UnitySingleton<FpsCounter> {
        const int UpdateFrequency = 2;

        const float _invUpdatesPerSecond = 1f / (float)UpdateFrequency;

        const float RectWidth = 100f;

        const float RectHeight = 50f;

        const float ShadowX = 1f;

        const float ShadowY = 1f;

        readonly Rect _rect = new Rect (0f, 0f, RectWidth, RectHeight);

        readonly Rect _rectShadow = new Rect (ShadowX, ShadowY, RectWidth + ShadowX, RectHeight + ShadowY);

        float _frameCount;

        float _lastTime;

        string _data = string.Empty;

        protected override void OnConstruct () {
            DontDestroyOnLoad (gameObject);
        }

        void Update () {
            _frameCount++;
            if ((Time.realtimeSinceStartup - _lastTime) >= _invUpdatesPerSecond) {
                CurrentFps = _frameCount * UpdateFrequency;
                _frameCount = 0;
                _lastTime = Time.realtimeSinceStartup;
                _data = string.Format ("<b>fps: {0:F2}</b>", CurrentFps);
            }
        }

        void OnGUI () {
            if (Event.current.type == EventType.Repaint) {
                GUI.color = Color.black;
                GUI.Label (_rectShadow, _data);
                GUI.color = Color.white;
                GUI.Label (_rect, _data);
            }
        }

        /// <summary>
        /// Get current fps.
        /// </summary>
        /// <value>The current fps.</value>
        public float CurrentFps { get; private set; }
    }
}