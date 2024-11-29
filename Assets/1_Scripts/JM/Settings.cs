using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public GameObject _settingsPanelPrefab; // 프리팹 연결
    GameObject _settingsPanelInstance;
    public Slider _BGMSlider;
    public Slider _effectSlider;

    public Button _closeButton;

    public SoundManager _soundManager;

    void Start()
    {
        Canvas canvas = GameObject.Find("JM_UI").GetComponent<Canvas>(); // 씬에서 Canvas를 찾음
        
        if (canvas != null && _settingsPanelPrefab != null)
        {
            _settingsPanelInstance = Instantiate(_settingsPanelPrefab, canvas.transform);
            _settingsPanelInstance.SetActive(false);

            _closeButton = _settingsPanelInstance.GetComponentInChildren<Button>();
            _closeButton?.onClick.AddListener(CloseSettings);

            _BGMSlider = _settingsPanelInstance.transform.Find("BGMSlider").GetComponent<Slider>();
            _effectSlider = _settingsPanelInstance.transform.Find("EffectSlider").GetComponent<Slider>();

            if (_BGMSlider != null)
            {
                _BGMSlider.onValueChanged.AddListener(delegate { _soundManager.OnBGMVolumeChanged(); });
            }
            if (_effectSlider != null)
            {
                _effectSlider.onValueChanged.AddListener(delegate { _soundManager.OnEffectVolumeChanged(); });
            }
        }
        else
        {
            Debug.LogWarning("Settings Panel Prefab is not set.");
        }
    }

    public void OpenSettings()
    {
        if (_settingsPanelInstance != null)
        {
            _settingsPanelInstance.SetActive(true);
        }
    }

    public void CloseSettings()
    {
        if (_settingsPanelInstance != null)
        {
            _settingsPanelInstance.SetActive(false);
        }
    }
}
