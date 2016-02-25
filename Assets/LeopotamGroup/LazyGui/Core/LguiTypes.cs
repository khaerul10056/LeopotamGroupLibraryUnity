//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.Events;

namespace LeopotamGroup.LazyGui.Core {

    public enum SpriteType {
        Simple = 0,
        Sliced,
        TiledHorizontal,
        TiledVertical,
        TiledBoth
    }

    public enum Alignment {
        Left = 0,
        Right,
        Center
    }

    [Serializable]
    public class SpriteData {
        public string Name;

        public float CornerX;

        public float CornerY;

        public float CornerW;

        public float CornerH;

        public float BorderL;

        public float BorderT;

        public float BorderR;

        public float BorderB;

        public float CenterWidth { get { return CornerW - BorderL - BorderR; } }

        public float CenterHeight { get { return CornerH - BorderT - BorderB; } }
    }

    public enum PanelClipType {
        None,
        Range,
        Texture
    }

    [Flags]
    public enum ChangeType {
        None,
        Geometry = 1,
        Panel = 2,
        Depth = 4,
        Color = 8,
        All = Geometry | Panel | Depth | Color
    }

    public struct TouchEventArg {
        public bool State;

        public Vector2 Position;

        public Vector2 Delta;

        public TouchEventArg (bool state, Vector2 position, Vector2 delta) {
            State = state;
            Position = position;
            Delta = delta;
        }
    }

    public static class LguiConsts {
        public const string ShaderKeyWordClipRange = "LGUI_CLIP_RANGE";

        public const string ShaderParamClipData = "_ClipData";

        public const string ShaderParamClipTrans = "_ClipTrans";

        public const string MethodOnLguiPanelChanged = "OnLguiPanelChanged";

        public const string MethodOnLguiVisualSizeChanged = "OnLguiVisualSizeChanged";

        public static readonly int DefaultGuiLayer = LayerMask.NameToLayer ("UI");

        public static readonly LayerMask DefaultCameraMask = 1 << DefaultGuiLayer;

        public static bool IsTiled (this SpriteType spriteType) {
            return spriteType == SpriteType.TiledBoth || spriteType == SpriteType.TiledHorizontal || spriteType == SpriteType.TiledVertical;
        }
    }

    [Serializable]
    public sealed class OnTouchEventHandler : UnityEvent<LguiEventReceiver, TouchEventArg> {
    }
}