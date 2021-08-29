using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
[Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField]
    private List<TKey> keys = new List<TKey>();

    [SerializeField]
    private List<TValue> values = new List<TValue>();

    public void OnAfterDeserialize()
    {
        this.Clear();

        if(keys.Count != values.Count)
        {
            throw new Exception(string.Format("Keys: {0}; Values: {1}", keys.Count, values.Count));
        }

        for (int entry = 0; entry < keys.Count; entry++)
        {
            this.Add(keys[entry], values[entry]);
        }
    }

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach(KeyValuePair<TKey, TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }   
    }
}
*/

