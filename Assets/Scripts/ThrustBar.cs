using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrustBar : MonoBehaviour
{
    private Slider slider;
    public Image fillImage; // Reference to the slider's fill image

    private void Awake()
    {
        slider = GameObject.Find("ThrustBar").GetComponent<Slider>();
        fillImage = slider.fillRect.GetComponent<Image>(); // Ensure the fill image is linked correctly
    }

    public void UpdateMeter(float newValue)
    {
        slider.value = newValue;
        UpdateColor(newValue);
    }

    private void UpdateColor(float value)
    {
        // Assuming the slider's minimum value is 0 and maximum value is 1
        if (value < 0.5f)
        {
            // Interpolate between red and green when value is less than 0.5
            fillImage.color = Color.Lerp(Color.red, Color.green, value * 2);
        }
        else
        {
            // Interpolate between green and blue when value is between 0.5 and 1
            fillImage.color = Color.Lerp(Color.green, Color.blue, (value - 0.5f) * 2);
        }
    }
}
