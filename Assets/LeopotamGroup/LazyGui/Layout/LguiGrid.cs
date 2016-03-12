//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using LeopotamGroup.Common;
using UnityEngine;

namespace LeopotamGroup.LazyGui.Layout {
    [ExecuteInEditMode]
    public class LguiGrid : MonoBehaviourBase {
        public bool NeedValidate = true;

        public float CellWidth = 100f;

        public float CellHeight = 100f;

        public int LineLength = 0;

        public bool isVertical = false;

        public TextAlignment AlignInLine = TextAlignment.Left;

        void LateUpdate () {
            if (NeedValidate) {
                if (Application.isPlaying) {
                    NeedValidate = false;
                }
                Validate ();
            }
        }

        public void Validate () {
            var root = transform;
            Transform tr;
            Vector3 pos;
            float xOrigin;
            float yOrigin;
            switch (AlignInLine) {
                case TextAlignment.Center:
                    xOrigin = isVertical ? 0.5f * CellWidth : (0.5f - LineLength * 0.5f) * CellWidth; 
                    yOrigin = isVertical ? (0.5f - LineLength * 0.5f) * CellHeight : 0.5f * CellHeight;
                    break;
                case TextAlignment.Right:
                    xOrigin = isVertical ? 0.5f * CellWidth : (0.5f - LineLength) * CellWidth; 
                    yOrigin = isVertical ? (0.5f - LineLength) * CellHeight : 0.5f * CellHeight;
                    break;
                default:
                    xOrigin = 0.5f * CellWidth;
                    yOrigin = 0.5f * CellHeight;
                    break;
            }
            for (int i = 0, iMax = root.childCount; i < iMax; i++) {
                tr = root.GetChild (i);
                pos = tr.localPosition;

                if (isVertical) {
                    pos.x = (LineLength > 0 ? (i / LineLength) : 0) * CellWidth + xOrigin;
                    pos.y = (LineLength > 0 ? (i % LineLength) : i) * CellHeight + yOrigin;
                } else {
                    pos.x = (LineLength > 0 ? (i % LineLength) : i) * CellWidth + xOrigin;
                    pos.y = (LineLength > 0 ? (i / LineLength) : 0) * CellHeight + yOrigin;
                }

                tr.localPosition = pos;
            }
        }
    }
}