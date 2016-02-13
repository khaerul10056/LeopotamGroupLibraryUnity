//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using UnityEngine;

namespace LeopotamGroup.Math {
    public sealed class Rng {
        const int N = 624;

        const int M = 397;

        const ulong MatrixA = 0x9908b0dfUL;

        const ulong UpperMask = 0x80000000UL;

        const ulong LowerMask = 0x7fffffffUL;

        readonly ulong[] _mt = new ulong[N];

        readonly ulong[] _mag01 = { 0x0UL, MatrixA };

        int _mti = N + 1;

        public Rng () : this ((ulong) (Time.realtimeSinceStartup * 100000)) {
        }

        public Rng (ulong seed) {
            SetSeed (seed);
        }

        static readonly Rng _instance = new Rng ();

        public static void SetSeedStatic (ulong seed) {
            _instance.SetSeed (seed);
        }

        /// <summary>
        /// Get int32 random number from range [0, n).
        /// </summary>
        public static int GetInt32Static (int n) {
            return _instance.GetInt32 (n);
        }

        /// <summary>
        /// Get float random number from range [0, 1) or [0, 1] for includeOne=true.
        /// </summary>
        public static float GetFloatStatic (bool includeOne = true) {
            return _instance.GetFloat (includeOne);
        }

        public void SetSeed (ulong seed) {
            _mt[0] = seed & 0xffffffffUL;
            for (_mti = 1; _mti < N; _mti++) {
                _mt[_mti] = (1812433253UL * (_mt[_mti - 1] ^ (_mt[_mti - 1] >> 30)) + (ulong) _mti) & 0xffffffffUL; 
            }
        }

        /// <summary>
        /// Get int32 random number from range [0, n).
        /// </summary>
        public int GetInt32 (int n) {
            return (int) (GetRandomUInt32 () * (n / 4294967296.0));
        }

        /// <summary>
        /// Get float random number from range [0, 1) or [0, 1] for includeOne=true.
        /// </summary>
        public float GetFloat (bool includeOne = true) {
            return (float) (GetRandomUInt32 () * (1.0 / (includeOne ? 4294967295.0 : 4294967296.0)));
        }

        ulong GetRandomUInt32 () {
            ulong y;
            if (_mti >= N) {
                int kk;
                if (_mti == N + 1) {         
                    SetSeed (5489UL);   
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
    }
}