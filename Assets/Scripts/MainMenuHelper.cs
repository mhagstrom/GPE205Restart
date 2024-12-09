using System;
using System.Collections;
using System.Collections.Generic;
using AAA;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuHelper : MonoBehaviour
{
    public TextMeshProUGUI BGVolume;
    public TextMeshProUGUI SFXVolume;
    public TextMeshProUGUI MapSize;
    public TextMeshProUGUI EnemyCount;
    
    public Slider BGVolumeSlider;
    public Slider SFXVolumeSlider;
    public Slider MapSizeSlider;
    public Slider EnemyCountSlider;

    public AudioMixer mixer;

    // Start is called before the first frame update
    void Start()
    {
        BGVolumeSlider.value = PlayerPrefs.GetFloat("BGVolume", 1f);
        SFXVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
        EnemyCountSlider.value = PlayerPrefs.GetFloat("EnemyCount", 0);
        MapSizeSlider.value = PlayerPrefs.GetFloat("MapSize", 0);

        BGVolumeSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        SFXVolumeSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        MapSizeSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        EnemyCountSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });


        ValueChangeCheck();
    }

    private void ValueChangeCheck()
    {
        GameManager.Instance.numTanks = (int)EnemyCountSlider.value;
        MapGenerator.Instance.SetMapSize(MapSizeSlider.value);

        EnemyCountSlider.maxValue = (MapGenerator.Instance.rows * MapGenerator.Instance.columns) - 1;
        BGVolume.text = BGVolumeSlider.value.ToString();
        SFXVolume.text = SFXVolumeSlider.value.ToString();
        MapSize.text = $"{MapGenerator.Instance.rows} x {MapGenerator.Instance.columns}";
        EnemyCount.text = EnemyCountSlider.value.ToString();

        PlayerPrefs.SetFloat("BGVolume", BGVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", SFXVolumeSlider.value);
        PlayerPrefs.SetFloat("EnemyCount", EnemyCountSlider.value);
        PlayerPrefs.SetFloat("MapSize", MapSizeSlider.value);

        SetVolume("SFXVolume", SFXVolumeSlider.value);
        SetVolume("BGMVolume", BGVolumeSlider.value);
    }

    private void SetVolume(string keyName, float value)
    {
        var logValue = Mathf.Clamp(Mathf.Log10(value) * 20.0f, -80, 0);
        mixer.SetFloat(keyName, logValue);
    }

}
