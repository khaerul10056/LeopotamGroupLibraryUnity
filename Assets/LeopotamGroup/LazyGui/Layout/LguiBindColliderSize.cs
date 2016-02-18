//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using LeopotamGroup.LazyGui.Widgets;
using UnityEngine;

namespace LeopotamGroup.LazyGui.Layout {
    [ExecuteInEditMode]
    [RequireComponent(typeof(LguiVisualBase))]
    public class LguiBindColliderSize : MonoBehaviour {
        public Collider Target {
            get { return _target; }
            set {
                if (value != _target) {
                    _target = value;
                    OnLguiVisualSizeChanged ();
                }
            }
        }

        [HideInInspector]
        [SerializeField]
        Collider _target;

        void OnLguiVisualSizeChanged () {
            if (_target != null) {
                var sender = GetComponent<LguiVisualBase> ();
                var target = _target as BoxCollider;
                if (target) {
                    target.center = Vector3.zero;
                    target.size = new Vector3 (sender.Width, sender.Height, 0f);
                } else {
                    #if UNITY_EDITOR
                    Debug.LogWarning ("Only BoxCollider supported");
                    #endif
                }
            }
        }
    }
}