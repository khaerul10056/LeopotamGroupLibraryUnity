//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System.Collections;
using UnityEngine;

namespace LeopotamGroup.FX {
    /// <summary>
    /// Setup music parameters on start.
    /// </summary>
    public sealed class MusicOnStart : MonoBehaviour {
        /// <summary>
        /// Music path (for SoundManager).
        /// </summary>
        public string Music = null;

        /// <summary>
        /// Is music looped.
        /// </summary>
        public bool IsLooped = true;

        IEnumerator Start () {
            yield return null;
            if (SoundManager.Instance.MusicVolume <= 0f) {
                SoundManager.Instance.StopMusic ();
            }

            SoundManager.Instance.PlayMusic (Music, IsLooped);
        }
    }
}