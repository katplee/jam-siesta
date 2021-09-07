using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPatience : MonoBehaviour, IUserInterface
{
    private CustomerPatience patience = null;
    private Image image = null;

    private Color32 bonus = new Color32(56, 152, 73, 255);
    private Color32 normal = new Color32(241, 150, 20, 255);

    public string label
    {
        get { return GetType().Name; }
    }

    private void Awake()
    {
        image = GetComponent<Image>();
        patience = GetComponentInParent<CustomerPatience>();

        if (patience != null)
        {
            patience.DeclareThis(label, this);
        }
    }

    public void ChangeFillAmount(float patienceLeft, float patienceTotal)
    {
        image.fillAmount = patienceLeft / patienceTotal;
    }

    public void ChangeFillColor(bool plus)
    {
        image.color = plus ? bonus : normal;
    }
}
