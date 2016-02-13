using LeopotamGroup.Common;
using UnityEngine;

namespace LeopotamGroup.Examples.Common.Extensions {
    public class ExtensionsTest : MonoBehaviour {
        void Start () {
            IntToShortStringTest ();
            FloatToNormalizedStringTest ();
            StringToFloatTest ();
            StringToFloatUncheckedTest ();
            StringToColor24Test ();
            StringToColor32Test ();
        }

        void IntToShortStringTest () {
            foreach (var item in new [] {
                0,
                123,
                1234,
                1234567,
                -1234567,
                1234567890
            }) {
                Debug.LogFormat ("{0}.ToShortString = {1}", item, item.ToShortString ());
            }
        }

        void FloatToNormalizedStringTest () {
            foreach (var item in new [] {
                0f,
                123f,
                123.45678f,
                123.45670f,
                -123.123f,
                0.12345f,
                0.00005f
            }) {
                Debug.LogFormat ("{0:F5}.ToNormalizedString = {1}", item, item.ToNormalizedString ());
            }
        }

        void StringToFloatTest () {
            foreach (var item in new [] {
                "0",
                "123",
                "123.45678",
                "123.45670",
                "-123.123",
                "0.12345",
                "0.00005"
            }) {
                Debug.LogFormat ("{0}.ToFloat(invariant culture) = {1:0.#####}", item, item.ToFloat ());
            }
        }

        void StringToFloatUncheckedTest () {
            foreach (var item in new [] {
                "0",
                "123",
                "123.45678",
                "123.45670",
                "-123.123",
                "0.12345",
                "0.00005"
            }) {
                Debug.LogFormat ("{0}.ToFloatUnchecked(invariant culture, GC optimized, [digits | '.' | '-']) = {1:0.#####}", item, item.ToFloatUnchecked ());
            }
        }

        void StringToColor24Test () {
            foreach (var item in new [] {
                "000000",
                "ffffff",
                "ff0000",
                "00ff00",
                "0000ff",
                "ff00ff"
            }) {
                Debug.LogFormat ("{0}.ToColor24 = {1}", item, item.ToColor24 ());
            }
        }

        void StringToColor32Test () {
            foreach (var item in new [] {
                "00000000",
                "ffffffff",
                "ff0000ff",
                "00ff0077",
                "0000ffff",
                "ff00ffff"
            }) {
                Debug.LogFormat ("{0}.ToColor32 = {1}", item, item.ToColor32 ());
            }
        }
    }
}