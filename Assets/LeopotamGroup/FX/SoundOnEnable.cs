//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using UnityEngine;

namespace LeopotamGroup.FX {
    /// <summary>
    /// Setup FX parameters on enable.
    /// </summary>
    public sealed class SoundOnEnable : MonoBehaviour {
        /// <summary>
        /// FX AudioClip.
        /// </summary>
        public AudioClip Sound = null;

        /// <summary>
        /// FX channel at SoundManager.
        /// </summary>
        public SoundFXChannel Channel = SoundFXChannel.First;

        /// <summary>
        /// Should new FX force interrupts FX at channel or not.
        /// </summary>
        public bool IsInterrupt = false;

        void OnEnable () {
            SoundManager.Instance.PlayFX (Sound, Channel, IsInterrupt);
        }
    }
}