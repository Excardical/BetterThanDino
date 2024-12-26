using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FloatingStatusBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text statusText;
    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
        
        if (statusText != null)
        {
            statusText.text = $"{currentValue} / {maxValue}";
        }
    }

    void Update()
    {
    }
}
