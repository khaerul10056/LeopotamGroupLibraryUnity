//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System.Runtime.InteropServices;
using UnityEngine;

namespace LeopotamGroup.Math {
    /// <summary>
    /// Holder of extensions / helpers.
    /// </summary>
    public static class MathFast {
        /// <summary>
        /// PI approximation.
        /// </summary>
        public const float PI = 3.141592654f;

        /// <summary>
        /// PI/2 approximation.
        /// </summary>
        public const float PI_DIV_2 = 3.141592654f * 0.5f;

        /// <summary>
        /// PI*2 approximation.
        /// </summary>
        public const float PI_2 = PI * 2f;

        /// <summary>
        /// Radians to Degrees conversion multiplier.
        /// </summary>
        public const float Rad2Deg = 180f / PI;

        /// <summary>
        /// Degrees to Radians conversion multiplier.
        /// </summary>
        public const float Deg2Rad = PI / 180f;

        const int _sinCosIndexMask = ~(-1 << 12);

        static readonly float[] _sinCache;

        static readonly float[] _cosCache;

        const float _sinCosIndexFactor = SinCosCacheSize / PI_2;

        const int SinCosCacheSize = _sinCosIndexMask + 1;

        const int Atan2Size = 1024;

        const int Atan2NegSize = -Atan2Size;

        static readonly float[] _atan2CachePPY = new float[Atan2Size + 1];

        static readonly float[] _atan2CachePPX = new float[Atan2Size + 1];

        static readonly float[] _atan2CachePNY = new float[Atan2Size + 1];

        static readonly float[] _atan2CachePNX = new float[Atan2Size + 1];

        static readonly float[] _atan2CacheNPY = new float[Atan2Size + 1];

        static readonly float[] _atan2CacheNPX = new float[Atan2Size + 1];

        static readonly float[] _atan2CacheNNY = new float[Atan2Size + 1];

        static readonly float[] _atan2CacheNNX = new float[Atan2Size + 1];

        [StructLayout (LayoutKind.Explicit)]
        struct FloatInt {
            [FieldOffset (0)]
            public float Float;

            [FieldOffset (0)]
            public int Int;
        }

        static MathFast () {
            // Sin/Cos
            _sinCache = new float[SinCosCacheSize];
            _cosCache = new float[SinCosCacheSize];
            int i;
            for (i = 0; i < SinCosCacheSize; i++) {
                _sinCache[i] = (float) System.Math.Sin ((i + 0.5f) / (float) SinCosCacheSize * PI_2);
                _cosCache[i] = (float) System.Math.Cos ((i + 0.5f) / (float) SinCosCacheSize * PI_2);
            }

            var factor = SinCosCacheSize / 360f;
            for (i = 0; i < 360; i += 90) {
                _sinCache[(int) (i * factor) & _sinCosIndexMask] = (float) System.Math.Sin (i * PI / 180f);
                _cosCache[(int) (i * factor) & _sinCosIndexMask] = (float) System.Math.Cos (i * PI / 180f);
            }

            // Atan2

            var invAtan2Size = 1f / Atan2Size;
            for (i = 0; i <= Atan2Size; i++) {
                _atan2CachePPY[i] = (float) System.Math.Atan (i * invAtan2Size);
                _atan2CachePPX[i] = PI_DIV_2 - _atan2CachePPY[i];
                _atan2CachePNY[i] = -_atan2CachePPY[i];
                _atan2CachePNX[i] = _atan2CachePPY[i] - PI_DIV_2;
                _atan2CacheNPY[i] = PI - _atan2CachePPY[i];
                _atan2CacheNPX[i] = _atan2CachePPY[i] + PI_DIV_2;
                _atan2CacheNNY[i] = _atan2CachePPY[i] - PI;
                _atan2CacheNNX[i] = -PI_DIV_2 - _atan2CachePPY[i];
            }
        }

        /// <summary>
        /// Fast Vector2 normalization with 0.001 threshold error.
        /// </summary>
        /// <returns>Normalized Vector2.</returns>
        public static Vector2 NormalizedFast (this Vector2 v) {
            var wrapper = new FloatInt ();
            wrapper.Float = v.x * v.x + v.y * v.y;
            wrapper.Int = 0x5f3759df - (wrapper.Int >> 1);
            v.x *= wrapper.Float;
            v.y *= wrapper.Float;
            return v;
        }

        /// <summary>
        /// Fast Vector3 normalization with 0.001 threshold error.
        /// </summary>
        /// <returns>Normalized Vector3.</returns>
        public static Vector3 NormalizedFast (this Vector3 v) {
            var wrapper = new FloatInt ();
            wrapper.Float = v.x * v.x + v.y * v.y + v.z * v.z;
            wrapper.Int = 0x5f3759df - (wrapper.Int >> 1);
            v.x *= wrapper.Float;
            v.y *= wrapper.Float;
            v.z *= wrapper.Float;
            return v;
        }

        /// <summary>
        /// Fast Sin with 0.0003 threshold error.
        /// </summary>
        /// <param name="v">Angle in radians.</param>
        public static float Sin (float v) {
            return _sinCache[(int) (v * _sinCosIndexFactor) & _sinCosIndexMask];
        }

        /// <summary>
        /// Fast Cos with 0.0003 threshold error.
        /// </summary>
        /// <param name="v">Angle in radians.</param>
        public static float Cos (float v) {
            return _cosCache[(int) (v * _sinCosIndexFactor) & _sinCosIndexMask];
        }

        /// <summary>
        /// Fast Atan2 with 0.02 threshold error.
        /// </summary>
        public static float Atan2 (float y, float x) {
            if (x >= 0) {
                if (y >= 0) {
                    if (x >= y) {
                        return _atan2CachePPY[(int) (Atan2Size * y / x + 0.5)];
                    } else {
                        return _atan2CachePPX[(int) (Atan2Size * x / y + 0.5)];
                    }
                } else {
                    if (x >= -y) {
                        return _atan2CachePNY[(int) (Atan2NegSize * y / x + 0.5)];
                    } else {
                        return _atan2CachePNX[(int) (Atan2NegSize * x / y + 0.5)];
                    }
                }
            } else {
                if (y >= 0) {
                    if (-x >= y) {
                        return _atan2CacheNPY[(int) (Atan2NegSize * y / x + 0.5)];
                    } else {
                        return _atan2CacheNPX[(int) (Atan2NegSize * x / y + 0.5)];
                    }
                } else {
                    if (x <= y) {
                        return _atan2CacheNNY[(int) (Atan2Size * y / x + 0.5)];
                    } else {
                        return _atan2CacheNNX[(int) (Atan2Size * x / y + 0.5)];
                    }
                }
            }
        }
    }
}