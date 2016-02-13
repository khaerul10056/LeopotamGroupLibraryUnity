//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d License
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using UnityEngine;
using UnityEngine.Events;

namespace LeopotamGroup.Mobile {
    public sealed class BackButtonBehaviour : MonoBehaviour {
        public UnityEvent OnBackPressed = null;

        void Update () {
            if (Input.GetKeyDown (KeyCode.Escape)) {
                if (OnBackPressed != null) {
                    OnBackPressed.Invoke ();
                }
            }
        }
    }
}