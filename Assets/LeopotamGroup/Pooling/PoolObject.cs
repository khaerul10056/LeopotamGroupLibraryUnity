//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using LeopotamGroup.Common;
using UnityEngine;

namespace LeopotamGroup.Pooling {
    /// <summary>
    /// Helper for PoolContainer.
    /// </summary>
    public sealed class PoolObject : MonoBehaviourBase {
        public PoolContainer Pool {
            get { return _pool; }
            set {
                #if UNITY_EDITOR
                if (_pool != null) {
                    Debug.LogWarning ("Pool container already assigned", gameObject);
                }
                #endif
                _pool = value;    
            }
        }

        PoolContainer _pool;

        GameObject _cachedGO;

        /// <summary>
        /// Recycle this instance.
        /// </summary>
        public void Recycle () {
            if (Pool != null) {
                Pool.Recycle (this);
            }
        }

        /// <summary>
        /// Set activity of this instance of prefab.
        /// </summary>
        /// <param name="state">If set to <c>true</c> state.</param>
        public void SetActive (bool state) {
            if (_cachedGO == null) {
                _cachedGO = gameObject;
            }
            _cachedGO.SetActive (state);
        }
    }
}