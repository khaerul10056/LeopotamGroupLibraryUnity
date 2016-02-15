//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using LeopotamGroup.Common;
using UnityEngine;

namespace LeopotamGroup.FX {
    public enum SoundFXChannel {
        First = 0,
        Second = 1,
        Third = 2
    }

    sealed class SoundManager : UnitySingleton<SoundManager> {
        public float SoundVolume {
            get { return _fxes[0].volume; }
            set {
                foreach (var item in _fxes) {
                    item.volume = value;
                }
            }
        }

        public float MusicVolume {
            get { return _music.volume; }
            set { _music.volume = value; }
        }

        public string LastPlayedMusic { get; private set; }

        bool _isLastPlayedMusicLooped;

        AudioSource _music;

        AudioSource[] _fxes;

        protected override void OnConstruct () {
            DontDestroyOnLoad (gameObject);

            _music = gameObject.AddComponent<AudioSource> ();
            _fxes = new []
            {
                gameObject.AddComponent<AudioSource> (),
                gameObject.AddComponent<AudioSource> (),
                gameObject.AddComponent<AudioSource> ()
            };

            _music.loop = false;
            _music.playOnAwake = false;
            foreach (var item in _fxes) {
                item.loop = false;
                item.playOnAwake = false;   
            }
        }

        public void PlayMusic (string music, bool isLooped = false) {
            if (LastPlayedMusic == music && _music.isPlaying) {
                return;
            }

            StopMusic ();

            LastPlayedMusic = music;
            _isLastPlayedMusicLooped = isLooped;

            if (MusicVolume > 0f && !string.IsNullOrEmpty (LastPlayedMusic)) {
                _music.clip = Resources.Load<AudioClip> (LastPlayedMusic);
                _music.loop = isLooped;
                _music.Play ();
            }
        }

        public void PlayFX (AudioClip clip, SoundFXChannel channel = SoundFXChannel.First, bool forceInterrupt = false) {
            var fx = _fxes[(int) channel];
            if (!forceInterrupt && fx.isPlaying) {
                return;
            }

            StopFX (channel);

            fx.clip = clip;

            if (SoundVolume > 0f && clip != null) {
                fx.Play ();
            }
        }

        public void StopFX (SoundFXChannel channel) {
            var fx = _fxes[(int) channel];
            if (fx.isPlaying) {
                fx.Stop ();
            }
            fx.clip = null;
        }

        public void StopMusic () {
            _music.Stop ();
        }

        public void ValidateMusic () {
            if (MusicVolume > 0f && !string.IsNullOrEmpty (LastPlayedMusic)) {
                if (!_music.isPlaying) {
                    PlayMusic (LastPlayedMusic, _isLastPlayedMusicLooped);
                }
            } else {
                StopMusic ();
            }
        }
    }
}