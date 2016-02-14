using LeopotamGroup.EditorHelpers;
using UnityEngine;

namespace LeopotamGroup.Examples.EditorHelpers.FpsCounterTest {
    public class FpsCounterTest : MonoBehaviour {
        void Start () {
            // Just touch any FpsCounter singleton class member - fps counter will be shown cross all scenes.
            FpsCounter.Instance.Validate ();
        }
    }
}