//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using LeopotamGroup.LazyGui.Widgets;
using LeopotamGroup.Tweening;
using UnityEngine;

namespace LeopotamGroup.LazyGui.Tweeners {
    public class LguiTweenColor : TweeningBase {
        public LguiVisualBase Target = null;

        public Color StartValue = Color.white;

        public Color EndValue = Color.clear;

        protected override void OnInit () {
            if (Target == null) {
                Target = GetComponent <LguiVisualBase> ();
                enabled = false;
            }
        }

        public LguiTweenColor Begin (Color start, Color end, float time) {
            enabled = false;
            StartValue = start;
            EndValue = end;
            TweenTime = time;
            enabled = true;
            return this;
        }

        public static LguiTweenColor Begin (GameObject go, Color start, Color end, float time) {
            var tweener = Get<LguiTweenColor> (go);
            if (tweener != null) {
                tweener.Begin (start, end, time);
            }
            return tweener;
        }

        protected override void OnUpdateValue () {
            Target.Color = Color.Lerp (StartValue, EndValue, Value);
        }
    }
}