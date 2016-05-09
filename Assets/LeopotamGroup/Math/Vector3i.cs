//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System;
using UnityEngine;

namespace LeopotamGroup.Math {
    /// <summary>
    /// Vector3 struct with int32 fields.
    /// </summary>
    [Serializable]
    public struct Vector3i {
        /// <summary>
        /// X field.
        /// </summary>
        public int x;

        /// <summary>
        /// Y field.
        /// </summary>
        public int y;

        /// <summary>
        /// Z field.
        /// </summary>
        public int z;

        /// <summary>
        /// Static value of Vector3i(0, 0, 0). No protection from external property changes, dont be stupid to do this!
        /// </summary>
        public static readonly Vector3i zero = new Vector3i (0, 0, 0);

        /// <summary>
        /// Static value of Vector3i(1, 1, 1). No protection from external property changes, dont be stupid to do this!
        /// </summary>
        public static readonly Vector3i one = new Vector3i (1, 1, 1);

        /// <summary>
        /// Static value of Vector3i(0, 0, 1). No protection from external property changes, dont be stupid to do this!
        /// </summary>
        public static readonly Vector3i forward = new Vector3i (0, 0, 1);

        /// <summary>
        /// Static value of Vector3i(0, 0, -1). No protection from external property changes, dont be stupid to do this!
        /// </summary>
        public static readonly Vector3i back = new Vector3i (0, 0, -1);

        /// <summary>
        /// Static value of Vector3i(0, 1, 0). No protection from external property changes, dont be stupid to do this!
        /// </summary>
        public static readonly Vector3i up = new Vector3i (0, 1, 0);

        /// <summary>
        /// Static value of Vector3i(0, -1, 0). No protection from external property changes, dont be stupid to do this!
        /// </summary>
        public static readonly Vector3i down = new Vector3i (0, -1, 0);

        /// <summary>
        /// Static value of Vector3i(-1, 0, 0). No protection from external property changes, dont be stupid to do this!
        /// </summary>
        public static readonly Vector3i left = new Vector3i (-1, 0, 0);

        /// <summary>
        /// Static value of Vector3i(1, 0, 0). No protection from external property changes, dont be stupid to do this!
        /// </summary>
        public static readonly Vector3i right = new Vector3i (1, 0, 0);

        /// <summary>
        /// Initialization with custom values for X/Y/Z.
        /// </summary>
        /// <param name="inX">X value.</param>
        /// <param name="inY">Y value.</param>
        /// <param name="inZ">Z value.</param>
        public Vector3i (int inX, int inY, int inZ) {
            x = inX;
            y = inY;
            z = inZ;
        }

        /// <summary>
        /// Initialization with custom values for X/Y with Z=0.
        /// </summary>
        /// <param name="inX">X value.</param>
        /// <param name="inY">Y value.</param>
        public Vector3i (int inX, int inY) : this (inX, inY, 0) {
        }

        /// <summary>
        /// Initialization from Vector2i instance.
        /// </summary>
        public Vector3i (Vector2i v) {
            x = v.x;
            y = v.y;
            z = 0;
        }

        /// <summary>
        /// Initialization from Vector3i instance.
        /// </summary>
        public Vector3i (Vector3i v) {
            x = v.x;
            y = v.y;
            z = v.z;
        }

        /// <summary>
        /// Initialization from Vector2 instance.
        /// </summary>
        public Vector3i (Vector2 v) {
            x = Mathf.RoundToInt (v.x);
            y = Mathf.RoundToInt (v.y);
            z = 0;
        }

        /// <summary>
        /// Initialization from Vector3 instance.
        /// </summary>
        public Vector3i (Vector3 v) {
            x = Mathf.RoundToInt (v.x);
            y = Mathf.RoundToInt (v.y);
            z = Mathf.RoundToInt (v.z);
        }

        /// <summary>
        /// Return square of vector length.
        /// </summary>
        public int SqrMagnitude {
            get { return x * x + y * y + z * z; }
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
            if (!(rhs is Vector3i)) {
                return false;
            }
            return this == (Vector3i) rhs;
        }

