//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using LeopotamGroup.Common;
using LeopotamGroup.LazyGui.Core;
using UnityEngine;

namespace LeopotamGroup.LazyGui.Layout {
    [ExecuteInEditMode]
    public class LguiGrid : MonoBehaviourBase {
        public bool NeedValidate = true;

        public float CellWidth = 100f;

        public float CellHeight = 100f;

        public int ItemsInRow = 0;

        public bool VerticalOrder = true;

        public VerticalAlignment VerticalAlignment = VerticalAlignment.Top;

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
            var childCount = root.childCount;

            if (childCount > 0) {
                Transform tr;
                Vector3 pos;
                float offset;

                // For top to down direction.
                var cH = -CellHeight;
                var iIR = (float) (ItemsInRow == 0 ? 1 : ItemsInRow);

                switch (VerticalAlignment) {
                    case VerticalAlignment.Bottom:
                        offset = CellHeight * childCount / iIR;
                        break;
                    case VerticalAlignment.Center:
                        offset = CellHeight * childCount / iIR * 0.5f;
                        break;
                    default:
                        offset = 0f;
                        break;
                }

                for (var i = 0; i < childCount; i++) {
                    tr = root.GetChild (i);
                    pos = tr.localPosition;

                    if (VerticalOrder) {
                        pos.x = (ItemsInRow > 0 ? (i / ItemsInRow) : 0) * CellWidth;
                        pos.y = (ItemsInRow > 0 ? (i % ItemsInRow) : i) * cH + offset;
                    } else {
                        pos.x = (ItemsInRow > 0 ? (i % ItemsInRow) : i) * CellWidth;
                        pos.y = (ItemsInRow > 0 ? (i / ItemsInRow) : 0) * cH + offset;
                    }

                    tr.localPosition = pos;
                }
            }
        }
    }
}