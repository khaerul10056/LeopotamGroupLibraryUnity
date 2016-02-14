using LeopotamGroup.Notifications;
using UnityEngine;

namespace LeopotamGroup.Examples.NotificationsTest {
    public class NotificationsTest : MonoBehaviour {
        void OnGUI () {
            if (LoadingIndicator.Instance.IsActive) {
                if (GUILayout.Button ("Hide loading indicator")) {
                    LoadingIndicator.Instance.Show (false);
                }
            } else {
                if (GUILayout.Button ("Show loading indicator")) {
                    LoadingIndicator.Instance.Show (true);
                }
            }
            if (GUILayout.Button ("Show toast message")) {
                ToastManager.Instance.Show ("My toast message");
            }
        }
    }
}