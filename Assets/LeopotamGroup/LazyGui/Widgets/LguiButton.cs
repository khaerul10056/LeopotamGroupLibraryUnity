//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using LeopotamGroup.LazyGui.Core;
using LeopotamGroup.LazyGui.Tweeners;
using LeopotamGroup.Tweening;
using UnityEngine;

namespace LeopotamGroup.LazyGui.Widgets {
    [RequireComponent (typeof (BoxCollider))]
    public class LguiButton : LguiEventReceiver {
        public LguiVisualBase[] Visuals = null;

        public Color EnableColor = Color.white;

        public Color ActiveColor = Color.white;

        public Color DisableColor = Color.gray;

        public Vector3 ScaleOnPress = Vector3.one * 0.97f;

        public float TweenTime = 0.2f;

        public Transform CachedTransform { get; private set; }

        void Awake () {
            CachedTransform = transform;
            OnPress.AddListener (OnBtnPressed);
            OnEnableChanged.AddListener (OnBtnStateChanged);
            UpdateAttachedWidgets (enabled ? EnableColor : DisableColor);
            UpdateColliderState (enabled);
        }

        void OnBtnPressed (LguiEventReceiver sender, TouchEventArg tea) {
            TweeningScale.Begin (gameObject, tea.State ? Vector3.one : ScaleOnPress, tea.State ? ScaleOnPress : Vector3.one, TweenTime);
            UpdateAttachedWidgets (tea.State ? ActiveColor : (enabled ? EnableColor : DisableColor));
        }

        void OnBtnStateChanged (LguiEventReceiver sender, TouchEventArg tea) {
            UpdateAttachedWidgets (tea.State ? EnableColor : DisableColor);
        }

        void UpdateAttachedWidgets (Color color) {
            if (Visuals != null) {
                foreach (var vis in Visuals) {
                    if (vis != null) {
                        LguiTweenColor.Begin (vis.gameObject, vis.Color, color, TweenTime);
                    }
                }
            }
        }
    }
}