using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CustomerManager : Singleton<CustomerManager>
{
    public GameObject customerPrefab;

    public float timeUntilSpawn = 0f;
    public float spawnTime;

    // Start is called before the first frame update
    void Start()
    {        
        spawnTime = 3 * Random.Range(1f, 4f);
    }

    // Update is called once per frame
    void Update()
    {
        if (timeUntilSpawn == 0)
        {
            Spawn();
        }

        CountdownSpawnTimer();                
    }    

    private void CountdownSpawnTimer()
    {
        timeUntilSpawn += Time.deltaTime;

        if(Mathf.Clamp(timeUntilSpawn, 0f, spawnTime) >= spawnTime)
        {            
            timeUntilSpawn = 0f;
            spawnTime = 3 * Random.Range(1f, 4f);            
        }
    }
    
    private void Spawn()
    {
        for (int i = 0; i < WaitingCustomerManager.Instance.transform.childCount; i++)
        {
            if (!Utilities._HasChildWithComponent<Customer>(WaitingCustomerManager.Instance.transform.GetChild(i).gameObject))
            {
                GameObject go = Instantiate(customerPrefab, WaitingCustomerManager.Instance.transform.GetChild(i).transform, false);
                go.name = customerPrefab.name;

                return;
            }
        }
    }
}
