using LeopotamGroup.Common;
using LeopotamGroup.FX;
using UnityEngine;

namespace LeopotamGroup.Examples.EditorHelpers.FadeManagerTest {
    public class FadeManagerTest : MonoBehaviour {
        float _targetFade = 1f;

        void OnGUI () {
            // FadeManager automatically changed Globals.IsUILocked flag during fading.
            if (!Globals.IsUILocked) {
                if (GUILayout.Button ("Fade in/ Fade out")) {
                    FadeManager.Instance.StartFadeTo (_targetFade, 1f, () => {
                        _targetFade = _targetFade > 0f ? 0f : 1f;
                    });
                }
            }
        }
    }
}