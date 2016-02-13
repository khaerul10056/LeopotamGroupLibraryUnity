//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d License
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System.Collections;
using UnityEngine;

namespace LeopotamGroup.FX {
    public sealed class MusicOnStart : MonoBehaviour {
        public string Music = null;

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