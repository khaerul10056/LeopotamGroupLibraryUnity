//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using LeopotamGroup.LazyGui.Core;
using UnityEngine;

namespace LeopotamGroup.LazyGui.Widgets {
    [ExecuteInEditMode]
    public abstract class LguiWidgetBase : MonoBehaviour {
        public Transform CachedTransform {
            get {
                if (_cachedTransform == null) {
                    _cachedTransform = transform;
                }
                return _cachedTransform;
            }
        }

        Transform _cachedTransform;

        protected virtual bool UpdateVisuals (ChangeType changeType) {
            return true;
        }
    }
}