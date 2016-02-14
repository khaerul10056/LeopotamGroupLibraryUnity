using LeopotamGroup.Localization;
using UnityEngine;

namespace LeopotamGroup.Examples.LocalizationTest {
    public class LocalizationTest : MonoBehaviour {
        const string TestKey = "common.language";

        void OnGUI () {
            GUILayout.Label ("You can create localization file for all supported languages,\n" +
            "name it 'Localization.csv' and place at 'Resources' folder.\n" +
            "Good decision - use 'google docs' or 'office excel' with csv export.\n\n" +
            "Default language is 'English', user choice will be saved to user prefs.");
            foreach (var item in new [] {
                "English",
                "Russian",
                "German",
                "French"
            }) {
                if (GUILayout.Button (item)) {
                    // After change language OnLocalize method notification will be sent to scene.
                    // It can be used for relocalize custom user content (labels, sprite names, etc).
                    Localizer.Language = item;
                }
            }
            GUILayout.Label (string.Format ("\n<b>Localization for '{0}' is '{1}'</b>", TestKey, Localizer.Get (TestKey)));
        }
    }
}