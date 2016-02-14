using UnityEngine;

namespace LeopotamGroup.Examples.MobileTest {
    public class MobileTest : MonoBehaviour {
        void OnGUI () {
            GUILayout.Label ("On android if you will press 'back' button - app will be closed.\n" +
            "At editor / standalone app you can press 'Escape' key with same result.");
        }

        #region UI callbacks

        public void OnBackPressed () {
            // This event was subscribed from inspector of BackButtonBehaviour.
            Debug.Log ("Back pressed");
            Application.Quit ();
        }

        #endregion
    }
}