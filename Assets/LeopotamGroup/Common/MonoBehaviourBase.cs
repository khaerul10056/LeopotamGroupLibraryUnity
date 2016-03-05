//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using UnityEngine;

namespace LeopotamGroup.Common {
    public abstract class MonoBehaviourBase : MonoBehaviour {
        /// <summary>
        /// Patched transform, gains 2-2.5x performance boost compare to standard.
        /// </summary>
        /// <value>The transform.</value>
        public new Transform transform { get; private set; }

        /// <summary>
        /// Internal cached transform. Dont be fool to overwrite it, no protection for additional 2x performance boost.
        /// </summary>
        protected Transform _cachedTransform;

        protected virtual void Awake () {
            transform = base.transform;
            _cachedTransform = transform;
        }
    }
}