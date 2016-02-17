using LeopotamGroup.Analytics;
using UnityEngine;

namespace LeopotamGroup.Examples.Analytics.GoogleAnalyticsTest {
    public class GoogleAnalyticsTest : MonoBehaviour {
        void OnGUI () {
            if (string.IsNullOrEmpty (GoogleAnalyticsManager.Instance.TrackerID)) {
                GUILayout.Label ("Fill TrackerID field for GoogleAnalytics object first!");
                return;
            }
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