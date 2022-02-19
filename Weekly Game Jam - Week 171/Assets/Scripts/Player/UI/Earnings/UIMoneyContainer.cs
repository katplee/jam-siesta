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
        new UIMoney(),  //ten thousands: 4
        new UIMoney() //comma: 5
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
        string _earnings = (earnings == 0f)? "0" : earnings.ToString();

        //the last element of the money list is the comma, thus the -1
        for (int i = 0; i < money.Count - 1; i++)
        {
            //hide the comma if the earnings is less than 1,000
            if (_earnings.Length > 3) { money[5].gameObject.SetActive(true); break; }
            else if (i + 1 > _earnings.Length) { money[i + 1].gameObject.SetActive(false); break; }

            char i_text = _earnings[_earnings.Length - (i + 1)];
            money[i].ChangeText(i_text);
            money[i].gameObject.SetActive(true);
        }
    }
}
