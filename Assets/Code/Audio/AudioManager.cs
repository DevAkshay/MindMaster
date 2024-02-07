using System.Collections;
using System.Collections.Generic;
using Code.Utils;
using Code.Utils.Pooling;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Audio
{
    public class AudioManager : GenericSingleton<AudioManager>
    {
        [SerializeField] private AudioDataSO audioData;
        [SerializeField] private AudioSource musicSourcePrefab;
        [SerializeField] private GameObject sfxSourcePrefab; 

        private AudioSource _musicSource;
        private Transform _audioContainer;

        private float _musicVolume = 1f;
        private float _sfxVolume = 1f;

        private IObjectPoolManager _objectPoolManager;

        protected override void Awake()
        {
            Initialize();
            LoadVolumeSettings();
        }

        private void Initialize()
        {
            _musicSource = Instantiate(musicSourcePrefab, transform);
            _objectPoolManager = ObjectPoolManager.Instance;
            _objectPoolManager.CreatePool("SFX", sfxSourcePrefab, 2);
        }

        public void PlayMusic(string audioId)
        {
            var clip = audioData.GetAudioClipById(audioId);
            if (clip == null)
            {
                Debug.LogError($"Audio clip is null for id : {audioId}");
                return;
            }

            _musicSource.clip = clip;
            _musicSource.loop = true;
            _musicSource.volume = _musicVolume;
            _musicSource.Play();
        }

        public void PlaySfx(string audioId)
        {
            var clip = audioData.GetAudioClipById(audioId);
            if (clip == null)
            {
                Debug.LogError($"Audio clip is null for id : {audioId}");
                return;
            }
            Debug.LogError("PlaySfx 2 "+clip.name);

            var sfxObject = _objectPoolManager.GetObjectFromPool("SFX", transform.position, Quaternion.identity);
            sfxObject.transform.SetParent(transform);
            var sfxSource = sfxObject.GetComponent<AudioSource>();
            sfxSource.clip = clip;
            sfxSource.volume = _sfxVolume;
            sfxSource.Play();

            StartCoroutine(ReleaseSfxObject(sfxObject, clip.length));
        }
        
        private IEnumerator ReleaseSfxObject(GameObject sfxObject, float delay)
        {
            yield return new WaitForSeconds(delay);

            if (sfxObject != null)
            {
                sfxObject.GetComponent<AudioSource>().Stop();
                _objectPoolManager.ReleaseObjectToPool("SFX", sfxObject);
            }
        }

        public void SetMusicVolume(float volume)
        {
            _musicVolume = Mathf.Clamp01(volume);
            _musicSource.volume = _musicVolume;
            SaveVolumeSettings();
        }

        public void SetSfxVolume(float volume)
        {
            _sfxVolume = Mathf.Clamp01(volume);
            SaveVolumeSettings();
        }
        
        private void SaveVolumeSettings()
        {
            PlayerPrefs.SetFloat("MusicVolume", _musicVolume);
            PlayerPrefs.SetFloat("SfxVolume", _sfxVolume);
            PlayerPrefs.Save();
        }

        private void LoadVolumeSettings()
        {
            _musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
            _sfxVolume = PlayerPrefs.GetFloat("SfxVolume", 1f);
            SetMusicVolume(_musicVolume);
            SetSfxVolume(_sfxVolume);
        }

        public void StopMusic()
        {
            _musicSource.Stop();
        }
    }
}