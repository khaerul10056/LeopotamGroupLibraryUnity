using LeopotamGroup.Common;
using UnityEngine;

namespace LeopotamGroup.Examples.Common.ScreenManagerTest {
    public class ScreenManagerTestScene2 : MonoBehaviour {
        void OnGUI () {
            GUILayout.Label ("Second scene loaded!");
            if (GUILayout.Button ("Go back to first scene")) {
                ScreenManager.Instance.NavigateBack ();
            }
        }
    }
}