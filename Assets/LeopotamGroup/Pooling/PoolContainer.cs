//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace LeopotamGroup.Pooling {
    public sealed class PoolContainer : MonoBehaviour {
        readonly Stack<PoolObject> _store = new Stack<PoolObject> (64);

        GameObject _prefab;

        public string PrefabPath = "UnknownPrefab";

        bool LoadPrefab () {
            _prefab = Resources.Load<GameObject> (PrefabPath);
            if (_prefab == null) {
                Debug.LogWarning ("Cant load asset " + PrefabPath);
                return false;
            }
            #if UNITY_EDITOR
            if (_prefab.GetComponent <PoolObject> () != null) {
                Debug.LogWarning ("PoolObject cant be used on prefabs");
                _prefab = null;
                UnityEditor.EditorApplication.isPaused = true;
                return false;
            }
            #endif

            return true;
        }

        public PoolObject Get () {
            if (_prefab == null) {
                if (!LoadPrefab ()) {
                    return null;
                }
            }

            PoolObject obj;
            if (_store.Count > 0) {
                obj = _store.Pop ();
            } else {
                var go = Instantiate<GameObject> (_prefab);
                obj = go.AddComponent<PoolObject> ();
                obj.Pool = this;
            }
            obj.SetActive (false);
            return obj;
        }

        public void Recycle (PoolObject obj) {
            if (obj != null && obj.Pool == this) {
                obj.SetActive (false);
                if (!_store.Contains (obj)) {
                    _store.Push (obj);
                }
            } else {
                #if UNITY_EDITOR
                Debug.LogWarning ("Invalid obj to recycle", obj);
                #endif
            }
        }

        public static PoolContainer CreatePool (string prefab) {
            if (string.IsNullOrEmpty (prefab)) {
                return null;
            }
            var container =
                new GameObject (
                #if UNITY_EDITOR
                    "_POOL_" + prefab
                #endif
                ).AddComponent <PoolContainer> ();
            container.PrefabPath = prefab;
            return container;
        }
    }
}