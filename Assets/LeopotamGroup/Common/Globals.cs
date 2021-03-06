﻿//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

namespace LeopotamGroup.Common {
    /// <summary>
    /// Storage for global properties, can be extended as partial class.
    /// </summary>
    public static partial class Globals {
        /// <summary>
        /// Is user interface locked. Can be used from any external code as global flag.
        /// </summary>
        public static bool IsUILocked = false;

        /// <summary>
        /// Is sound enabled. Can be used from any external code as global flag.
        /// </summary>
        public static bool IsSoundEnable = true;
    }
}

