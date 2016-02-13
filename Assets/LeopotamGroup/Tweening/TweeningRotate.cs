//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d License
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using UnityEngine;

namespace LeopotamGroup.Tweening {
    public class TweeningRotate : TweeningBase {
        public Transform Target = null;

        public Vector3 StartValue = Vector3.zero;

        public Vector3 EndValue = Vector3.zero;

        protected override void OnInit () {
            if (Target == null) {
                Target = transform;
            }
        }

        public TweeningRotate Begin (Vector3 target, float time) {
            enabled = false;
            EndValue = target;
            TweenTime = time;
            enabled = true;
            return this;
        }

        protected override void OnUpdateValue () {
            Target.localRotation = Quaternion.Euler (Vector3.Lerp (StartValue, EndValue, Value));
        }
    }
}