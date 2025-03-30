using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_VolumeSlider : MonoBehaviour
{
    public Slider slider;
    public string parametr;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float multiplier = 30f; // Default deðer

    private void Start()
    {
        // Slider deðiþtiðinde otomatik çaðrýlacak
        slider.onValueChanged.AddListener(SliderValue);
    }

    public void SliderValue(float _value)
    {
        // 0.001f altýndaki deðerler için minimum ses
        float volumeValue = Mathf.Max(_value, 0.001f);
        audioMixer.SetFloat(parametr, Mathf.Log10(volumeValue) * multiplier);
    }

    public void LoadSlider(float _value)
    {
        if (_value >= 0.001f)
        {
            slider.value = _value;
            // AudioMixer'a da uygula
            SliderValue(_value);
        }
        else
        {
            // Minimum ses seviyesi
            slider.value = 0.001f;
            SliderValue(0.001f);
        }
    }
}
