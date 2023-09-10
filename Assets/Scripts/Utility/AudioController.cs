using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public class AudioController : MonoSingleton<AudioController>
    {
        public AudioAssetManager Assets;

        public float MusicVolume = 1f;
        private AudioSource _music;
        private int _currentDialogue = 0;
        private bool _playingDialogue = false;

        void Start()
        {
            StartCoroutine(PlayMusic());
        }

        IEnumerator PlayMusic()
        {
            _music = Play(Assets.Music[0], false);
            float duration = _music.clip.length;
            while (true)
            {
                yield return new WaitForSeconds(duration);
                _music = Play(Assets.Music.GetRandom(), false);
                duration = _music.clip.length;
            }
        }
        public void TriggerDialogue(int dialogue)
        {
            if (_currentDialogue <= dialogue && !_playingDialogue)
            {
                float duration = Play(Assets.Dialogue[dialogue], false).clip.length;
                StartCoroutine(DuckMusic(duration));
                _currentDialogue = dialogue+1; // Don't repeat
            }
        }

        IEnumerator DuckMusic(float duration)
        {
            _playingDialogue = true;
            MusicVolume = 0.4f;
            yield return new WaitForSeconds(duration);
            MusicVolume = 1f;
            _playingDialogue = false;
        }

        void Update()
        {
            _music.volume = Mathf.Lerp(_music.volume, MusicVolume, Time.deltaTime*1f);
        }

        public static AudioSource Play(AudioClip cl, bool loop)
        {
            GameObject audiothing = new GameObject();
            AudioSource source = audiothing.AddComponent<AudioSource>();
            source.clip = cl;
            source.loop = loop;
            source.Play();

            if (!loop)
                Object.Destroy(audiothing, cl.length);
            return source;

        }


    }

    public static class Utils
    {
        public static T GetRandom<T>(this List<T> list)
        {
            if (list == null || list.Count == 0)
            {
                Debug.LogError("Error: Don't pull random elements from an empty list!");
                return default(T);
            }

            return list[(int)(Random.value * list.Count)];
        }
        public static T GetRandom<T>(this T[] array)
        {
            if (array == null || array.Length == 0)
            {
                Debug.LogError("Error: Don't pull random elements from an empty list!");
                return default(T);
            }

            return array[(int)(Random.value * array.Length)];
        }

        public static float ThresholdLerp(float value, float target, float threshold, float t)
        {
            if (value == target)
                return target;
            value = Mathf.Lerp(value, target, t);
            return Mathf.Abs(value - target) < threshold ? target : value;
        }
    }

}