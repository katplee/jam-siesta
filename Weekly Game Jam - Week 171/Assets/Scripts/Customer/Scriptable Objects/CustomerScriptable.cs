using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Customer")]
public class CustomerScriptable : ScriptableObject
{
    public CustomerType customerType;
    public string customerSubType;
    public int sleepNeeded; //the factor to which the customerType will be multiplied to, default = 3
    public int sleepAllowance; //the factor to which the customerType will be multiplied to, default = 1
    public int patience;

    public AnimatorOverrideController controller;
    public Sprite sleeping;
    public Sprite standard;
}
