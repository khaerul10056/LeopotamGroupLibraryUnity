//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using LeopotamGroup.Common;
using LeopotamGroup.Gui.Common;
using LeopotamGroup.Gui.Layout;
using UnityEngine;

namespace LeopotamGroup.Gui.Widgets {
    /// <summary>
    /// Contains helpers for widget creation at runtime.
    /// </summary>
    public static class WidgetFactory {
        static T CreateWidget<T> (Transform parent = null) where T: MonoBehaviour {
            GuiSystem.Instance.Validate ();
            var go = new GameObject (typeof (T).Name);
            go.layer = GuiConsts.DefaultGuiLayer;
            var widget = go.AddComponent<T> ();
            if (parent != null) {
                widget.transform.SetParent (parent, false);
                widget.transform.localPosition = Vector3.zero;
            }
            return widget;
        }

        /// <summary>
        /// Create GuiPanel.
        /// </summary>
        /// <returns>The widget panel.</returns>
        public static GuiPanel CreateWidgetPanel () {
            return CreateWidget<GuiPanel> ();
        }

        /// <summary>
        /// Create GuiSprite.
        /// </summary>
        /// <returns>The widget sprite.</returns>
        public static GuiSprite CreateWidgetSprite () {
            return CreateWidget<GuiSprite> ();
        }

        /// <summary>
        /// Create GuiLabel.
        /// </summary>
        /// <returns>The widget label.</returns>
        public static GuiLabel CreateWidgetLabel () {
            return CreateWidget<GuiLabel> ();
        }

        /// <summary>
        /// Create GuiButton.
        /// </summary>
        /// <returns>The widget button.</returns>
        public static GuiButton CreateWidgetButton () {
            var button = CreateWidget<GuiButton> ();
            var spr = CreateWidget<GuiSprite> (button.transform);
            button.Visuals = new [] { spr };
            return button;
        }

        /// <summary>
        /// Create GuiBindPosition.
        /// </summary>
        /// <returns>The layout bind position.</returns>
        /// <param name="go">Go.</param>
        public static GuiBindPosition CreateLayoutBindPosition (GameObject go) {
            return go != null ? go.EnsureGetComponent<GuiBindPosition> () : null;
        }

        /// <summary>
        /// Create GuiBindSize.
        /// </summary>
        /// <returns>The layout bind size.</returns>
        /// <param name="go">Go.</param>
        public static GuiBindSize CreateLayoutBindSize (GameObject go) {
            return go != null ? go.EnsureGetComponent<GuiBindSize> () : null;
        }
    }
}