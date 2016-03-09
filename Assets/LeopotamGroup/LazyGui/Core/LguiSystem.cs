//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System;
using UnityEngine;

namespace LeopotamGroup.LazyGui.Core {
    [ExecuteInEditMode]
    [RequireComponent (typeof (Camera))]
    public sealed class LguiSystem : MonoBehaviour {
        public int ScreenHeight {
            get { return _screenHeight; }
            set {
                if (value > 0 && value != _screenHeight) {
                    _screenHeight = value;
                    _isChanged = true;
                }
            }
        }

        public CameraClearFlags ClearFlags {
            get { return _clearFlags; }
            set {
                if (value != _clearFlags) {
                    _clearFlags = value;
                    _isChanged = true;
                }
            }
        }

        public Color BackgroundColor {
            get { return _backgroundColor; }
            set {
                if (value != _backgroundColor) {
                    _backgroundColor = value;
                    _isChanged = true;
                }
            }
        }

        public LayerMask CullingMask {
            get { return _cullingMask; }
            set {
                if (value != _cullingMask) {
                    _cullingMask = value;
                    _isChanged = true;
                }
            }
        }

        public int Depth {
            get { return _depth; }
            set {
                if (value != _depth) {
                    _depth = value;
                    _isChanged = true;
                }
            }
        }

        public Camera Camera {
            get {
                if (_camera == null) {
                    FixCamera ();
                }
                return _camera;
            }
        }

        public static LguiSystem Instance {
            get {
                if (_instance == null) {
                    _instance = FindObjectOfType <LguiSystem> ();
                    if (_instance == null) {
                        var go = new GameObject ("LguiSystem");
                        go.layer = LguiConsts.DefaultGuiLayer;
                        go.AddComponent <LguiSystem> ().CullingMask = LguiConsts.DefaultCameraMask;
                    }
                }
                return _instance;
            }
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

        readonly TouchInfo[] _touches = new TouchInfo[5];

        static LguiSystem _instance;

        void Awake () {
            var count = FindObjectsOfType <LguiSystem> ().Length;
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
            Camera.nearClipPlane = -256f;
            Camera.farClipPlane = 256f;
            Camera.useOcclusionCulling = false;
            Camera.clearFlags = _clearFlags;
            Camera.backgroundColor = _backgroundColor;
            Camera.cullingMask = _cullingMask;
            Camera.depth = _depth;
        }

        void Update () {
            if (_isChanged) {
                _isChanged = false;
                FixCamera ();
            }

            if (Application.isPlaying) {
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

            TouchEventArg te;
            RaycastHit hitInfo;
            LguiEventReceiver newReceiver;
            for (var i = 0; i < touchCount; i++) {
                if (!isMouse) {
                    _touches[i].UpdateChanges (Input.GetTouch (i), Camera, ScreenHeight);
                } else {
                    isMouse = false;
                }

                if (_touches[i].IsStateChanged || _touches[i].IsDeltaChanged) {
                    if (Physics.Raycast (Camera.ScreenPointToRay (_touches[i].RawPosition), out hitInfo, Camera.farClipPlane, 1 << gameObject.layer)) {
                        newReceiver = hitInfo.collider.gameObject.GetComponent<LguiEventReceiver> ();
                    } else {
                        newReceiver = null;
                    }

                    te = new TouchEventArg (_touches[i].State, _touches[i].RawPosition, _touches[i].Delta);

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

        public void Validate () {
            if (!enabled) {
                enabled = true;
            }
        }

        struct TouchInfo {
            public int ID;

            public bool State;

            public Vector2 Position;

            public Vector2 RawPosition;

            public Vector2 Delta;

            public bool IsStateChanged;

            public bool IsDeltaChanged;

            public LguiEventReceiver Receiver;

            //            public bool IsDrag;

            public static int DragOffsetSqr = 25 * 25;

//            Vector2 _startPos;

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
//                    _startPos = Position;
                    IsDeltaChanged = false;
//                    IsDrag = false;
                }
//                if (!State) {
//                    IsDrag = (Position - _startPos).sqrMagnitude > DragOffsetSqr;
//                }
            }

            public void ResetChanges () {
                IsStateChanged = false;
                IsDeltaChanged = false;
//                IsDrag = false;
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
//                        _startPos = Position;
                        IsDeltaChanged = false;
//                        IsDrag = false;
                    }
//                    if (!State) {
//                        IsDrag = (Position - _startPos).sqrMagnitude > DragOffsetSqr;
//                    }
                }

                return IsStateChanged || IsDeltaChanged;
            }
        }
    }
}