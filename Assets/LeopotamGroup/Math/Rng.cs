//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using UnityEngine;

namespace LeopotamGroup.Math {
    /// <summary>
    /// Rng generator, mersenne twister based.
    /// </summary>
    public sealed class Rng {
        const int N = 624;

        const int M = 397;

        const ulong MatrixA = 0x9908b0dfUL;

        const ulong UpperMask = 0x80000000UL;

        const ulong LowerMask = 0x7fffffffUL;

        static readonly Rng _instance = new Rng ();

        readonly ulong[] _mt = new ulong[N];

        readonly ulong[] _mag01 = { 0x0UL, MatrixA };

        int _mti = N + 1;

        /// <summary>
        /// Default initialization.
        /// </summary>
        public Rng () : this ((long)(Time.realtimeSinceStartup * 100000)) {
        }

        /// <summary>
        /// Initialization with custom seed.
        /// </summary>
        /// <param name="seed">Seed.</param>
        public Rng (long seed) {
            SetSeed (seed);
        }

        ulong GetRandomUInt32 () {
            ulong y;
            if (_mti >= N) {
                int kk;
                if (_mti == N + 1) {
                    SetSeed (5489L);
                }
                for (kk = 0; kk < N - M; kk++) {
                    y = (_mt[kk] & UpperMask) | (_mt[kk + 1] & LowerMask);
                    _mt[kk] = _mt[kk + M] ^ (y >> 1) ^ _mag01[y & 0x1UL];
                }
                for (; kk < N - 1; kk++) {
                    y = (_mt[kk] & UpperMask) | (_mt[kk + 1] & LowerMask);
                    _mt[kk] = _mt[kk + (M - N)] ^ (y >> 1) ^ _mag01[y & 0x1UL];
                }
                y = (_mt[N - 1] & UpperMask) | (_mt[0] & LowerMask);
                _mt[N - 1] = _mt[M - 1] ^ (y >> 1) ^ _mag01[y & 0x1UL];
                _mti = 0;
            }
            y = _mt[_mti++];
            y ^= (y >> 11);
            y ^= (y << 7) & 0x9d2c5680UL;
            y ^= (y << 15) & 0xefc60000UL;
            y ^= (y >> 18);
            return y;
        }

        /// <summary>
        /// Set new seed.
        /// </summary>
        /// <param name="seed">Seed.</param>
        public void SetSeed (long seed) {
            _mt[0] = (ulong)seed & 0xffffffffUL;
            for (_mti = 1; _mti < N; _mti++) {
                _mt[_mti] = (1812433253UL * (_mt[_mti - 1] ^ (_mt[_mti - 1] >> 30)) + (ulong)_mti) & 0xffffffffUL;
            }
        }

        /// <summary>
        /// Get int32 random number from range [0, max).
        /// </summary>
        /// <returns>Random int32 value.</returns>
        /// <param name="max">Max value (excluded).</param>
        public int GetInt32 (int max) {
            return (int)(GetRandomUInt32 () * (max / 4294967296.0));
        }

        /// <summary>
        /// Get int32 random number from range [min, max).
        /// </summary>
        /// <returns>Random int32 value.</returns>
        /// <param name="min">Min value.</param>
        /// <param name="max">Max value (excluded).</param>
        public int GetInt32 (int min, int max) {
            if (min > max) {
                var t = min;
                min = max;
                max = t;
            }
            return min + GetInt32 (max - min);
        }

        /// <summary>
        /// Get float random number from range [0, 1) or [0, 1] for includeOne=true.
        /// </summary>
        /// <param name="includeOne">Include 1 value for searching.</param>
        public float GetFloat (bool includeOne = true) {
            return (float)(GetRandomUInt32 () * (1.0 / (includeOne ? 4294967295.0 : 4294967296.0)));
        }

        /// <summary>
        /// Get float random number from range [min, max) or [min, max] for includeMax=true.
        /// </summary>
        /// <returns>The float.</returns>
        /// <param name="min">Min value.</param>
        /// <param name="max">Max value.</param>
        /// <param name="includeMax">Include max value for searching.</param>
        public float GetFloat (float min, float max, bool includeMax = true) {
            if (min > max) {
                var t = min;
                min = max;
                max = t;
            }
            return min + GetFloat (includeMax) * (max - min);
        }

        /// <summary>
        /// Set new seed for singleton rng.
        /// </summary>
        /// <returns>The seed static.</returns>
        /// <param name="seed">Seed.</param>
        public static void SetSeedStatic (long seed) {
            _instance.SetSeed (seed);
        }

        /// <summary>
        /// Get int32 random number from range [0, n) from singleton rng.
        /// </summary>
        /// <returns>Random int32.</returns>
        /// <param name="n">.</param>
        public static int GetInt32Static (int n) {
            return _instance.GetInt32 (n);
        }

        /// <summary>
        /// Get int32 random number from range [min, max) from singleton rng.
        /// </summary>
        /// <returns>Random int32 value.</returns>
        /// <param name="min">Min value.</param>
        /// <param name="max">Max value (excluded).</param>
        public static int GetInt32Static (int min, int max) {
            return _instance.GetInt32 (min, max);
        }

        /// <summary>
        /// Get float random number from range [0, 1) or [0, 1] for includeOne=true from singleton rng.
        /// </summary>
        /// <param name="includeOne">Include 1 value for searching.</param>
        public static float GetFloatStatic (bool includeOne = true) {
            return _instance.GetFloat (includeOne);
        }

        /// <summary>
        /// Get float random number from range [min, max) or [min, max] for includeMax=true from singleton rng.
        /// </summary>
        /// <returns>The float.</returns>
        /// <param name="min">Min value.</param>
        /// <param name="max">Max value.</param>
        /// <param name="includeMax">Include max value for searching.</param>
        public static float GetFloatStatic (float min, float max, bool includeMax = true) {
            return _instance.GetFloat (min, max, includeMax);
        }
    }
}