        /// <summary>
        /// Return formatted X/Y/Z values.
        /// </summary>
        public override string ToString () {
            return string.Format ("({0}, {1}, {2})", x, y, z);
        }

        /// <summary>
        /// Combine new Vector3i from min values of two vectors.
        /// </summary>
        /// <param name="lhs">First vector.</param>
        /// <param name="rhs">Second vector.</param>
        public static Vector3i Min (Vector3i lhs, Vector3i rhs) {
            return new Vector3i (lhs.x < rhs.x ? lhs.x : rhs.x, lhs.x < rhs.x ? lhs.y : rhs.y, lhs.z < rhs.z ? lhs.z : rhs.z);
        }

        /// <summary>
        /// Combine new Vector2i from max values of two vectors.
        /// </summary>
        /// <param name="lhs">First vector.</param>
        /// <param name="rhs">Second vector.</param>
        public static Vector3i Max (Vector3i lhs, Vector3i rhs) {
            return new Vector3i (lhs.x > rhs.x ? lhs.x : rhs.x, lhs.x > rhs.x ? lhs.y : rhs.y, lhs.z > rhs.z ? lhs.z : rhs.z);
        }

        /// <summary>
        /// Return clamped version of specified vector with min/max range.
        /// </summary>
        /// <param name="value">Source vector.</param>
        /// <param name="min">Min value.</param>
        /// <param name="max">Max value.</param>
        public static Vector3i Clamp (Vector3i value, Vector3i min, Vector3i max) {
            return new Vector3i (Mathf.Clamp (value.x, min.x, max.x), Mathf.Clamp (value.y, min.y, max.y), Mathf.Clamp (value.z, min.z, max.z));
        }

        public static bool operator == (Vector3i lhs, Vector3i rhs) {
            return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
        }

        public static bool operator != (Vector3i lhs, Vector3i rhs) {
            return !(lhs == rhs);
        }

        public static Vector3i operator - (Vector3i lhs) {
            return new Vector3i (-lhs.x, -lhs.y, -lhs.z);
        }

        public static Vector3i operator - (Vector3i lhs, Vector3i rhs) {
            return new Vector3i (lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
        }

        public static Vector3i operator + (Vector3i lhs, Vector3i rhs) {
            return new Vector3i (lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
        }

        public static Vector3i operator * (Vector3i lhs, int rhs) {
            return new Vector3i (lhs.x * rhs, lhs.y * rhs, lhs.z * rhs);
        }

        public static Vector3i operator * (Vector3i lhs, float rhs) {
            return new Vector3i (Mathf.RoundToInt (lhs.x * rhs), Mathf.RoundToInt (lhs.y * rhs), Mathf.RoundToInt (lhs.z * rhs));
        }

        public static Vector3i operator * (Vector3i lhs, Vector3i rhs) {
            return new Vector3i (lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z);
        }

        public static Vector3i operator / (Vector3i lhs, Vector3i rhs) {
            return new Vector3i (lhs.x / rhs.x, lhs.y / rhs.y, lhs.z / rhs.z);
        }

        public static Vector3i operator / (Vector3i lhs, int rhs) {
            return new Vector3i (lhs.x / rhs, lhs.y / rhs, lhs.z / rhs);
        }

        public static implicit operator Vector3 (Vector3i lhs) {
            return new Vector3 (lhs.x, lhs.y, lhs.z);
        }

        public static implicit operator Vector2 (Vector3i lhs) {
            return new Vector2 (lhs.x, lhs.y);
        }

        public static implicit operator Vector3i (Vector3 lhs) {
            return new Vector3i (lhs);
        }

        public static implicit operator Vector3i (Vector2 lhs) {
            return new Vector3i (lhs);
        }

        public static implicit operator Vector3i (Vector2i lhs) {
            return new Vector3i (lhs);
        }
    }
}