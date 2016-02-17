//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using UnityEngine;

namespace LeopotamGroup.Tweening {
    public class TweeningScale : TweeningBase {
        public Transform Target = null;

        public Vector3 StartValue = Vector3.one;

        public Vector3 EndValue = Vector3.one * 0.97f;

        protected override void OnInit () {
            if (Target == null) {
                Target = transform;
            }
        }

        public TweeningScale Begin (Vector3 start, Vector3 end, float time) {
            enabled = false;
            StartValue = start;
            EndValue = end;
            TweenTime = time;
            enabled = true;
            return this;
        }

        public static TweeningScale Begin (GameObject go, Vector3 start, Vector3 end, float time) {
            var tweener = Get<TweeningScale> (go);
            if (tweener != null) {
                tweener.Begin (start, end, time);
            }
            return tweener;
        }

        protected override void OnUpdateValue () {
            Target.localScale = Vector3.Lerp (StartValue, EndValue, Value);
        }
    }
}