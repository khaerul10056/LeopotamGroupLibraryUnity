using LeopotamGroup.FX;
using UnityEngine;

namespace LeopotamGroup.Examples.FX.SoundManagerTest {
    public class SoundManagerTest : MonoBehaviour {
        public AudioClip FXClip = null;

        const string MusicName = "Music/Forest";

        void OnGUI () {
            if (GUILayout.Button ("Turn on music")) {
                SoundManager.Instance.PlayMusic (MusicName, true);
            }
            if (GUILayout.Button ("Turn off music")) {
                SoundManager.Instance.StopMusic ();
            }
            if (FXClip != null && GUILayout.Button ("Play FX at channel 1 without interrupt")) {
                SoundManager.Instance.PlayFX (FXClip);
            }
            if (FXClip != null && GUILayout.Button ("Play FX at channel 1 with interrupt")) {
                SoundManager.Instance.PlayFX (FXClip, SoundFXChannel.First, true);
            }
        }
    }
}