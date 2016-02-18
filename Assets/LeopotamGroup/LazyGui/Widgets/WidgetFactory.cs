//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using LeopotamGroup.LazyGui.Core;
using LeopotamGroup.LazyGui.Layout;
using UnityEngine;

namespace LeopotamGroup.LazyGui.Widgets {
    public static class WidgetFactory {
        static T CreateWidget<T> (Transform parent = null) where T: MonoBehaviour {
            LguiSystem.Instance.Validate ();
            var go = new GameObject (typeof (T).Name);
            go.layer = LguiConsts.DefaultGuiLayer;
            var widget = go.AddComponent<T> ();
            if (parent != null) {
                widget.transform.SetParent (parent, false);
                widget.transform.localPosition = Vector3.zero;
            }
            return widget;
        }

        public static LguiPanel CreateWidgetPanel () {
            var widget = CreateWidget<LguiPanel> ();
            widget.InitPhysics ();
            return widget;
        }

        public static LguiSprite CreateWidgetSprite () {
            return CreateWidget<LguiSprite> ();
        }

        public static LguiLabel CreateWidgetLabel () {
            return CreateWidget<LguiLabel> ();
        }

        public static LguiButton CreateWidgetButton () {
            var button = CreateWidget<LguiButton> ();
            var spr = CreateWidget<LguiSprite> (button.transform);
            button.Visuals = new [] { spr };
            var col = button.GetComponent <BoxCollider> ();
            col.isTrigger = true;
            col.size = new Vector3 (spr.Width * 2f, spr.Height * 2f, 0f);
            spr.gameObject.AddComponent <LguiBindColliderSize> ().Target = col;
            return button;
        }

        public static LguiBindPosition CreateLayoutBindPosition () {
            return CreateWidget<LguiBindPosition> ();
        }
    }
}