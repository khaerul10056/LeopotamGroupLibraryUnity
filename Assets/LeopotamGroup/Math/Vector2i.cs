//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d License
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System;
using UnityEngine;

namespace LeopotamGroup.Math {
    [Serializable]
    public struct Vector2i {
        public int x;

        public int y;

        // No protection from external property changes, dont be stupid to do this!
        public static readonly Vector2i Zero = new Vector2i (0, 0);

        public static readonly Vector2i One = new Vector2i (1, 1);

        public static readonly Vector2i Up = new Vector2i (0, 1);

        public static readonly Vector2i Down = new Vector2i (0, -1);

        public static readonly Vector2i Left = new Vector2i (-1, 0);

        public static readonly Vector2i Right = new Vector2i (1, 0);

        public Vector2i (int inX, int inY) {
            x = inX;
            y = inY;
        }

        public Vector2i (Vector3i v) {
            x = v.x;
            y = v.y;
        }

        public Vector2i (Vector2i v) {
            x = v.x;
            y = v.y;
        }

        public Vector2i (Vector3 v) {
            x = Mathf.RoundToInt (v.x);
            y = Mathf.RoundToInt (v.y);
        }

        public Vector2i (Vector2 v) {
            x = Mathf.RoundToInt (v.x);
            y = Mathf.RoundToInt (v.y);
        }

        public Vector2i Set (int inX, int inY) {
            x = inX;
            y = inY;
            return this;
        }

        public int SqrMagnitude {
            get { return x * x + y * y; }
        }

        public Vector2i LossyNormalized {
            get {
                return Vector2i.LossyNormalize (this);
            }
        }

        public void LossyNormalize () {
            var num = Vector2i.LossyMagnitude (this);
            if (num > 0) {
                this /= num;
            } else {
                this = Vector2i.Zero;
            }
        }

        public static Vector2i LossyNormalize (Vector2i rhs) {
            var num = Vector2i.LossyMagnitude (rhs);
            if (num > 0) {
                return rhs / num;
            }
            return Vector2i.Zero;
        }

        public static int LossyMagnitude (Vector2i rhs) {
            return Mathf.RoundToInt (Mathf.Sqrt (rhs.x * rhs.x + rhs.y * rhs.y));
        }

        public override int GetHashCode () {
            return base.GetHashCode ();
        }

        public override bool Equals (object rhs) {
            if (!(rhs is Vector2i)) {
                return false;
            }

            return this == (Vector2i) rhs;
        }

        public override string ToString () {
            return string.Format ("({0:D3}, {1:D3})", x, y);
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

        public static explicit operator Vector3 (Vector2i lhs) {
            return new Vector3 (lhs.x, lhs.y, 0f);
        }

        public static explicit operator Vector2 (Vector2i lhs) {
            return new Vector2 (lhs.x, lhs.y);
        }

        public static explicit operator Vector2i (Vector3 lhs) {
            return new Vector2i (lhs);
        }

        public static explicit operator Vector2i (Vector2 lhs) {
            return new Vector2i (lhs);
        }

        public static explicit operator Vector2i (Vector3i lhs) {
            return new Vector2i (lhs);
        }

        public static Vector2i Min (Vector2i lhs, Vector2i rhs) {
            return new Vector2i (Mathf.Min (lhs.x, rhs.x), Mathf.Min (lhs.y, rhs.y));
        }

        public static Vector2i Max (Vector2i lhs, Vector2i rhs) {
            return new Vector2i (Mathf.Max (lhs.x, rhs.x), Mathf.Max (lhs.y, rhs.y));
        }

        public static Vector2i Clamp (Vector2i value, Vector2i min, Vector2i max) {
            return new Vector2i (Mathf.Clamp (value.x, min.x, max.x), Mathf.Clamp (value.y, min.y, max.y));
        }
    }
}