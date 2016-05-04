//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using UnityEngine;

namespace LeopotamGroup.Common {
    /// <summary>
    /// Singleton pattern, unity version.
    /// </summary>
    public abstract class UnitySingleton<T> : MonoBehaviour where T : MonoBehaviour {
        static T _instance;
        static bool _instanceCreated;

        /// <summary>
        /// Get singleton instance.
        /// </summary>
        /// <value>Instance.</value>
        public static T Instance {
            get {
                // Workaround for slow checking "_instance == null" operation
                // (unity issue, overrided equality operators for additional internal checking).
                if (!_instanceCreated) {
#if UNITY_EDITOR
                    if (!Application.isPlaying) {
                        throw new UnityException (typeof (T).Name + " singleton can be used only at PLAY mode");
                    }
#endif
                    _instance = Object.FindObjectOfType<T> ();
                    if (_instance == null) {
                        _instance = new GameObject (
#if UNITY_EDITOR
                            "_SINGLETON_" + typeof (T).Name
#endif
                        ).AddComponent<T> ();
                    }
                    _instanceCreated = true;
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

        /// <summary>
        /// Replacement of Awake method, will be raised only once for singleton.
        /// Dont use Awake method in inherited classes!
        /// </summary>
        protected virtual void OnConstruct () {
        }

        /// <summary>
        /// Replacement of OnDestroy method, will be raised only once for singleton.
        /// Dont use OnDestroy method in inherited classes!
        /// </summary>
        protected virtual void OnDestruct () {
        }

        /// <summary>
        /// Save checking for singleton instance availability.
        /// </summary>
        /// <returns>Instance exists.</returns>
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