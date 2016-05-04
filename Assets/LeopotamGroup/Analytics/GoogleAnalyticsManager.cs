//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using LeopotamGroup.Common;
using LeopotamGroup.EditorHelpers;
using LeopotamGroup.Localization;
using UnityEngine;

namespace LeopotamGroup.Analytics {
    /// <summary>
    /// Simple GoogleAnalytics manager. Supports tracking of events, screens.
    /// </summary>
    sealed class GoogleAnalyticsManager : UnitySingleton<GoogleAnalyticsManager> {
        [SerializeField]
        public string _trackerID = null;

        const string AnalyticsUrl = "http://www.google-analytics.com/collect?v=1&tid={0}&cid={1}&sr={2}x{3}&an={4}&av={5}&z=";

        readonly Queue<string> _requests = new Queue<string> (64);

        string _requestUrl;

        protected override void OnConstruct () {
            DontDestroyOnLoad (gameObject);
        }

        IEnumerator Start () {
            _requestUrl = null;
            // Wait for additional init.
            yield return null;
#if UNITY_EDITOR
            if (string.IsNullOrEmpty (_trackerID)) {
                Debug.LogWarning ("GA.TrackerID not defined");
            }
#endif
            if (!string.IsNullOrEmpty (_trackerID)) {
                _requestUrl = string.Format (
                    AnalyticsUrl,
                    _trackerID,
                    SystemInfo.deviceUniqueIdentifier,
                    Screen.width, Screen.height,
                    BuildInfo.Instance.AppName,
                    BuildInfo.Instance.AppVersion
                );
            }

            string url = null;
            string data = null;

            while (true) {
                if (_requests.Count > 0) {
                    data = _requests.Dequeue ();
                    // If tracking id defined and url inited.
                    if (!string.IsNullOrEmpty (_requestUrl)) {
                        url = string.Format ("{0}{1}&{2}&ul={3}", _requestUrl, Random.Range (1, 99999), data, Localizer.Language);
                    }
                }

                if (url != null) {
#if UNITY_EDITOR
                    Debug.Log ("[GA REQUEST] " + url);
#endif

                    using (var www = new WWW (url)) {
                        yield return www;
                    }
                    url = null;
                } else {
                    yield return null;
                }
            }
        }

        void EnqueueRequest (string url) {
            _requests.Enqueue (url);
        }

        /// <summary>
        /// Track current screen.
        /// </summary>
        public void TrackScreen () {
            TrackScreen (ScreenManager.Instance.Current);
        }

        /// <summary>
        /// Track screen with custom name.
        /// </summary>
        /// <param name="screenName">Custom screen name.</param>
        public void TrackScreen (string screenName) {
            EnqueueRequest (string.Format ("t=screenview&cd={0}", WWW.EscapeURL (screenName)));
        }

        /// <summary>
        /// Track event.
        /// </summary>
        /// <param name="category">Category name.</param>
        /// <param name="action">Action name.</param>
        public void TrackEvent (string category, string action) {
            EnqueueRequest (string.Format ("t=event&ec={0}&ea={1}",
                WWW.EscapeURL (category),
                WWW.EscapeURL (action)
            ));
        }

        /// <summary>
        /// Track event.
        /// </summary>
        /// <param name="category">Category name.</param>
        /// <param name="action">Action name.</param>
        /// <param name="label">Label name.</param>
        /// <param name="value">Value.</param>
        public void TrackEvent (string category, string action, string label, string value) {
            EnqueueRequest (string.Format ("t=event&ec={0}&ea={1}&el={2}&ev={3}",
                WWW.EscapeURL (category),
                WWW.EscapeURL (action),
                WWW.EscapeURL (label),
                WWW.EscapeURL (value)
            ));
        }

        /// <summary>
        /// Track exception event.
        /// </summary>
        /// <param name="description">Description of exception.</param>
        /// <param name="isFatal">Is exception fatal.</param>
        public void TrackException (string description, bool isFatal) {
            EnqueueRequest (string.Format ("t=exception&exd={0}&exf={1}", WWW.EscapeURL (description), isFatal ? 1 : 0));
        }
    }
}