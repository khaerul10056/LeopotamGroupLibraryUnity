using LeopotamGroup.Common;
using UnityEngine;

namespace LeopotamGroup.Examples.Common.ScreenManaging {
    public class ScreenManagerTestScene1 : MonoBehaviour {
        const string Scene1Name = "ScreenManager - 1";

        const string Scene2Name = "ScreenManager - 2";

        void OnGUI () {
            GUILayout.Label (
                string.Format ("Add both scenes ('{0}', '{1}') from this folder to BuildSettings and press button",
                    Scene1Name, Scene2Name));
            if (GUILayout.Button ("Go to second scene")) {
                ScreenManager.Instance.NavigateTo (Scene2Name, true);
            }
        }
    }
}