//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System;
using LeopotamGroup.EditorHelpers;
using UnityEngine;

namespace LeopotamGroup.Tutorials {
    /// <summary>
    /// Mask of bits (flags).
    /// </summary>
    [Flags]
    public enum TutorialMask : int {
        Bit00 = (1 << 0),
        Bit01 = (1 << 1),
        Bit02 = (1 << 2),
        Bit03 = (1 << 3),
        Bit04 = (1 << 4),
        Bit05 = (1 << 5),
        Bit06 = (1 << 6),
        Bit07 = (1 << 7),
        Bit08 = (1 << 8),
        Bit09 = (1 << 9),
        Bit10 = (1 << 10),
        Bit11 = (1 << 11),
        Bit12 = (1 << 12),
        Bit13 = (1 << 13),
        Bit14 = (1 << 14),
        Bit15 = (1 << 15),
        Bit16 = (1 << 16),
        Bit17 = (1 << 17),
        Bit18 = (1 << 18),
        Bit19 = (1 << 19),
        Bit20 = (1 << 20),
        Bit21 = (1 << 21),
        Bit22 = (1 << 22),
        Bit23 = (1 << 23),
        Bit24 = (1 << 24),
        Bit25 = (1 << 25),
        Bit26 = (1 << 26),
        Bit27 = (1 << 27),
        Bit28 = (1 << 28),
        Bit29 = (1 << 29),
        Bit30 = (1 << 30)
    }

    /// <summary>
    /// Helper for tutorial stage processing. All children will be set active / non-active based on mask state.
    /// </summary>
    public sealed class TutorialInfo : MonoBehaviour {
        /// <summary>
        /// Children will be hidden if this bits mask equals to current state of tutorial bits processing.
        /// </summary>
        [EnumFlags]
        public TutorialMask HideChildrenOnMask = 0;

        /// <summary>
        /// Children will be shown if this bits mask equals to current state of tutorial bits processing.
        /// </summary>
        [EnumFlags]
        public TutorialMask ShowChildrenOnMask = 0;

        void OnEnable () {
            TutorialManager.Instance.OnTutorialUpdated += OnTutorialUpdated;
            OnTutorialUpdated ();
        }

        void OnDisable () {
            if (TutorialManager.IsInstanceCreated ()) {
                TutorialManager.Instance.OnTutorialUpdated -= OnTutorialUpdated;
            }
        }

        void OnTutorialUpdated () {
            var isProcessed = false;
            var result = false;

            if ((int) HideChildrenOnMask != 0 && TutorialManager.Instance.ValidateMask (HideChildrenOnMask)) {
                isProcessed = true;
                result = false;
            }
            if (!isProcessed && (int) ShowChildrenOnMask != 0 && TutorialManager.Instance.ValidateMask (ShowChildrenOnMask)) {
                isProcessed = true;
                result = true;
            }

            if (isProcessed) {
                foreach (Transform child in transform) {
                    child.gameObject.SetActive (result);
                }
            }
        }
    }
}