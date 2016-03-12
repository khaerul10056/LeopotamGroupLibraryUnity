//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using LeopotamGroup.Common;
using UnityEngine;

namespace LeopotamGroup.Pooling {
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

        public void Recycle () {
            if (Pool != null) {
                Pool.Recycle (this);
            }
        }

        public void SetActive (bool state) {
            gameObject.SetActive (state);
        }
    }
}