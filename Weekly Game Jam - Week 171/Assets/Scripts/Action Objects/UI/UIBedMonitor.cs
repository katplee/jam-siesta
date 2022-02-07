using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBedMonitor : UIObject
{
    public static event Action<UIBedMonitor, int> OnBedSynchronization;
    public event Action OnBedEmpty;

    private UICleanBed bed = null;
    private UIWashPajamas pajamas = null;
    private UICustomerType ctype = null;
    private UICustomerImage cimage = null;
    private UICustomerTimeLeft ctimeleft = null;
    private UICustomerTimeText ctimetext = null;

    private Image image = null;
    [SerializeField] private List<Sprite> bgs = new List<Sprite>(); //0: normal, 1: green, 2: red

    private int index = -1;

    private void Awake()
    {
        index = transform.GetSiblingIndex();
        image = GetComponent<Image>();
    }

    private void Start()
    {
        OnBedSynchronization?.Invoke(this, index);
        ResetPanel();
    }

    private void ChangeBackground(int index)
    {
        image.sprite = bgs[index];
    }

    public void ResetPanel()
    {
        bed.ChangeOpacity(false);
        pajamas.ChangeOpacity(false);
        ctype.EmptyText();
        cimage.ResetImage();
        ctimeleft.ResetText();
    }

    public void ReceiveBool(string label, bool item)
    {
        switch (label)
        {
            case "clean_bed":
                //signal the player to clean the sheets; true = bed is dirty
                bed.ChangeOpacity(item);
                break;

            case "wash_pajamas":
                //signal the player to clean the sheets; true = bed is dirty
                pajamas.ChangeOpacity(item);
                break;            

            default:
                break;
        }
    }

    public void ReceiveString(string label, string item)
    {
        switch (label)
        {
            case "customer_type":
                ctype.ChangeText(item);
                break;

            case "customer_timeleft":
                ctimeleft.ChangeText(item);
                break;

            default:
                break;
        }
    }

    public void ReceiveSprite(string label, Sprite item)
    {
        switch (label)
        {
            case "customer_image":
                cimage.ChangeImage(item);
                break;

            default:
                break;
        }
    }

    public void ReceiveInt(string label, int item)
    {
        switch (label)
        {
            case "wake_up_customer":
                ChangeBackground(item);
                break;

            default:
                break;
        }
    }

    

    public void DeclareThis<T>(string element, T UIObject)
        where T : UIObject
    {
        switch (element)
        {
            case "UICleanBed":
                SetBed(UIObject as UICleanBed);
                break;

            case "UIWashPajamas":
                SetPajamas(UIObject as UIWashPajamas);
                break;

            case "UICustomerType":
                SetCustomerType(UIObject as UICustomerType);
                break;

            case "UICustomerImage":
                SetCustomerImage(UIObject as UICustomerImage);
                break;

            case "UICustomerTimeLeft":
                SetCustomerTimeLeft(UIObject as UICustomerTimeLeft);
                break;

            case "UICustomerTimeText":
                SetCustomerTimeText(UIObject as UICustomerTimeText);
                break;
        }
    }

    private void SetBed(UICleanBed bed)
    {
        this.bed = bed;
    }

    private void SetPajamas(UIWashPajamas pajamas)
    {
        this.pajamas = pajamas;
    }

    private void SetCustomerType(UICustomerType ctype)
    {
        this.ctype = ctype;
    }

    private void SetCustomerImage(UICustomerImage cimage)
    {
        this.cimage = cimage;
    }

    private void SetCustomerTimeLeft(UICustomerTimeLeft ctimeleft)
    {
        this.ctimeleft = ctimeleft;
    }

    private void SetCustomerTimeText(UICustomerTimeText ctimetext)
    {
        this.ctimetext = ctimetext;
    }
}
