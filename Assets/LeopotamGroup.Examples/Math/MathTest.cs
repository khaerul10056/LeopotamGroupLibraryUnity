using UnityEngine;
using LeopotamGroup.Math;
using System.Collections;

namespace LeopotamGroup.Examples.MathTest {
    public class MathTest : MonoBehaviour {
        IEnumerator Start () {
            yield return new WaitForSeconds (1f);
            RngTest ();
            Vector2iTest ();
            Vector3iTest ();
            SinTest ();
            CosTest ();
            Atan2Test ();
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
            Debug.Log (">>>>> sin tests >>>>>");
            const int T = 10000;
            var sw = new System.Diagnostics.Stopwatch ();
            float f;
            float s = 1.345f;

            sw.Reset ();
            sw.Start ();
            for (int i = 0; i < T; i++) {
                f = Mathf.Sin (s);
            }
            sw.Stop ();
            Debug.LogFormat ("mathf.sin time on {0} iterations: {1}", T, sw.ElapsedTicks);

            sw.Reset ();
            sw.Start ();
            for (int i = 0; i < T; i++) {
                f = (float) System.Math.Sin (s);
            }
            sw.Stop ();
            Debug.LogFormat ("system.math.sin time on {0} iterations: {1}", T, sw.ElapsedTicks);

            // Warmup cache.
            f = MathFast.Sin (s);

            sw.Reset ();
            sw.Start ();
            for (int i = 0; i < T; i++) {
                f = MathFast.Sin (s);
            }
            sw.Stop ();
            Debug.LogFormat ("mathfast.sin time on {0} iterations: {1}", T, sw.ElapsedTicks);

            for (int i = 0; i < 10; i++) {
                f = Rng.GetFloatStatic () * 3.1415926f * 2;
                Debug.LogFormat ("sin({0}) => {1} / {2}", f, Mathf.Sin (f), MathFast.Sin (f));
            }
        }

        void CosTest () {
            Debug.Log (">>>>> cos tests >>>>>");
            const int T = 10000;
            var sw = new System.Diagnostics.Stopwatch ();
            float f;
            float s = 1.345f;

            sw.Reset ();
            sw.Start ();
            for (int i = 0; i < T; i++) {
                f = Mathf.Cos (s);
            }
            sw.Stop ();
            Debug.LogFormat ("mathf.cos time on {0} iterations: {1}", T, sw.ElapsedTicks);

            sw.Reset ();
            sw.Start ();
            for (int i = 0; i < T; i++) {
                f = (float) System.Math.Cos (s);
            }
            sw.Stop ();
            Debug.LogFormat ("system.math.cos time on {0} iterations: {1}", T, sw.ElapsedTicks);

            // Warmup cache.
            f = MathFast.Cos (s);

            sw.Reset ();
            sw.Start ();
            for (int i = 0; i < T; i++) {
                f = MathFast.Cos (s);
            }
            sw.Stop ();
            Debug.LogFormat ("mathfast.cos time on {0} iterations: {1}", T, sw.ElapsedTicks);

            for (int i = 0; i < 10; i++) {
                f = Rng.GetFloatStatic () * MathFast.PI_2;
                Debug.LogFormat ("cos({0}) error checking => {1} / {2}", f, Mathf.Cos (f), MathFast.Cos (f));
            }
        }

        void Atan2Test () {
            Debug.Log (">>>>> atan2 tests >>>>>");
            const int T = 10000;
            var sw = new System.Diagnostics.Stopwatch ();
            float f;
            var sy = 1.234f;
            var sx = 2.345f;

            sw.Reset ();
            sw.Start ();
            for (int i = 0; i < T; i++) {
                f = Mathf.Atan2 (sy, sx);
            }
            sw.Stop ();
            Debug.LogFormat ("mathf.atan2 time on {0} iterations: {1}", T, sw.ElapsedTicks);

            // Warmup cache.
            f = MathFast.Atan2 (sy, sx);

            sw.Reset ();
            sw.Start ();
            for (int i = 0; i < T; i++) {
                f = MathFast.Atan2 (sy, sx);
            }
            sw.Stop ();
            Debug.LogFormat ("mathfast.atan2 time on {0} iterations: {1}", T, sw.ElapsedTicks);

            for (int i = 0; i < 10; i++) {
                sy = Rng.GetFloatStatic () * MathFast.PI_2;
                sx = Rng.GetFloatStatic () * MathFast.PI_2;
                Debug.LogFormat ("atan2({0}, {1}) error checking => {2} / {3}",
                    sy, sx, Mathf.Atan2 (sy, sx) * MathFast.Rad2Deg, MathFast.Atan2 (sy, sx) * MathFast.Rad2Deg);
            }
        }
    }
}