//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using UnityEngine;
using LeopotamGroup.Gui.Common;
using LeopotamGroup.Tweening;

namespace LeopotamGroup.Gui.Layout {
    /// <summary>
    /// Grid widget.
    /// </summary>
    [ExecuteInEditMode]
    public class GuiGrid : MonoBehaviour {
        /// <summary>
        /// Width of item. Negative value changes direction.
        /// </summary>
        public float ItemWidth = 100f;

        /// <summary>
        /// Height of item. Negative value changes direction.
        /// </summary>
        public float ItemHeight = -100f;

        /// <summary>
        /// Items count in row. If 0 - no wrapping.
        /// </summary>
        public int ItemsInRow = 0;

        /// <summary>
        /// Is row vertical oriented. By default orientation is horizontal.
        /// </summary>
        public bool IsRowVerticalOriented = false;

        /// <summary>
        /// Animation time for items reposition. If 0 - no tweening.
        /// </summary>
        public float AnimationTime = 0f;

        /// <summary>
        /// Item bounds alignment.
        /// </summary>
        public GuiAlignment ItemsAlignment = GuiAlignment.TopLeft;

        void LateUpdate () {
            Validate ();
            if (Application.isPlaying) {
                enabled = false;
            }
        }

        /// <summary>
        /// Force revalidate alignment.
        /// </summary>
        public void Validate () {
            var root = transform;
            var childCount = root.childCount;

            ItemsInRow = Mathf.Max (0, ItemsInRow);
            AnimationTime = Mathf.Max (0f, AnimationTime);

            if (childCount > 0) {
                float boundWidth = ItemWidth;
                float boundHeight = ItemHeight;

                if (IsRowVerticalOriented) {
                    boundWidth *= ItemsInRow > 0 ? Mathf.Max (0, (childCount / ItemsInRow) - 1) : 0;
                    boundHeight *= (ItemsInRow > 0 ? ItemsInRow : childCount) - 1;
                } else {
                    boundWidth *= ((ItemsInRow > 0 ? ItemsInRow : childCount) - 1) * ItemWidth;
                    boundHeight *= ItemsInRow > 0 ? Mathf.Max (0, (childCount / ItemsInRow) - 1) : 0;
                }


                var pivotOffset = AlignmentOffset (ItemsAlignment, boundWidth, boundHeight);

                float itemOffset;
                float rowOffset;
                Vector3 pos;
                Transform tr;

                for (var i = 0; i < childCount; i++) {
                    if (IsRowVerticalOriented) {
                        itemOffset = ItemsInRow > 0 ? (i / ItemsInRow) * ItemWidth : 0f; 
                        rowOffset = (ItemsInRow > 0 ? (i % ItemsInRow) : i) * ItemHeight;
                    } else {
                        itemOffset = (ItemsInRow > 0 ? (i % ItemsInRow) : i) * ItemWidth; 
                        rowOffset = ItemsInRow > 0 ? (i / ItemsInRow) * ItemHeight : 0f;
                    }

                    tr = root.GetChild (i);
                    pos = new Vector3 (itemOffset + pivotOffset.x, rowOffset + pivotOffset.y, 0f);

                    if (Application.isPlaying && AnimationTime > 0f) {
                        TweeningPosition.Begin (tr.gameObject, tr.localPosition, pos, AnimationTime);
                    } else {
                        tr.localPosition = pos;
                    }
                }
            }
        }

        static Vector2 AlignmentOffset (GuiAlignment alignment, float width, float height) {
            switch (alignment) {
                case GuiAlignment.TopLeft:
                    return new Vector2 (width > 0f ? 0f : -width, height > 0f ? -height : 0f);
                case GuiAlignment.TopCenter:
                    return new Vector2 (-width * 0.5f, height > 0f ? -height : 0f);
                case GuiAlignment.TopRight:
                    return new Vector2 (width > 0f ? -width : 0f, height > 0f ? -height : 0f);
                case GuiAlignment.CenterLeft:
                    return new Vector2 (width > 0f ? 0f : -width, -height * 0.5f);
                case GuiAlignment.Center:
                    return new Vector2 (-width * 0.5f, -height * 0.5f);
                case GuiAlignment.CenterRight:
                    return new Vector2 (width > 0f ? -width : 0f, -height * 0.5f);
                case GuiAlignment.BottomLeft:
                    return new Vector2 (width > 0f ? 0f : -width, height > 0f ? 0f : -height);
                case GuiAlignment.BottomCenter:
                    return new Vector2 (-width * 0.5f, height > 0f ? 0f : -height);
                case GuiAlignment.BottomRight:
                    return new Vector2 (width > 0f ? -width : 0f, height > 0f ? 0f : -height);
                default:
                    return Vector2.zero;
            }
        }
    }
}