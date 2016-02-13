//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using UnityEngine;

namespace LeopotamGroup.Tweening {
    public class TweeningPosition : TweeningBase {
        public Transform Target = null;

        public Vector3 StartValue = Vector3.zero;

        public Vector3 EndValue = Vector3.zero;

        protected override void OnInit () {
            if (Target == null) {
                Target = transform;
            }
        }

        public TweeningPosition Begin (Vector3 target, float time) {
            enabled = false;
            EndValue = target;
            TweenTime = time;
            enabled = true;
            return this;
        }

        protected override void OnUpdateValue () {
            Target.localPosition = Vector3.Lerp (StartValue, EndValue, Value);
        }
    }
}