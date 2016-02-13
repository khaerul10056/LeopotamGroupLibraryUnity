//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using UnityEngine;

namespace LeopotamGroup.FX {
    public sealed class SoundOnEnable : MonoBehaviour {
        public AudioClip Sound = null;

        public SoundFXChannel Channel = SoundFXChannel.First;

        public bool IsInterrupt = false;

        void OnEnable () {
            SoundManager.Instance.PlayFX (Sound, Channel, IsInterrupt);
        }
    }
}