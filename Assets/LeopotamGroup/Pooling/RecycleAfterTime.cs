//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d License
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using UnityEngine;

namespace LeopotamGroup.Pooling {
    public sealed class RecycleAfterTime : MonoBehaviour {
        public float Timeout = 1f;

        float _endTime;

        void OnEnable () {
            _endTime = Time.time + Timeout;
        }

        void LateUpdate () {
            if (Time.time >= _endTime) {
                OnRecycle ();
            }
        }

        void OnRecycle () {
            var poolObj = GetComponent <PoolObject> ();
            if (poolObj != null) {
                poolObj.Recycle ();
            } else {
                gameObject.SetActive (false);
            }
        }
    }
}