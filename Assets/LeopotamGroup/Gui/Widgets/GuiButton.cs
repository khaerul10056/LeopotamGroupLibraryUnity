//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using LeopotamGroup.Gui.Common;
using LeopotamGroup.Gui.Tweeners;
using LeopotamGroup.Tweening;
using UnityEngine;

namespace LeopotamGroup.Gui.Widgets {
    /// <summary>
    /// Button widget.
    /// </summary>
    public sealed class GuiButton : GuiEventReceiver {
        /// <summary>
        /// List of button-frontface widgets.
        /// </summary>
        public GuiWidget[] Visuals = null;

        /// <summary>
        /// Color of normal enabled state.
        /// </summary>
        public Color EnableColor = Color.white;

        /// <summary>
        /// Color of pressed state.
        /// </summary>
        public Color ActiveColor = Color.white;

        /// <summary>
        /// Color of disabled state.
        /// </summary>
        public Color DisableColor = Color.gray;

        /// <summary>
        /// Scale on press event.
        /// </summary>
        public Vector3 ScaleOnPress = new Vector3 (0.97f, 0.97f, 1f);

        /// <summary>
        /// Tween time of scaling / recoloring on press event.
        /// </summary>
        public float TweenTime = 0.2f;

        protected override void Awake () {
            base.Awake ();
            OnPress.AddListener (OnBtnPressed);
        }

        protected override void OnEnable () {
            base.OnEnable ();
            UpdateAttachedWidgets (EnableColor, TweenTime);
        }

        protected override void OnDisable () {
            UpdateAttachedWidgets (DisableColor, 0f);
            base.OnDisable ();
        }

        void OnBtnPressed (GuiEventReceiver sender, GuiTouchEventArg tea) {
            TweeningScale.Begin (gameObject, tea.State ? Vector3.one : ScaleOnPress, tea.State ? ScaleOnPress : Vector3.one, TweenTime);
            UpdateAttachedWidgets (tea.State ? ActiveColor : (enabled ? EnableColor : DisableColor), TweenTime);
        }

        void UpdateAttachedWidgets (Color color, float time) {
            if (Visuals != null) {
                foreach (var vis in Visuals) {
                    if (vis != null) {
                        if (time > 0f) {
                            GuiTweenColor.Begin (vis.gameObject, vis.Color, color, time);
                        } else {
                            vis.Color = color;
                        }
                    }
                }
            }
        }
    }
}