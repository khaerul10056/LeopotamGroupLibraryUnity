//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System.Runtime.InteropServices;
using UnityEngine;

namespace LeopotamGroup.Math {
    public static class MathFast {
        public const float PI = 3.141592654f;

        public const float PI_DIV_2 = 3.141592654f / 2f;

        public const float PI_2 = 3.141592654f * 2;

        const int SinTableSize = 4096;

        const float SinTableIndexFactor = SinTableSize / PI_2;

        static readonly float[] _sinTable;

        static readonly float[] _cosTable;

        [StructLayout (LayoutKind.Explicit)]
        struct FloatInt {
            [FieldOffset (0)]
            public float Float;

            [FieldOffset (0)]
            public int Int;
        }

        static MathFast () {
            _sinTable = new float[SinTableSize];
            _cosTable = new float[SinTableSize];
            for (var i = 0; i < SinTableSize; i++) {
                _sinTable[i] = (float) System.Math.Sin (i / (float) SinTableSize * PI_2);
                _cosTable[i] = (float) System.Math.Cos (i / (float) SinTableSize * PI_2);
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
            v = v - (long) (v / PI_2) * PI_2;
            return _sinTable[(int) (v * SinTableIndexFactor)];
        }

        public static float Cos (float v) {
            v = v - (long) (v / PI_2) * PI_2;
            return _cosTable[(int) (v * SinTableIndexFactor)];
        }
    }
}