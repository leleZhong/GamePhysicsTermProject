using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public List<AudioSource> _BGMList; // BGM 오디오 소스 목록
    public List<AudioClip> _effectClips; // 효과음 오디오 클립 목록
    private AudioSource _effectSource; // 효과음을 재생하는 오디오 소스

    public Settings _settings;

    void Awake()
    {
        transform.SetParent(null);
        // DontDestroyOnLoad(gameObject); // SoundManager 객체를 파괴하지 않도록 설정

        // 효과음용 AudioSource 생성
        _effectSource = gameObject.AddComponent<AudioSource>();
        _effectSource.loop = false;
    }

    void Start()
    {
        _settings = FindObjectOfType<Settings>(); // 씬에서 Settings를 찾음
        Canvas canvas = FindObjectOfType<Canvas>(); // 씬에서 Canvas를 찾음

        if (_settings != null)
        {
            _settings._soundManager = this;

            InitializeSlider();
            LoadSettings();
        }
        else
        {
            Debug.LogWarning("Settings is not set.");
        }
    }

    void InitializeSlider()
    {
        if (_settings != null)
        {
            if (_settings._BGMSlider != null)
            {
                _settings._BGMSlider.value = PlayerPrefs.GetFloat("BGMVolume", 1f);
            }
            if (_settings._effectSlider != null)
            {
                _settings._effectSlider.value = PlayerPrefs.GetFloat("EffectVolume", 1f);
            }
        }
    }

    public void OnBGMVolumeChanged()
    {
        foreach (AudioSource audioSource in _BGMList)
        {
            audioSource.volume = _settings._BGMSlider.value;
        }

        Debug.Log("BGM Volume Changed: " + _settings._BGMSlider.value);

        SaveSettings();
    }

    public void OnEffectVolumeChanged()
    {
        _effectSource.volume = _settings._effectSlider.value;
        SaveSettings();
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("BGMVolume", _settings._BGMSlider.value);
        PlayerPrefs.SetFloat("EffectVolume", _settings._effectSlider.value);
        PlayerPrefs.Save();
    }

    public void LoadSettings()
    {
        if (PlayerPrefs.HasKey("BGMVolume"))
        {
            _settings._BGMSlider.value = PlayerPrefs.GetFloat("BGMVolume");
        }
        if (PlayerPrefs.HasKey("EffectVolume"))
        {
            _settings._effectSlider.value = PlayerPrefs.GetFloat("EffectVolume");
        }
    }

    // 캐릭터 효과음을 재생하기 위한 함수 추가
    public void PlayEffectSound(string clipName)
    {
        AudioClip clip = _effectClips.Find(c => c.name == clipName);
        if (clip != null)
        {
            _effectSource.PlayOneShot(clip, _settings._effectSlider.value);
        }
        else
        {
            Debug.LogWarning("Effect clip not found: " + clipName);
        }
    }
}
