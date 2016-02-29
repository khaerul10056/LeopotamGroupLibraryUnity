//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System.Runtime.InteropServices;
using UnityEngine;

namespace LeopotamGroup.Math {
    public static class MathFast {
        const float PI = 3.141592654f;

        const float PI_DIV_2 = 3.141592654f / 2f;

        const float PI_2 = 3.141592654f * 2;

        const float FOUR_DIV_PI = 4 / PI;

        const float FOUR_DIV_SQR_PI = 4 / (PI * PI);

        [StructLayout (LayoutKind.Explicit)]
        struct FloatInt {
            [FieldOffset (0)]
            public float Float;

            [FieldOffset (0)]
            public int Int;
        }

        [StructLayout (LayoutKind.Explicit)]
        struct DoubleInt {
            [FieldOffset (0)]
            public double Double;

            [FieldOffset (0)]
            public int Int;
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

        public static float Sin2 (float x) {
            if (x < -PI) {
                x += PI_2;
            } else {
                if (x > PI) {
                    x -= PI_2;
                }
            }
            var sin = FOUR_DIV_PI * x + (x < 0f ? 1f : -1f) * FOUR_DIV_SQR_PI * x * x;
            sin = 0.225f * ((sin < 0 ? -1f : 1f) * sin * sin - sin) + sin;
            return sin;
        }

        public static float Sin (float x) {
            if (x < -PI) {
                x += PI_2;
            } else {
                if (x > PI) {
                    x -= PI_2;
                }
            }
            x = x * (1.27323954f + (x < 0 ? 1f : -1f) * 0.405284735f * x);
            x = x * (x < 0 ? -0.225f * (x + 1) + 1 : 0.225f * (x - 1) + 1);
            return x;
        }

        public static float Cos (float x) {
            return Sin (x + PI_DIV_2);
        }
    }
}