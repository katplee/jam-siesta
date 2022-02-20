using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMoneyContainer : UIObject
{
    [SerializeField] List<Sprite> numberDictionary = new List<Sprite>();
    private List<UIMoney> money = new List<UIMoney> {
        new UIMoney(), //ones: 0
        new UIMoney(), //tens: 1
        new UIMoney(), //hundreds: 2
        new UIMoney(), //thousands: 3
        new UIMoney(), //ten thousands: 4
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
            if (i + 1 > _earnings.Length) { money[i].ChangeSprite(numberDictionary[10]); continue; }

            int i_text = int.Parse(_earnings[_earnings.Length - (i + 1)].ToString());
            money[i].ChangeSprite(numberDictionary[i_text]);
        }

        //hide the comma if the earnings is less than 1,000
        if (_earnings.Length > 3) { money[5].ChangeSprite(numberDictionary[11]); }
        else { money[5].ChangeSprite(numberDictionary[10]); }
    }
}
