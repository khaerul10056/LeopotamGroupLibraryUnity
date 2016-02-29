using UnityEngine;
using LeopotamGroup.Math;

namespace LeopotamGroup.Examples.MathTest {
    public class MathTest : MonoBehaviour {
        void Start () {
            RngTest ();
            Vector2iTest ();
            Vector3iTest ();
            SinTest ();
        }

        void RngTest () {
            for (var i = 0; i < 5; i++) {
                Debug.LogFormat ("Rng.GetFloatStatic [0;1]: {0}", Rng.GetFloatStatic (true));
            }
            for (var i = 0; i < 5; i++) {
                Debug.LogFormat ("Rng.GetIntStatic [0;100): {0}", Rng.GetInt32Static (100));
            }
        }

        void Vector2iTest () {
            var v2i_0 = new Vector2i (1, 2);
            var v2i_1 = new Vector2i (3, 4);
            Debug.LogFormat ("{0} + {1} = {2}", v2i_0, v2i_1, v2i_0 + v2i_1);
            Debug.LogFormat ("{0} - {1} = {2}", v2i_0, v2i_1, v2i_0 - v2i_1);
            Debug.LogFormat ("{0} * {1} = {2}", v2i_0, v2i_1, v2i_0 * v2i_1);
            Debug.LogFormat ("{0} / {1} = {2}", v2i_0, v2i_1, v2i_0 / v2i_1);
            Debug.LogFormat ("{0}.lossyMagnitude = {2}, {1}.lossyMagnitude = {3}", v2i_0, v2i_1, v2i_0.LossyMagnituded, v2i_1.LossyMagnituded);
        }

        void Vector3iTest () {
            var v3i_0 = new Vector3i (1, 2, 3);
            var v3i_1 = new Vector3i (4, 5, 6);
            Debug.LogFormat ("{0} + {1} = {2}", v3i_0, v3i_1, v3i_0 + v3i_1);
            Debug.LogFormat ("{0} - {1} = {2}", v3i_0, v3i_1, v3i_0 - v3i_1);
            Debug.LogFormat ("{0} * {1} = {2}", v3i_0, v3i_1, v3i_0 * v3i_1);
            Debug.LogFormat ("{0} / {1} = {2}", v3i_0, v3i_1, v3i_0 / v3i_1);
            Debug.LogFormat ("{0}.lossyMagnitude = {2}, {1}.lossyMagnitude = {3}", v3i_0, v3i_1, v3i_0.LossyMagnituded, v3i_1.LossyMagnituded);
        }

        void SinTest () {
            var T = 10000;
            var sw = new System.Diagnostics.Stopwatch ();
            float f;
            float s = 1.345f;

            sw.Reset ();
            sw.Start ();
            for (int i = 0; i < T; i++) {
                f = Mathf.Sin (s);
            }
            Debug.Log ("sin-unity: " + sw.ElapsedTicks);

            sw.Reset ();
            sw.Start ();
            for (int i = 0; i < T; i++) {
                f = (float)System.Math.Sin (s);
            }
            Debug.Log ("sin-system: " + sw.ElapsedTicks);

            sw.Reset ();
            sw.Start ();
            for (int i = 0; i < T; i++) {
                f = MathFast.Sin (s);
            }
            Debug.Log ("sin-fast: " + sw.ElapsedTicks);

            for (int i = 0; i < 10; i++) {
                f = Rng.GetFloatStatic () * 3.1415926f;
                Debug.LogFormat ("{0} => {1} + {2}", f, Mathf.Sin (f), MathFast.Sin (f));
            }
        }
    }
}