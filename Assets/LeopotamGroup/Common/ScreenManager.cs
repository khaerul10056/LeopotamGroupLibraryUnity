//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d License
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LeopotamGroup.Common {
    sealed class ScreenManager : UnitySingleton<ScreenManager> {
        public string PreviousScreen { get; private set; }

        public string CurrentScreen { get; private set; }

        readonly Stack<string> _history = new Stack<string> ();

        protected override void OnConstruct () {
            DontDestroyOnLoad (gameObject);
            PreviousScreen = null;
            CurrentScreen = SceneManager.GetActiveScene ().name;
        }

        public void NavigateTo (string screenName, bool saveToHistory = false) {
            PreviousScreen = CurrentScreen;
            if (saveToHistory) {
                _history.Push (PreviousScreen);
            }

            CurrentScreen = screenName;
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
                CurrentScreen = _history.Pop ();
                PreviousScreen = _history.Count > 0 ? _history.Peek () : null;
                SceneManager.LoadScene (CurrentScreen);
            }
        }

        public void ClearHistory () {
            _history.Clear ();
            PreviousScreen = null;
        }
    }
}