//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d License
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using UnityEngine;

namespace LeopotamGroup.Common {
    public abstract class UnitySingleton<T> : MonoBehaviour where T : MonoBehaviour {
        static T _instance;

        public static T Instance {
            get {
                if (_instance == null) {
                    #if UNITY_EDITOR
                    if (!Application.isPlaying) {
                        throw new UnityException (typeof (T).Name + " singleton can be used only at PLAY mode");
                    }
                    #endif
                    _instance = Object.FindObjectOfType <T> ();
                    if (_instance == null) {
                        _instance = new GameObject (
                            #if UNITY_EDITOR
                            "_SINGLETON_" + typeof (T).Name
                            #endif
                        ).AddComponent<T> ();
                    }
                }

                return _instance;
            }
        }

        void Awake () {
            if (_instance != null && _instance != this) {
                DestroyImmediate (gameObject);
                return;
            }

            _instance = this as T;

            OnConstruct ();
        }

        void OnDestroy () {
            if (_instance != null && _instance == this) {
                _instance = null;
                OnDestruct ();
            }
        }

        protected virtual void OnConstruct () {
        }

        protected virtual void OnDestruct () {
        }

        public static bool IsInstanceCreated () {
            return _instance != null;
        }

        /// <summary>
        /// Force validate instance.
        /// </summary>
        public void Validate () {
        }
    }
}