using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMoneyContainer : UIObject
{
    private Dictionary<ItemTransferrable, Sprite> numberDictionary = new Dictionary<ItemTransferrable, Sprite>();
    private List<UIMoney> money = new List<UIMoney> {
        new UIMoney(), //ones: 0
        new UIMoney(), //tens: 1
        new UIMoney(), //hundreds: 2
        new UIMoney(), //thousands: 3
        new UIMoney()  //ten thousands: 4
    };

    private void Awake()
    {
        PlayerPerformance.Instance.DeclareThis(Label, this);
    }

    private void Start()
    {
        UpdateEarnings(0f);
    }

    private void SetEarnings(UIMoney earnings, int place)
    {
        money[place] = earnings;
    }

    public void DeclareThis<T>(string element, T UIObject, int siblingIndex)
        where T : UIObject
    {
        switch (element)
        {
            case "UIMoney":
                SetEarnings(UIObject as UIMoney, siblingIndex);
                break;
        }
    }

    public void UpdateEarnings(float earnings)
    {
        string _earnings = earnings.ToString();
        Debug.Log(_earnings);


        for (int i = 0; i < _earnings.Length; i++)
        {
            Debug.Log(_earnings[_earnings.Length - (i + 1)]);
            money[i].ChangeText(_earnings[_earnings.Length - (i + 1)]);
        }
    }
}
