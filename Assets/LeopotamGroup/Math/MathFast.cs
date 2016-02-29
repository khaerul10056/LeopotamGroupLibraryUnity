//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System.Runtime.InteropServices;
using UnityEngine;

namespace LeopotamGroup.Math {
    public static class MathFast {
        public const float PI = 3.141592654f;

        public const float PI_2 = PI * 2f;

        // All data can be used for inline calculations instead of using sin / cos function calls.
        // How to use - check sin / cos functions.
        public const int SinCosCacheMask = ~(-1 << 12);

        public static readonly float[] SinCacheInternal;

        public static readonly float[] CosCacheInternal;

        public const float SinCosCacheIndexFactor = SinCosCacheSize / PI_2;

        const int SinCosCacheSize = SinCosCacheMask + 1;

        [StructLayout (LayoutKind.Explicit)]
        struct FloatInt {
            [FieldOffset (0)]
            public float Float;

            [FieldOffset (0)]
            public int Int;
        }

        static MathFast () {
            SinCacheInternal = new float[SinCosCacheSize];
            CosCacheInternal = new float[SinCosCacheSize];
            for (var i = 0; i < SinCosCacheSize; i++) {
                SinCacheInternal[i] = (float) System.Math.Sin ((i + 0.5f) / (float) SinCosCacheSize * PI_2);
                CosCacheInternal[i] = (float) System.Math.Cos ((i + 0.5f) / (float) SinCosCacheSize * PI_2);
            }

            var factor = SinCosCacheSize / 360f;
            for (var i = 0; i < 360; i += 90) {
                SinCacheInternal[(int) (i * factor) & SinCosCacheMask] = (float) System.Math.Sin (i * PI / 180f);
                CosCacheInternal[(int) (i * factor) & SinCosCacheMask] = (float) System.Math.Cos (i * PI / 180f);
            }
        }

        public static Vector2 NormalizedFast (this Vector2 v) {
            var wrapper = new FloatInt ();
            wrapper.Float = v.x * v.x + v.y * v.y;
            wrapper.Int = 0x5f3759df - (wrapper.Int >> 1);
            v.x *= wrapper.Float;
            v.y *= wrapper.Float;
            return v;
        }

        public static Vector3 NormalizedFast (this Vector3 v) {
            var wrapper = new FloatInt ();
            wrapper.Float = v.x * v.x + v.y * v.y + v.z * v.z;
            wrapper.Int = 0x5f3759df - (wrapper.Int >> 1);
            v.x *= wrapper.Float;
            v.y *= wrapper.Float;
            v.z *= wrapper.Float;
            return v;
        }

        public static float Sin (float v) {
            return SinCacheInternal[(int) (v * SinCosCacheIndexFactor) & SinCosCacheMask];
        }

        public static float Cos (float v) {
            return CosCacheInternal[(int) (v * SinCosCacheIndexFactor) & SinCosCacheMask];
        }
    }
}