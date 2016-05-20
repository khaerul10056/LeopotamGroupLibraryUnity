//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System.Diagnostics;
using UnityEngine;

namespace LeopotamGroup.EditorHelpers {
    /// <summary>
    /// Replacement for unity Debug class with automatic cleanup on create non-editor build.
    /// </summary>
    public static class UnityDebug {
        /// <summary>
        /// Log data as info.
        /// </summary>
        /// <param name="arg">Data.</param>
        [Conditional ("UNITY_EDITOR")]
        public static void LogInfo (object arg) {
            LogInternal (LogType.Log, "{0}", arg);
        }

        /// <summary>
        /// Log data as info with formatting mask.
        /// </summary>
        /// <param name="format">Format mask.</param>
        /// <param name="args">Arguments.</param>
        [Conditional ("UNITY_EDITOR")]
        public static void LogInfo (string format, params object[] args) {
            if (args != null && args.Length > 0) {
                LogInternal (LogType.Log, format, args);
            } else {
                LogInternal (LogType.Log, "{0}", format);
            }
        }

        /// <summary>
        /// Log data as warning.
        /// </summary>
        /// <param name="arg">Argument.</param>
        [Conditional ("UNITY_EDITOR")]
        public static void LogWarning (object arg) {
            LogInternal (LogType.Warning, "{0}", arg);
        }

        /// <summary>
        /// Log data as warning with formatting mask.
        /// </summary>
        /// <param name="format">Format mask.</param>
        /// <param name="args">Arguments.</param>
        [Conditional ("UNITY_EDITOR")]
        public static void LogWarning (string format, params object[] args) {
            if (args != null && args.Length > 0) {
                LogInternal (LogType.Warning, format, args);
            } else {
                LogInternal (LogType.Warning, "{0}", format);
            }
        }

        /// <summary>
        /// Log data as error.
        /// </summary>
        /// <param name="arg">Argument.</param>
        [Conditional ("UNITY_EDITOR")]
        public static void LogError (object arg) {
            LogInternal (LogType.Error, "{0}", arg);
        }

        /// <summary>
        /// Log data as error with formatting mask.
        /// </summary>
        /// <param name="format">Format mask.</param>
        /// <param name="args">Arguments.</param>
        [Conditional ("UNITY_EDITOR")]
        public static void LogError (string format, params object[] args) {
            if (args != null && args.Length > 0) {
                LogInternal (LogType.Error, format, args);
            } else {
                LogInternal (LogType.Error, "{0}", format);
            }
        }

        [Conditional ("UNITY_EDITOR")]
        static void LogInternal (LogType logType, string format, params object[] args) {
#if UNITY_EDITOR
            UnityEngine.Debug.logger.LogFormat (logType, format, args);
#endif
        }

        /// <summary>
        /// Draw debug 3d-line.
        /// </summary>
        /// <param name="start">Start position.</param>
        /// <param name="end">End position.</param>
        /// <param name="color">Color.</param>
        /// <param name="duration">Duration.</param>
        /// <param name="depthTest">Use depth test.</param>
        [Conditional ("UNITY_EDITOR")]
        public static void DrawLine (Vector3 start, Vector3 end, Color color, float duration = 1f, bool depthTest = true) {
#if UNITY_EDITOR
            UnityEngine.Debug.DrawLine (start, end, color, duration, depthTest);
#endif
        }

        /// <summary>
        /// Draw debug 3d-ray.
        /// </summary>
        /// <param name="start">Start position.</param>
        /// <param name="dir">Direction.</param>
        /// <param name="color">Color.</param>
        /// <param name="duration">Duration.</param>
        /// <param name="depthTest">Use depth test.</param>
        [Conditional ("UNITY_EDITOR")]
        public static void DrawRay (Vector3 start, Vector3 dir, Color color, float duration = 1f, bool depthTest = true) {
#if UNITY_EDITOR
            UnityEngine.Debug.DrawRay (start, dir, color, duration, depthTest);
#endif
        }

        /// <summary>
        /// Assert specified condition and log message.
        /// </summary>
        /// <param name="condition">Condition.</param>
        /// <param name="arg">Error message.</param>
        [Conditional ("UNITY_EDITOR")]
        public static void Assert (bool condition, string arg) {
            AssertInternal (condition, "{0}", arg);
        }

        /// <summary>
        /// Assert specified condition and log formatted message.
        /// </summary>
        /// <param name="condition">Condition.</param>
        /// <param name="format">Format mask.</param>
        /// <param name="args">Arguments.</param>
        [Conditional ("UNITY_EDITOR")]
        public static void Assert (bool condition, string format, params object[] args) {
            AssertInternal (condition, format, args);
        }

        [Conditional ("UNITY_EDITOR")]
        static void AssertInternal (bool condition, string format, params object[] args) {
#if UNITY_EDITOR
            UnityEngine.Debug.AssertFormat (condition, format, args);
            if (!condition) {
                UnityEditor.EditorApplication.isPaused = true;
                throw new UnityException ("Editor paused after assert.");
            }
#endif
        }
    }
}