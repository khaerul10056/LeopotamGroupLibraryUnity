//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System;
using UnityEngine;

namespace LeopotamGroup.Math {
    [Serializable]
    public struct Vector3i {
        public int x;

        public int y;

        public int z;

        // No protection from external property changes, dont be stupid to do this!
        public static readonly Vector3i Zero = new Vector3i (0, 0, 0);

        public static readonly Vector3i One = new Vector3i (1, 1, 1);

        public static readonly Vector3i Forward = new Vector3i (0, 0, 1);

        public static readonly Vector3i Back = new Vector3i (0, 0, -1);

        public static readonly Vector3i Up = new Vector3i (0, 1, 0);

        public static readonly Vector3i Down = new Vector3i (0, -1, 0);

        public static readonly Vector3i Left = new Vector3i (-1, 0, 0);

        public static readonly Vector3i Right = new Vector3i (1, 0, 0);

        public Vector3i (int inX, int inY, int inZ) {
            x = inX;
            y = inY;
            z = inZ;
        }

        public Vector3i (int inX, int inY) : this (inX, inY, 0) {
        }

        public Vector3i (Vector3i v) {
            x = v.x;
            y = v.y;
            z = v.z;
        }

        public Vector3i (Vector2i v) {
            x = v.x;
            y = v.y;
            z = 0;
        }

        public Vector3i (Vector3 v) {
            x = Mathf.RoundToInt (v.x);
            y = Mathf.RoundToInt (v.y);
            z = Mathf.RoundToInt (v.z);
        }

        public Vector3i (Vector2 v) {
            x = Mathf.RoundToInt (v.x);
            y = Mathf.RoundToInt (v.y);
            z = 0;
        }

        public int SqrMagnitude {
            get { return x * x + y * y + z * z; }
        }

        public Vector3i LossyNormalized {
            get {
                return Vector3i.LossyNormalize (this);
            }
        }

        public void LossyNormalize () {
            var num = Vector3i.LossyMagnitude (this);
            if (num > 0) {
                x /= num;
                y /= num;
                z /= num;
            } else {
                x = 0;
                y = 0;
                z = 0;
            }
        }

        public int LossyMagnituded {
            get { return LossyMagnitude (this); }
        }

        public static Vector3i LossyNormalize (Vector3i rhs) {
            var num = Vector3i.LossyMagnitude (rhs);
            if (num > 0) {
                return rhs / num;
            }
            return Vector3i.Zero;
        }

        public static int LossyMagnitude (Vector3i rhs) {
            return Mathf.RoundToInt (Mathf.Sqrt (rhs.SqrMagnitude));
        }

        public override int GetHashCode () {
            return base.GetHashCode ();
        }

        public override bool Equals (object rhs) {
            if (!(rhs is Vector3i)) {
                return false;
            }

            return this == (Vector3i) rhs;
        }

        public override string ToString () {
            return string.Format ("({0}, {1}, {2})", x, y, z);
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

        public static explicit operator Vector3 (Vector3i lhs) {
            return new Vector3 (lhs.x, lhs.y, lhs.z);
        }

        public static explicit operator Vector2 (Vector3i lhs) {
            return new Vector2 (lhs.x, lhs.y);
        }

        public static explicit operator Vector3i (Vector3 lhs) {
            return new Vector3i (lhs);
        }

        public static explicit operator Vector3i (Vector2 lhs) {
            return new Vector3i (lhs);
        }

        public static explicit operator Vector3i (Vector2i lhs) {
            return new Vector3i (lhs);
        }

        public static Vector3i Min (Vector3i lhs, Vector3i rhs) {
            return new Vector3i (Mathf.Min (lhs.x, rhs.x), Mathf.Min (lhs.y, rhs.y), Mathf.Min (lhs.z, rhs.z));
        }

        public static Vector3i Max (Vector3i lhs, Vector3i rhs) {
            return new Vector3i (Mathf.Max (lhs.x, rhs.x), Mathf.Max (lhs.y, rhs.y), Mathf.Max (lhs.z, rhs.z));
        }

        public static Vector3i Clamp (Vector3i value, Vector3i min, Vector3i max) {
            return new Vector3i (Mathf.Clamp (value.x, min.x, max.x), Mathf.Clamp (value.y, min.y, max.y), Mathf.Clamp (value.z, min.z, max.z));
        }
    }
}