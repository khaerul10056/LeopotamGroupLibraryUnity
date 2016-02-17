//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LeopotamGroup.Common {
    sealed class ScreenManager : UnitySingleton<ScreenManager> {
        public string Previous { get; private set; }

        public string Current { get; private set; }

        readonly Stack<string> _history = new Stack<string> ();

        protected override void OnConstruct () {
            DontDestroyOnLoad (gameObject);
            Previous = null;
            Current = SceneManager.GetActiveScene ().name;
        }

        public void NavigateTo (string screenName, bool saveToHistory = false) {
            Previous = Current;
            if (saveToHistory) {
                _history.Push (Previous);
            }

            Current = screenName;
            SceneManager.LoadScene (screenName);
        }

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

        public void ClearHistory () {
            _history.Clear ();
            Previous = null;
        }
    }
}