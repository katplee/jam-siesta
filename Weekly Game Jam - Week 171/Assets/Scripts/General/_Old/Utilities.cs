using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class Utilities 
{
    public static T FindComponentInChildWithTag<T>(this GameObject parent, string tag)
        where T : Component
    {
        Transform t = parent.transform;
        foreach (Transform tr in t)
        {
            if (tr.tag == tag)
            {
                return tr.GetComponent<T>();
            }
        }
        return null;
    }

    public static bool HasChildWithComponent<T>(this GameObject parent, out GameObject child)
        where T : Component
    {
        Transform t = parent.transform;
        foreach (Transform tr in t)
        {
            if (tr.GetComponent<T>() != null)
            {
                child = tr.gameObject;
                return true;
            }
        }
        child = null;
        return false;
    }

    public static bool HasChildWithComponent<T>(this GameObject parent)
        where T : Component
    {
        HasChildWithComponent<T>(parent, out _);
        return true; //CODE WILL NEVER GET TO THIS LINE
    }
    

    public static bool _HasChildWithComponent<T>(this GameObject parent)
        where T : Component
    {
        Transform t = parent.transform;
        foreach (Transform tr in t)
        {
            if (tr.GetComponent<T>() != null)
            {                
                return true;
            }
        }
        return false;
    }

    public static bool HasSiblingWithAChildWithComponent<T>(this GameObject sibling, out GameObject child)
        where T : Component
    {
        Transform t = sibling.transform.parent.transform;
        foreach (Transform tr in t)
        {
            foreach (Transform ttr in tr)
            {
                if (ttr.GetComponent<T>() != null)
                {
                    child = ttr.gameObject;
                    return true;
                }
            }
        }
        child = null;
        return false;
    }
}
