//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace LeopotamGroup.Gui.Common {
    /// <summary>
    /// Gui atlas - holder of packed sprites.
    /// </summary>
    public sealed class GuiAtlas : MonoBehaviour {
        /// <summary>
        /// Packed sprites data.
        /// </summary>
        public GuiSpriteData[] Sprites = null;

        /// <summary>
        /// Color texture reference.
        /// </summary>
        public Texture2D ColorTexture = null;

        /// <summary>
        /// Alpha texture. Can be null.
        /// </summary>
        public Texture2D AlphaTexture = null;

        readonly Dictionary<string, GuiSpriteData> _spriteCache = new Dictionary<string, GuiSpriteData> ();

        string[] _spriteNameCache;

        void OnEnable () {
            ResetCache ();
        }

        /// <summary>
        /// Revalidate sprites cache.
        /// </summary>
        public void ResetCache () {
            _spriteCache.Clear ();
            _spriteNameCache = null;
            if (Sprites != null) {
                string sprName;
                _spriteNameCache = new string[Sprites.Length];
                for (int i = 0, iMax = Sprites.Length; i < iMax; i++) {
                    sprName = Sprites[i].Name;
                    _spriteCache[sprName] = Sprites[i];
                    _spriteNameCache[i] = sprName;
                }
            }
        }

        /// <summary>
        /// Get sprite data by sprite name.
        /// </summary>
        /// <param name="spriteName">Sprite name.</param>
        public GuiSpriteData GetSpriteData (string spriteName) {
            if (_spriteCache.Count == 0) {
                ResetCache ();
            }
            return !string.IsNullOrEmpty (spriteName) && _spriteCache.ContainsKey (spriteName) ? _spriteCache[spriteName] : null;
        }

        /// <summary>
        /// Get all sprite names from atlas.
        /// </summary>
        /// <returns>The sprite names.</returns>
        public string[] GetSpriteNames () {
            if (_spriteCache.Count == 0) {
                ResetCache ();
            }
            return _spriteNameCache;
        }
    }
}