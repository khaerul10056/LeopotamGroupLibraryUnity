//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LeopotamGroup.Common {
    /// <summary>
    /// Screen / scene manager, provides api for navigation with history rollback support.
    /// </summary>
    sealed class ScreenManager : UnitySingleton<ScreenManager> {
        /// <summary>
        /// Get previous screen name or null.
        /// </summary>
        public string Previous { get; private set; }

        /// <summary>
        /// Get current screen name.
        /// </summary>
        public string Current { get; private set; }

        readonly Stack<string> _history = new Stack<string> ();

        protected override void OnConstruct () {
            DontDestroyOnLoad (gameObject);
            Previous = null;
            Current = SceneManager.GetActiveScene ().name;
        }

        /// <summary>
        /// Navigate to new screen.
        /// </summary>
        /// <param name="screenName">Target screen name.</param>
        /// <param name="saveToHistory">Save current screen to history for using NavigateBack later.</param>
        public void NavigateTo (string screenName, bool saveToHistory = false) {
            Previous = Current;
            if (saveToHistory) {
                _history.Push (Previous);
            }

            Current = screenName;
            SceneManager.LoadScene (screenName);
        }

        /// <summary>
        /// Navigate back through saved in history screens.
        /// </summary>
        public void NavigateBack () {
#if UNITY_EDITOR
            if (_history.Count == 0) {
                Debug.LogWarning ("Cant navigate back");
                return;
            }
#endif
            if (_history.Count > 0) {
                Current = _history.Pop ();
                Previous = _history.Count > 0 ? _history.Peek () : null;
                SceneManager.LoadScene (Current);
            }
        }

        /// <summary>
        /// Force history clearup.
        /// </summary>
        public void ClearHistory () {
            _history.Clear ();
            Previous = null;
        }
    }
}