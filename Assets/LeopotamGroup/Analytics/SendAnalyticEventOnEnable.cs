//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using UnityEngine;

namespace LeopotamGroup.Analytics {
    /// <summary>
    /// Send analytic event on enable.
    /// </summary>
    public sealed class SendAnalyticEventOnEnable : MonoBehaviour {
        [SerializeField]
        string _category = "Category";

        [SerializeField]
        string _event = "Event";

        void OnEnable () {
            if (!string.IsNullOrEmpty (_category) && !string.IsNullOrEmpty (_event)) {
                GoogleAnalyticsManager.Instance.TrackEvent (_category, _event);
            }
        }
    }
}