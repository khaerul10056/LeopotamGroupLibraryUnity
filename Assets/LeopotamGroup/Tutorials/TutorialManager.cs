//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System;
using System.Collections.Generic;
using LeopotamGroup.Common;
using LeopotamGroup.Serialization;
using UnityEngine;

namespace LeopotamGroup.Tutorials {
    /// <summary>
    /// Tutorial stages processing. Progress will be autosaved to PlayerPrefs.
    /// </summary>
    sealed class TutorialManager : LeopotamGroup.Common.UnitySingleton<TutorialManager> {
        /// <summary>
        /// Will be raised on stage changes.
        /// </summary>
        public event Action OnTutorialUpdated = delegate {};

        Dictionary<string, int> _sceneMasks;

        const int MaxKeyAmount = 30;

        const string TutorialKey = "tutorial";

        protected override void OnConstruct () {
            DontDestroyOnLoad (gameObject);
            LoadData ();
            SaveData ();
        }

        void LoadData () {
            try {
                Debug.Log ("tutorials: " + PlayerPrefs.GetString (TutorialKey));
                _sceneMasks = JsonSerialization.DeserializeStatic<Dictionary<string, int>> (PlayerPrefs.GetString (TutorialKey));
            } catch {
//                Debug.LogWarning (ex);
                _sceneMasks = null;
            }

            if (_sceneMasks == null) {
                _sceneMasks = new Dictionary<string, int> ();
            }
        }

        void SaveData () {
            try {
                // Convert to Json serialization compatible view.
                var copy = new Dictionary<string, object> ();
                foreach (var item in _sceneMasks) {
                    copy.Add (item.Key, item.Value);
                }
                PlayerPrefs.SetString (TutorialKey, JsonSerialization.SerializeStatic (copy));
            } catch (Exception ex) {
                Debug.LogWarning (ex);
            }
        }

        /// <summary>
        /// Clears full tutorial data (for all screens).
        /// </summary>
        /// <param name="sendEvent">If set to <c>true</c> send event.</param>
        public void ClearAllScreensData (bool sendEvent = false) {
            _sceneMasks = new Dictionary<string, int> ();
            SaveData ();
            if (sendEvent) {
                OnTutorialUpdated ();
            }
        }

        /// <summary>
        /// Are masked bits enabled for current screen.
        /// </summary>
        /// <param name="mask">Mask.</param>
        public bool ValidateMask (TutorialMask mask) {
            var scene = ScreenManager.Instance.Current;

            if (_sceneMasks.ContainsKey (scene)) {
                return (_sceneMasks[scene] & (int) mask) == (int) mask;
            }
            return false;
        }

        /// <summary>
        /// Get mask for current screen.
        /// </summary>
        public TutorialMask GetMask () {
            var scene = ScreenManager.Instance.Current;

            return (TutorialMask) (_sceneMasks.ContainsKey (scene) ? _sceneMasks[scene] : 0);
        }

        /// <summary>
        /// Set masked bits additive to new state for current screen.
        /// </summary>
        /// <param name="mask">Masked bits.</param>
        /// <param name="state">New state.</param>
        public void SetMask (TutorialMask mask, bool state = true) {
            var scene = ScreenManager.Instance.Current;

            var data = _sceneMasks.ContainsKey (scene) ? _sceneMasks[scene] : 0;
            var newData = data;
            if (state) {
                newData |= (int) mask;
            } else {
                newData &= ~(int) mask;
            }
            if (newData != data) {
                _sceneMasks[scene] = newData;
                SaveData ();
                OnTutorialUpdated ();
            }
        }

        /// <summary>
        /// Sets all bits to new state for current screen.
        /// </summary>
        /// <param name="state">New state.</param>
        public void SetAll (bool state) {
            var scene = ScreenManager.Instance.Current;
            if (state) {
                _sceneMasks[scene] = (1 << MaxKeyAmount) - 1;
            } else {
                if (_sceneMasks.ContainsKey (scene)) {
                    _sceneMasks.Remove (scene);
                }
            }
            SaveData ();
        }

        /// <summary>
        /// Raises next bit in sequence of stages for current screen.
        /// </summary>
        public void RaiseNextBit () {
            var mask = (int) GetMask ();
            for (int i = 0; i < MaxKeyAmount; i++) {
                if ((mask & 1) == 0) {
                    SetMask ((TutorialMask) (1 << i));
                    break;
                }
                mask >>= 1;
            }
        }
    }
}