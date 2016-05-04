//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System.Collections;
using UnityEngine;

namespace LeopotamGroup.FX {
    /// <summary>
    /// Setup FX parameters on start.
    /// </summary>
    public sealed class SoundOnStart : MonoBehaviour {
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

        IEnumerator Start () {
            yield return null;
            SoundManager.Instance.PlayFX (Sound, Channel, IsInterrupt);
        }
    }
}