//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace LeopotamGroup.LazyGui.Core {
    public sealed class LguiAtlas : MonoBehaviour {
        public SpriteData[] Sprites = null;

        public Texture2D ColorTexture = null;

        public Texture2D AlphaTexture = null;

        readonly Dictionary<string, SpriteData> _spriteCache = new Dictionary<string, SpriteData> ();

        string[] _spriteNameCache;

        void OnEnable () {
            RefreshCache ();
        }

        public void RefreshCache () {
            _spriteCache.Clear ();
            _spriteNameCache = null;
            if (Sprites != null) {
                string name;
                _spriteNameCache = new string[Sprites.Length];
                for (int i = 0, iMax = Sprites.Length; i < iMax; i++) {
                    name = Sprites[i].Name;
                    _spriteCache[name] = Sprites[i];
                    _spriteNameCache[i] = name;
                }
            }
        }

        public SpriteData GetSpriteData (string spriteName) {
            if (_spriteCache.Count == 0) {
                RefreshCache ();
            }

            return !string.IsNullOrEmpty (spriteName) && _spriteCache.ContainsKey (spriteName) ? _spriteCache[spriteName] : null;
        }

        public string[] GetSpriteNames () {
            if (_spriteCache.Count == 0) {
                RefreshCache ();
            }
            return _spriteNameCache;
        }
    }
}