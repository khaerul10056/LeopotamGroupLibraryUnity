using LeopotamGroup.Tutorials;
using UnityEngine;

namespace LeopotamGroup.Examples.TutorialsTest {
    public class TutorialTestStage : MonoBehaviour {
        public string Content;

        void OnEnable () {
            Debug.Log ("New tutorial stage activated");
        }

        void OnGUI () {
            GUILayout.Label ((Content ?? "No content"));
            if (GUILayout.Button ("Move to next stage")) {
                TutorialManager.Instance.RaiseNextBit ();
            }
        }
    }
}