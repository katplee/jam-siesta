using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownUI : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private Gradient gradient;
    [SerializeField]
    private bool isSet = false;
    [SerializeField]
    private CanvasRenderer fillCanvas;
    [SerializeField]
    private Image fillImage;

    private void Start()
    {
        fillCanvas = transform.GetChild(0).GetComponent<CanvasRenderer>();
        fillImage = transform.GetChild(0).GetComponent<Image>();
        gameObject.GetComponent<Slider>().fillRect = transform.GetChild(0).GetComponent<RectTransform>();
    }

    public void SetStartingAlarm(float neededSleep)
    {
        if (!isSet)
        {
            slider.maxValue = neededSleep;
            slider.value = neededSleep;
            fillImage.color = gradient.Evaluate(1f);
            isSet = true;
        }        
    }

    public void SetAlarm(float timeRemaining)
    {
        slider.value = timeRemaining;
        fillImage.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void ResetAlarm()
    {
        isSet = false;
    }

    public void HideBar()
    {
        fillCanvas.SetAlpha(0f);
    }

    public void ShowBar()
    {
        fillCanvas.SetAlpha(1f);
    }
}
