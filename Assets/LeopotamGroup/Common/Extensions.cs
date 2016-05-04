//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System;
using System.Globalization;
using System.Text;
using UnityEngine;

namespace LeopotamGroup.Common {
    /// <summary>
    /// Holder of extensions / helpers.
    /// </summary>
    public static class Extensions {
        /// <summary>
        /// Find GameObject with name in recursive hierarchy.
        /// </summary>
        /// <returns>Transform of found GameObject.</returns>
        /// <param name="target">Root of search.</param>
        /// <param name="name">Name to search.</param>
        public static Transform FindRecursive (this Transform target, string name) {
            if (target == null || string.CompareOrdinal (target.name, name) == 0) {
                return target;
            }
            Transform retVal = null;
            foreach (Transform child in target) {
                retVal = child.FindRecursive (name);
                if (retVal != null) {
                    break;
                }
            }
            return retVal;
        }

        /// <summary>
        /// Normalized NumberFormatInfo.
        /// </summary>
        public static readonly NumberFormatInfo NumberFormatInfo = new NumberFormatInfo {
            NumberDecimalSeparator = "."
        };

        /// <summary>
        /// Convert number to normalized string with support of "kilo-million-billion" short names.
        /// </summary>
        /// <returns>Normalized string.</returns>
        /// <param name="data">Source Number.</param>
        public static string ToShortString (this int data) {
            if (data < 0) {
                data = -data;
            }
            if (data >= 1000000000) {
                return string.Format ("{0}B", Mathf.Floor (data / 10000000f) / 10f);
            }
            if (data >= 1000000) {
                return string.Format ("{0}M", Mathf.Floor (data / 100000f) / 10f);
            }
            if (data >= 1000) {
                return string.Format ("{0}k", Mathf.Floor (data / 100f) / 10f);
            }
            return data.ToString ();
        }

        /// <summary>
        /// Broadcast method with data to all active GameObjects.
        /// </summary>
        /// <param name="method">Method name.</param>
        /// <param name="data">Optional data.</param>
        public static void BroadcastToAll (string method, object data = null) {
            foreach (var go in UnityEngine.Object.FindObjectsOfType<GameObject> ()) {
                go.SendMessage (method, data, SendMessageOptions.DontRequireReceiver);
            }
        }

        /// <summary>
        /// Convert string to float.
        /// </summary>
        /// <returns>Float number.</returns>
        /// <param name="text">Source string.</param>
        public static float ToFloat (this string text) {
            return float.Parse (text, NumberFormatInfo);
        }

        /// <summary>
        /// Fast convert string to float. Fast, no GC allocation, no support for scientific format.
        /// </summary>
        /// <returns>Float number.</returns>
        /// <param name="text">Raw string.</param>
        public static float ToFloatUnchecked (this string text) {
            var retVal1 = 0f;
            var retVal2 = 0f;
            var sign = 1f;
            if (text != null) {
                var dir = 10f;
                int i;
                var iMax = text.Length;
                char c;
                for (i = 0; i < iMax; i++) {
                    c = text[i];
                    if (c >= '0' && c <= '9') {
                        retVal1 *= dir;
                        retVal1 += (c - '0');
                    } else {
                        if (c == '.') {
                            break;
                        } else {
                            if (c == '-') {
                                sign = -1f;
                            }
                        }
                    }
                }
                i++;
                dir = 0.1f;
                for (; i < iMax; i++) {
                    c = text[i];
                    if (c >= '0' && c <= '9') {
                        retVal2 += (c - '0') * dir;
                        dir *= 0.1f;
                    }
                }
            }
            return sign * (retVal1 + retVal2);
        }

        static readonly StringBuilder _floatToStrBuf = new StringBuilder (64);

        /// <summary>
        /// Convert float number to string. Fast, no support for scientific format.
        /// </summary>
        /// <returns>Normalized string.</returns>
        /// <param name="data">Data.</param>
        public static string ToNormalizedString (this float data) {
            lock (_floatToStrBuf) {
                const int prec_mul = 100000;
                _floatToStrBuf.Length = 0;
                var isNeg = data < 0f;
                if (isNeg) {
                    data = -data;
                }
                var v0 = (uint) data;
                var diff = (data - v0) * prec_mul;
                var v1 = (uint) diff;
                diff -= v1;
                if (diff > 0.5f) {
                    v1++;
                    if (v1 >= prec_mul) {
                        v1 = 0;
                        v0++;
                    }
                } else {
                    if (diff == 0.5f && (v1 == 0 || (v1 & 1) != 0)) {
                        v1++;
                    }
                }
                if (v1 > 0) {
                    var count = 5;
                    while ((v1 % 10) == 0) {
                        count--;
                        v1 /= 10;
                    }

                    do {
                        count--;
                        _floatToStrBuf.Append ((char) ((v1 % 10) + '0'));
                        v1 /= 10;
                    } while (v1 > 0);
                    while (count > 0) {
                        count--;
                        _floatToStrBuf.Append ('0');
                    }
                    _floatToStrBuf.Append ('.');
                }
                do {
                    _floatToStrBuf.Append ((char) ((v0 % 10) + '0'));
                    v0 /= 10;
                } while (v0 > 0);
                if (isNeg) {
                    _floatToStrBuf.Append ('-');
                }
                var i0 = 0;
                var i1 = _floatToStrBuf.Length - 1;
                char c;
                while (i1 > i0) {
                    c = _floatToStrBuf[i0];
                    _floatToStrBuf[i0] = _floatToStrBuf[i1];
                    _floatToStrBuf[i1] = c;
                    i0++;
                    i1--;
                }

                return _floatToStrBuf.ToString ();
            }
        }

        /// <summary>
        /// Convert string "rrggbb" to Color.
        /// </summary>
        /// <returns>Color.</returns>
        /// <param name="text">"rrggbb" string.</param>
        public static Color ToColor24 (this string text) {
            try {
                var data = Convert.ToInt32 (text.Length > 6 ? text.Substring (0, 6) : text, 16);
                return new Color (
                    ((data >> 16) & 0xff) / 255f,
                    ((data >> 8) & 0xff) / 255f,
                    (data & 0xff) / 255f,
                    1f);
            } catch {
                return Color.black;
            }
        }

        /// <summary>
        /// Convert string "rrggbbaa" to Color32.
        /// </summary>
        /// <returns>Color.</returns>
        /// <param name="text">"rrggbbaa" string.</param>
        public static Color ToColor32 (this string text) {
            try {
                var data = Convert.ToInt32 (text.Length > 8 ? text.Substring (0, 8) : text, 16);
                return new Color (
                    ((data >> 24) & 0xff) / 255f,
                    ((data >> 16) & 0xff) / 255f,
                    ((data >> 8) & 0xff) / 255f,
                    (data & 0xff) / 255f);
            } catch {
                return Color.black;
            }
        }

        /// <summary>
        /// Ensure that GammeObject have component.
        /// </summary>
        /// <returns>Wanted component.</returns>
        /// <param name="go">Target GameObject.</param>
        /// <typeparam name="T">Any unity-based component.</typeparam>
        public static T EnsureGetComponent<T> (this GameObject go) where T : Component {
            if (go != null) {
                var c = go.GetComponent<T> ();
                if (c == null) {
                    c = go.AddComponent<T> ();
                }
                return c;
            }
            return null;
        }
    }
}