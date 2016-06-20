//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace LeopotamGroup.Gui.Common {
    /// <summary>
    /// Main class of gui system.
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent (typeof (Camera))]
    public sealed class GuiSystem : MonoBehaviour {
        /// <summary>
        /// Virtual screen height.
        /// </summary>
        /// <value>The height of the screen.</value>
        public int ScreenHeight {
            get { return _screenHeight; }
            set {
                if (value > 0 && value != _screenHeight) {
                    _screenHeight = value;
                    _isChanged = true;
                }
            }
        }

        /// <summary>
        /// Clear flags for gui camera.
        /// </summary>
        /// <value>The clear flags.</value>
        public CameraClearFlags ClearFlags {
            get { return _clearFlags; }
            set {
                if (value != _clearFlags) {
                    _clearFlags = value;
                    _isChanged = true;
                }
            }
        }

        /// <summary>
        /// Background color for gui camera.
        /// </summary>
        /// <value>The color of the background.</value>
        public Color BackgroundColor {
            get { return _backgroundColor; }
            set {
                if (value != _backgroundColor) {
                    _backgroundColor = value;
                    _isChanged = true;
                }
            }
        }

        /// <summary>
        /// Culling mask for gui camera.
        /// </summary>
        /// <value>The culling mask.</value>
        public LayerMask CullingMask {
            get { return _cullingMask; }
            set {
                if (value != _cullingMask) {
                    _cullingMask = value;
                    _isChanged = true;
                }
            }
        }

        /// <summary>
        /// Depth order for gui camera.
        /// </summary>
        /// <value>The depth.</value>
        public int Depth {
            get { return _depth; }
            set {
                if (value != _depth) {
                    _depth = value;
                    _isChanged = true;
                }
            }
        }

        /// <summary>
        /// Gui camera reference.
        /// </summary>
        /// <value>The camera.</value>
        public Camera Camera {
            get {
                if (_camera == null) {
                    FixCamera ();
                }
                return _camera;
            }
        }

        /// <summary>
        /// Is user input locked.
        /// </summary>
        public bool IsInputLocked = false;

        /// <summary>
        /// Singleton of GuiSystem.
        /// </summary>
        /// <value>The instance.</value>
        public static GuiSystem Instance {
            get {
                if (_instance == null) {
                    _instance = FindObjectOfType <GuiSystem> ();
                    if (_instance == null) {
                        var go = new GameObject ("GuiSystem");
                        go.layer = GuiConsts.DefaultGuiLayer;
                        go.AddComponent <GuiSystem> ().CullingMask = GuiConsts.DefaultGuiLayerMask;
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Save checking for singleton instance availability.
        /// </summary>
        /// <returns>Instance exists.</returns>
        public static bool IsInstanceCreated () {
            return _instance != null;
        }

        [HideInInspector]
        [SerializeField]
        CameraClearFlags _clearFlags = CameraClearFlags.SolidColor;

        [HideInInspector]
        [SerializeField]
        Color _backgroundColor = Color.black;

        [HideInInspector]
        [SerializeField]
        LayerMask _cullingMask;

        [HideInInspector]
        [SerializeField]
        int _depth = 0;

        [HideInInspector]
        [SerializeField]
        int _screenHeight = 768;

        bool _isChanged;

        Camera _camera;

        readonly GuiTouchInfo[] _touches = new GuiTouchInfo[5];

        static GuiSystem _instance;

        void Awake () {
            var count = FindObjectsOfType <GuiSystem> ().Length;
            if (count > 1) {
                DestroyImmediate (gameObject);
                return;
            }
            _instance = this;
        }

        void OnDestroy () {
            if (_instance == this) {
                _instance = null;
            }
            _eventReceivers.Clear ();
        }

        void OnEnable () {
            if (_instance == null) {
                Awake ();
            }
            if (_instance != null) {
                FixCamera ();
            }
        }

        void FixCamera () {
            if (_camera == null) {
                _camera = GetComponent <Camera> ();
            }
            Camera.hideFlags = HideFlags.HideInInspector;
            Camera.orthographic = true;
            Camera.orthographicSize = ScreenHeight * 0.5f;
            Camera.nearClipPlane = -501f;
            Camera.farClipPlane = 501f;
            Camera.useOcclusionCulling = false;
            Camera.clearFlags = _clearFlags;
            Camera.backgroundColor = _backgroundColor;
            Camera.cullingMask = _cullingMask;
            Camera.depth = _depth;
        }

        void LateUpdate () {
            if (_isChanged) {
                _isChanged = false;
                FixCamera ();
            }

            if (!IsInputLocked && Application.isPlaying) {
                ProcessInput ();
            }
        }

        void ProcessInput () {
            var touchCount = Mathf.Min (_touches.Length, Input.touchCount);
            bool isMouse;
            if (touchCount == 0 && _touches[0].ProcessMouse (Camera, ScreenHeight)) {
                touchCount = 1;
                isMouse = true;
            } else {
                isMouse = false;
            }

            GuiTouchEventArg te;
            GuiEventReceiver newReceiver;
            Vector3 worldPos;

            _eventReceivers.Sort ((a, b) => a.GlobalDepthOrder.CompareTo (b.GlobalDepthOrder));

            for (var i = 0; i < touchCount; i++) {
                if (!isMouse) {
                    _touches[i].UpdateChanges (Input.GetTouch (i), Camera, ScreenHeight);
                } else {
                    isMouse = false;
                }

                if (_touches[i].IsStateChanged || _touches[i].IsDeltaChanged) {
                    worldPos = Camera.ScreenToWorldPoint (_touches[i].RawPosition);
                    newReceiver = null;
                    for (int j = _eventReceivers.Count - 1; j >= 0; j--) {
                        if (_eventReceivers[j].IsPointInside (worldPos.x, worldPos.y)) {
                            newReceiver = _eventReceivers[j];
                            break;
                        }
                    }

                    te = new GuiTouchEventArg (_touches[i].State, _touches[i].RawPosition, _touches[i].Delta);

                    if (_touches[i].IsDeltaChanged) {
                        if (_touches[i].Receiver != null) {
                            _touches[i].Receiver.RaiseDragEvent (te);
                        }
                    }

                    if (_touches[i].IsStateChanged) {
                        if (!_touches[i].State) {
                            if (_touches[i].Receiver != null) {
                                _touches[i].Receiver.RaisePressEvent (te);
                                if (!_touches[i].IsDeltaChanged && _touches[i].Receiver == newReceiver) {
                                    _touches[i].Receiver.RaiseClickEvent (te);
                                }
                            }
                            newReceiver = null;
                        } else {
                            _touches[i].Receiver = newReceiver;
                        }
                        if (_touches[i].Receiver != null) {
                            _touches[i].Receiver.RaisePressEvent (te);
                        }
                    }
                }
                _touches[i].ResetChanges ();
            }
        }

        /// <summary>
        /// Force revalidate GuiSystem.
        /// </summary>
        public void Validate () {
            if (!enabled) {
                enabled = true;
            }
            _isChanged = true;
        }

        /// <summary>
        /// Add event receiver.
        /// </summary>
        /// <param name="receiver">Receiver.</param>
        public void AddEventReceiver (GuiEventReceiver receiver) {
            if (receiver == null) {
                return;
            }
            if (!_eventReceivers.Contains (receiver)) {
                _eventReceivers.Add (receiver);
            }
        }

        /// <summary>
        /// Remove event receiver.
        /// </summary>
        /// <param name="receiver">Receiver.</param>
        public void RemoveEventReceiver (GuiEventReceiver receiver) {
            if (receiver == null) {
                return;
            }
            var indexOf = _eventReceivers.IndexOf (receiver);
            if (indexOf != -1) {
                _eventReceivers.RemoveAt (indexOf);
            }
        }

        readonly List<GuiEventReceiver> _eventReceivers = new List<GuiEventReceiver> (64);

        struct GuiTouchInfo {
            public int ID;

            public bool State;

            public Vector2 Position;

            public Vector2 RawPosition;

            public Vector2 Delta;

            public bool IsStateChanged;

            public bool IsDeltaChanged;

            public GuiEventReceiver Receiver;

            public static int DragOffsetSqr = 25 * 25;

            static Vector2 _mousePos;

            public void UpdateChanges (Touch info, Camera camera, int screenHeight) {
                var newState = !(info.phase == TouchPhase.Ended || info.phase == TouchPhase.Canceled);
                IsStateChanged = newState != State;
                State = newState;
                IsDeltaChanged = info.phase == TouchPhase.Moved;
                Delta = info.deltaPosition / camera.pixelHeight * screenHeight;
                RawPosition = info.position;
                Position = camera.ScreenToWorldPoint (RawPosition);
                if (info.phase == TouchPhase.Began) {
                    IsDeltaChanged = false;
                }
            }

            public void ResetChanges () {
                IsStateChanged = false;
                IsDeltaChanged = false;
            }

            public bool ProcessMouse (Camera camera, int screenHeight) {
                if (IsStateChanged || IsDeltaChanged) {
                    return true;
                }
                if (!Input.mousePresent) {
                    return false;
                }
                var newState = Input.GetMouseButton (0);
                IsStateChanged = newState != State;
                State = newState;
                if (State || IsStateChanged) {
                    var oldPos = RawPosition;
                    RawPosition = Input.mousePosition;
                    Delta = (RawPosition - oldPos) / camera.pixelHeight * screenHeight;
                    IsDeltaChanged = Delta.sqrMagnitude > 0.1f;

                    Position = camera.ScreenToWorldPoint (RawPosition);
                    if (State && IsStateChanged) {
                        IsDeltaChanged = false;
                    }
                }

                return IsStateChanged || IsDeltaChanged;
            }
        }
    }
}