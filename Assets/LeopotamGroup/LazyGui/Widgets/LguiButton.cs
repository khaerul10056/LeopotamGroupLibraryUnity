//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using LeopotamGroup.LazyGui.Core;
using LeopotamGroup.Tweening;
using UnityEngine;

namespace LeopotamGroup.LazyGui.Widgets {
    [RequireComponent (typeof (BoxCollider))]
    public sealed class LguiButton : LguiEventReceiver {
        public LguiVisualBase[] Visuals = null;

        public Color EnabledColor = Color.white;

        public Color DisabledColor = Color.gray;

        public Vector3 ScaleOnPress = Vector3.one * 0.97f;

        public float ScaleTime = 0.2f;

        public Transform CachedTransform { get; private set; }

        void Awake () {
            CachedTransform = transform;
            OnPress.AddListener (OnBtnPressed);
            OnStateChanged.AddListener (OnBtnStateChanged);
            UpdateAttachedWidgets (enabled);
            UpdateColliderState (enabled);
        }

        void OnBtnPressed (LguiEventReceiver sender, TouchEventArg tea) {
            TweeningScale.Begin (gameObject, tea.State ? Vector3.one : ScaleOnPress, tea.State ? ScaleOnPress : Vector3.one, ScaleTime);
        }

        void OnBtnStateChanged (LguiEventReceiver sender, TouchEventArg tea) {
            UpdateAttachedWidgets (tea.State);
        }

        void UpdateAttachedWidgets (bool isEnabled) {
            if (Visuals != null) {
                var color = isEnabled ? EnabledColor : DisabledColor;
                foreach (var vis in Visuals) {
                    if (vis != null) {
                        vis.Color = color;
                    }
                }
            }
        }
    }
}