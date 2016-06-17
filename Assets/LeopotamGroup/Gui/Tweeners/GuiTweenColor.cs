//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using LeopotamGroup.Gui.Widgets;
using LeopotamGroup.Tweening;
using UnityEngine;

namespace LeopotamGroup.Gui.Tweeners {
    /// <summary>
    /// Tweening GuiWidget color.
    /// </summary>
    [RequireComponent (typeof (GuiWidget))]
    public class GuiTweenColor : TweeningBase {
        /// <summary>
        /// Target GuiWidget. If null on start - GuiWidget on current gameobject will be used.
        /// </summary>
        public GuiWidget Target = null;

        /// <summary>
        /// Start color.
        /// </summary>
        public Color StartValue = Color.white;

        /// <summary>
        /// End color.
        /// </summary>
        public Color EndValue = Color.clear;

        protected override void OnInit () {
            if (Target == null) {
                Target = GetComponent <GuiWidget> ();
                enabled = false;
            }
            if (Target == null) {
                Destroy (this);
            }
        }

        protected override void OnUpdateValue () {
            if (Target != null) {
                Target.Color = Color.Lerp (StartValue, EndValue, Value);
            }
        }

        /// <summary>
        /// Begin tweening.
        /// </summary>
        /// <param name="start">Start color.</param>
        /// <param name="end">End color.</param>
        /// <param name="time">Time for tweening.</param>
        public GuiTweenColor Begin (Color start, Color end, float time) {
            enabled = false;
            StartValue = start;
            EndValue = end;
            TweenTime = time;
            enabled = true;
            return this;
        }

        /// <summary>
        /// Begin tweening at specified GameObject.
        /// </summary>
        /// <param name="go">Holder of tweener.</param>
        /// <param name="start">Start color.</param>
        /// <param name="end">End color.</param>
        /// <param name="time">Time for tweening.</param>
        public static GuiTweenColor Begin (GameObject go, Color start, Color end, float time) {
            var tweener = Get<GuiTweenColor> (go);
            if (tweener != null) {
                tweener.Begin (start, end, time);
            }
            return tweener;
        }
    }
}