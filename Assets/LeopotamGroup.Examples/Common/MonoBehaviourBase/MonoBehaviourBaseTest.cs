using System.Collections;
using LeopotamGroup.Common;
using UnityEngine;

namespace LeopotamGroup.Examples.Common.MonoBehaviourTest {
    public class MonoBehaviourBaseTest : MonoBehaviourBase {
        IEnumerator Start () {
            yield return new WaitForSeconds (1f);

            var sw = new System.Diagnostics.Stopwatch ();
            var T = 1000000;
            Transform t;

            sw.Reset ();
            sw.Start ();
            for (int i = 0; i < T; i++) {
                t = transform;
            }
            sw.Stop ();
            Debug.Log (sw.ElapsedTicks + " - patched transform, access from local component");

            sw.Reset ();
            sw.Start ();
            for (int i = 0; i < T; i++) {
                t = _cachedTransform;
            }
            sw.Stop ();
            Debug.Log (sw.ElapsedTicks + " - cached to internal field transform, access from local component");

            var c = gameObject.AddComponent <StandardMonoBehaviour> ();
            sw.Reset ();
            sw.Start ();
            for (int i = 0; i < T; i++) {
                t = c.transform;
            }
            sw.Stop ();
            Debug.Log (sw.ElapsedTicks + " - standard transform, access from external component");
        }
    }
}