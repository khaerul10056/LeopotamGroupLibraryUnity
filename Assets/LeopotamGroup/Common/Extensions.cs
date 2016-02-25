//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System;
using System.Globalization;
using System.Text;
using UnityEngine;

namespace LeopotamGroup.Common {
    public static class Extensions {
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

        public static readonly NumberFormatInfo NumberFormatInfo = new NumberFormatInfo
        {
            NumberDecimalSeparator = "."
        };

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

        static public void BroadcastToAll (string method) {
            foreach (var go in UnityEngine.Object.FindObjectsOfType<GameObject>()) {
                go.SendMessage (method, SendMessageOptions.DontRequireReceiver);
            }
        }

        static public float ToFloat (this string text) {
            return float.Parse (text, NumberFormatInfo);
        }

        static public float ToFloatUnchecked (this string text) {
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

        static public string ToNormalizedString (this float data) {
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

        static public Color ToColor24 (this string text) {
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

        static public Color ToColor32 (this string text) {
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

        static public T EnsureGetComponent<T> (this GameObject go) where T : Component {
            if (go != null) {
                var c = go.GetComponent <T> ();
                if (c == null) {
                    c = go.AddComponent <T> ();
                }
                return c;
            }
            return null;
        }
    }
}