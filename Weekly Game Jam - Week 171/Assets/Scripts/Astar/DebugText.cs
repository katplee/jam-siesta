using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugText : MonoBehaviour
{
    [SerializeField]
    private RectTransform arrow;
    [SerializeField]
    private TextMeshProUGUI p;
    [SerializeField]
    private TextMeshProUGUI f;
    [SerializeField]
    private TextMeshProUGUI g;
    [SerializeField]
    private TextMeshProUGUI h;

    public RectTransform MyArrow { get => arrow; set => arrow = value; }
    public TextMeshProUGUI P { get => p; set => p = value; }
    public TextMeshProUGUI F { get => f; set => f = value; }
    public TextMeshProUGUI G { get => g; set => g = value; }
    public TextMeshProUGUI H { get => h; set => h = value; }
}
