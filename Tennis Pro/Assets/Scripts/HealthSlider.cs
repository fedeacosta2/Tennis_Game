using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthSlider : MonoBehaviour
{
    [SerializeField] private Line line;
    public Slider slider;
    public float sliderValue = 1f;
    private float _currentSliderValue = 1f;
    // Add a public variable to control the slider value in Unity
    //public float externalSliderValue = 0.0f;

    private void Update()
    {
        if (_currentSliderValue != sliderValue)
        {
            slider.value = sliderValue;
            _currentSliderValue = sliderValue;
        }
    }

    private void updateslidermultiplier1()
    {
        sliderValue+= 0.1f;
    }



}
