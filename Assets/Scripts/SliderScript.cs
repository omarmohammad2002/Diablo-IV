using UnityEngine;
using UnityEngine.UI;

public class GradientSlider : MonoBehaviour
{
    public Slider slider;
    public Color colorStart = Color.red;
    public Color colorEnd = Color.green;

    void Update()
    {
        // Update the Fill's color based on the slider's value
        float t = slider.value / slider.maxValue;
        slider.fillRect.GetComponent<Image>().color = Color.Lerp(colorStart, colorEnd, t);
    }
}