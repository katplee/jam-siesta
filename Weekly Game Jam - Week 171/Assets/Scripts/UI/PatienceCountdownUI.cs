using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatienceCountdownUI : MonoBehaviour
{
    [SerializeField]
    private bool isSet = false;
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private Gradient gradient;
    [SerializeField]
    private Image fillImage;
    [SerializeField]
    private CanvasRenderer fillCanvas;

    // Start is called before the first frame update
    void Start()
    {
        fillImage = transform.GetChild(0).GetComponent<Image>();
        fillCanvas = transform.GetChild(0).GetComponent<CanvasRenderer>();
        slider.fillRect = transform.GetChild(0).GetComponent<RectTransform>();
    }

    public void SetStartingPatience(float patienceLeft)
    {
        if (!isSet)
        {
            slider.maxValue = patienceLeft;
            slider.value = patienceLeft;
            fillImage.color = gradient.Evaluate(1f);
            fillCanvas.SetAlpha(1f);
            isSet = true;
        }
    }

    public void SetPatience(float patienceLeft)
    {
        slider.value = Mathf.Abs(patienceLeft);
        fillImage.color = (patienceLeft > 0) ? gradient.Evaluate(1f) : gradient.Evaluate(0f);        
    }
}
