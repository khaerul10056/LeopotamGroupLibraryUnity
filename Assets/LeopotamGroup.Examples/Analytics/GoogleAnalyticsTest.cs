using LeopotamGroup.Analytics;
using UnityEngine;

namespace LeopotamGroup.Examples.Analytics.GoogleAnalyticsTest {
    public class GoogleAnalyticsTest : MonoBehaviour {
        void OnGUI () {
            if (!GoogleAnalyticsManager.Instance.IsInited) {
                GUILayout.Label ("Fill TrackerID field for GoogleAnalytics object first!");
                return;
            }

            GUILayout.Label ("Device identifier: " + GoogleAnalyticsManager.Instance.DeviceHash);

            if (GUILayout.Button ("Track 'Screen Test opened'")) {
                GoogleAnalyticsManager.Instance.TrackScreen ("Test");
            }
            if (GUILayout.Button ("Track 'Item.001 purchased'")) {
                GoogleAnalyticsManager.Instance.TrackEvent ("Purchases", "Item.001");
            }
            if (GUILayout.Button ("Track 'Exception raised'")) {
                GoogleAnalyticsManager.Instance.TrackException ("OMG, app crashed", true);
            }
        }
    }
}