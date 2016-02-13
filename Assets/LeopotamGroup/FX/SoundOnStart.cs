//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d License
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System.Collections;
using UnityEngine;

namespace LeopotamGroup.FX {
    public sealed class SoundOnStart : MonoBehaviour {
        public AudioClip Sound = null;

        public SoundFXChannel Channel = SoundFXChannel.First;

        public bool IsInterrupt = false;

        IEnumerator Start () {
            yield return null;
            SoundManager.Instance.PlayFX (Sound, Channel, IsInterrupt);
        }
    }
}