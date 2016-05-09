//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System;
using UnityEngine;

namespace LeopotamGroup.Math {
    /// <summary>
    /// Vector2 struct with int32 fields.
    /// </summary>
    [Serializable]
    public struct Vector2i {
        /// <summary>
        /// X field.
        /// </summary>
        public int x;

        /// <summary>
        /// Y field.
        /// </summary>
        public int y;

        /// <summary>
        /// Static value of Vector2i(0, 0). No protection from external property changes, dont be stupid to do this!
        /// </summary>
        public static readonly Vector2i zero = new Vector2i (0, 0);

        /// <summary>
        /// Static value of Vector2i(1, 1). No protection from external property changes, dont be stupid to do this!
        /// </summary>
        public static readonly Vector2i one = new Vector2i (1, 1);

        /// <summary>
        /// Static value of Vector2i(0, 1). No protection from external property changes, dont be stupid to do this!
        /// </summary>
        public static readonly Vector2i up = new Vector2i (0, 1);

        /// <summary>
        /// Static value of Vector2i(0, -1). No protection from external property changes, dont be stupid to do this!
        /// </summary>
        public static readonly Vector2i down = new Vector2i (0, -1);

        /// <summary>
        /// Static value of Vector2i(-1, 0). No protection from external property changes, dont be stupid to do this!
        /// </summary>
        public static readonly Vector2i left = new Vector2i (-1, 0);

        /// <summary>
        /// Static value of Vector2i(1, 0). No protection from external property changes, dont be stupid to do this!
        /// </summary>
        public static readonly Vector2i right = new Vector2i (1, 0);

        /// <summary>
        /// Initialization with custom values for X/Y.
        /// </summary>
        /// <param name="inX">X value.</param>
        /// <param name="inY">Y value.</param>
        public Vector2i (int inX, int inY) {
            x = inX;
            y = inY;
        }

        /// <summary>
        /// Initialization from Vector2i instance.
        /// </summary>
        public Vector2i (Vector2i v) {
            x = v.x;
            y = v.y;
        }

        /// <summary>
        /// Initialization from Vector3i instance.
        /// </summary>
        public Vector2i (Vector3i v) {
            x = v.x;
            y = v.y;
        }

        /// <summary>
        /// Initialization from Vector2 instance.
        /// </summary>
        public Vector2i (Vector2 v) {
            x = Mathf.RoundToInt (v.x);
            y = Mathf.RoundToInt (v.y);
        }

        /// <summary>
        /// Initialization from Vector3 instance.
        /// </summary>
        public Vector2i (Vector3 v) {
            x = Mathf.RoundToInt (v.x);
            y = Mathf.RoundToInt (v.y);
        }

        /// <summary>
        /// Return square of vector length.
        /// </summary>
        public int SqrMagnitude {
            get { return x * x + y * y; }
        }

        /// <summary>
        /// Get hash code.
        /// </summary>
        public override int GetHashCode () {
            return base.GetHashCode ();
        }

        /// <summary>
        /// Is instance equals with specified one.
        /// </summary>
        /// <param name="rhs">Specified instance for comparation.</param>
        public override bool Equals (object rhs) {
            if (!(rhs is Vector2i)) {
                return false;
            }
            return this == (Vector2i) rhs;
        }

        /// <summary>
        /// Return formatted X/Y values.
        /// </summary>
        public override string ToString () {
            return string.Format ("({0}, {1})", x, y);
        }

        /// <summary>
        /// Combine new Vector2i from min values of two vectors.
        /// </summary>
        /// <param name="lhs">First vector.</param>
        /// <param name="rhs">Second vector.</param>
        public static Vector2i Min (Vector2i lhs, Vector2i rhs) {
            return new Vector2i (lhs.x < rhs.x ? lhs.x : rhs.x, lhs.y < rhs.y ? lhs.y : rhs.y);
        }

        /// <summary>
        /// Combine new Vector2i from max values of two vectors.
        /// </summary>
        /// <param name="lhs">First vector.</param>
        /// <param name="rhs">Second vector.</param>
        public static Vector2i Max (Vector2i lhs, Vector2i rhs) {
            return new Vector2i (lhs.x > rhs.x ? lhs.x : rhs.x, lhs.y > rhs.y ? lhs.y : rhs.y);
        }

        /// <summary>
        /// Return clamped version of specified vector with min/max range.
        /// </summary>
        /// <param name="value">Source vector.</param>
        /// <param name="min">Min value.</param>
        /// <param name="max">Max value.</param>
        public static Vector2i Clamp (Vector2i value, Vector2i min, Vector2i max) {
            return new Vector2i (Mathf.Clamp (value.x, min.x, max.x), Mathf.Clamp (value.y, min.y, max.y));
        }

        public static bool operator == (Vector2i lhs, Vector2i rhs) {
            return lhs.x == rhs.x && lhs.y == rhs.y;
        }

        public static bool operator != (Vector2i lhs, Vector2i rhs) {
            return !(lhs == rhs);
        }

        public static Vector2i operator - (Vector2i lhs) {
            return new Vector2i (-lhs.x, -lhs.y);
        }

        public static Vector2i operator - (Vector2i lhs, Vector2i rhs) {
            return new Vector2i (lhs.x - rhs.x, lhs.y - rhs.y);
        }

        public static Vector2i operator + (Vector2i lhs, Vector2i rhs) {
            return new Vector2i (lhs.x + rhs.x, lhs.y + rhs.y);
        }

        public static Vector2i operator * (Vector2i lhs, int rhs) {
            return new Vector2i (lhs.x * rhs, lhs.y * rhs);
        }

        public static Vector2i operator * (Vector2i lhs, float rhs) {
            return new Vector2i (Mathf.RoundToInt (lhs.x * rhs), Mathf.RoundToInt (lhs.y * rhs));
        }

        public static Vector2i operator * (Vector2i lhs, Vector2i rhs) {
            return new Vector2i (lhs.x * rhs.x, lhs.y * rhs.y);
        }

        public static Vector2i operator / (Vector2i lhs, Vector2i rhs) {
            return new Vector2i (lhs.x / rhs.x, lhs.y / rhs.y);
        }

        public static Vector2i operator / (Vector2i lhs, int rhs) {
            return new Vector2i (lhs.x / rhs, lhs.y / rhs);
        }

        public static implicit operator Vector3 (Vector2i lhs) {
            return new Vector3 (lhs.x, lhs.y, 0f);
        }

        public static implicit operator Vector2 (Vector2i lhs) {
            return new Vector2 (lhs.x, lhs.y);
        }

        public static implicit operator Vector2i (Vector3 lhs) {
            return new Vector2i (lhs);
        }

        public static implicit operator Vector2i (Vector2 lhs) {
            return new Vector2i (lhs);
        }

        public static explicit operator Vector2i (Vector3i lhs) {
            return new Vector2i (lhs);
        }
    }
}