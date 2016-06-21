//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.Events;

namespace LeopotamGroup.Gui.Common {
    /// <summary>
    /// Sprite rendering type.
    /// </summary>
    public enum GuiSpriteType {
        Simple = 0,
        Sliced,
        TiledHorizontal,
        TiledVertical,
        TiledBoth
    }

    /// <summary>
    /// Label rendering effect.
    /// </summary>
    public enum GuiFontEffect {
        None = 0,
        Shadow,
        Outline
    }

    /// <summary>
    /// Alignment of widget.
    /// </summary>
    public enum GuiAlignment {
        TopLeft = 0,
        TopCenter,
        TopRight,
        CenterLeft,
        Center,
        CenterRight,
        BottomLeft,
        BottomCenter,
        BottomRight
    }

    /// <summary>
    /// Sprite data for keeping inside GuiAtlas.
    /// </summary>
    [Serializable]
    public sealed class GuiSpriteData {
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

    /// <summary>
    /// Panel clipping type.
    /// </summary>
    public enum GuiPanelClipType {
        None,
        Range,
        Texture
    }

    /// <summary>
    /// Widget dirty type.
    /// </summary>
    [Flags]
    public enum GuiDirtyType {
        None,
        Geometry = 1,
        Panel = 2,
        Depth = 4,
        All = Geometry | Panel | Depth
    }

    /// <summary>
    /// Data of touch event.
    /// </summary>
    public struct GuiTouchEventArg {
        public bool State;

        public Vector2 Position;

        public Vector2 Delta;

        public GuiTouchEventArg (bool state, Vector2 position, Vector2 delta) {
            State = state;
            Position = position;
            Delta = delta;
        }
    }

    /// <summary>
    /// Internal consts.
    /// </summary>
    public static class GuiConsts {
        public const string ShaderKeyWordClipRange = "GUI_CLIP_RANGE";

        public const string ShaderParamClipData = "_ClipData";

        public const string ShaderParamClipTrans = "_ClipTrans";

        public static readonly int DefaultGuiLayer = LayerMask.NameToLayer ("UI");

        public static readonly LayerMask DefaultGuiLayerMask = 1 << DefaultGuiLayer;

        public static bool IsTiled (this GuiSpriteType spriteType) {
            return spriteType == GuiSpriteType.TiledBoth || spriteType == GuiSpriteType.TiledHorizontal || spriteType == GuiSpriteType.TiledVertical;
        }
    }

    /// <summary>
    /// Touch event handler.
    /// </summary>
    [Serializable]
    public sealed class OnGuiTouchEventHandler : UnityEvent<GuiEventReceiver, GuiTouchEventArg> {
    }
}