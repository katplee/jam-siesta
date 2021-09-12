﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CustomerSpawner : Singleton<CustomerSpawner>
{
    [SerializeField] private GameObject customer = null;
    private int spawnEvery = 0; //time between spawns
    private float spawnIn = 0f; //remaining time until next spawn

    private void Awake()
    {
        SetTimers();
    }

    private void Start()
    {
        //SpawnCustomer();
        InvokeRepeating("SpawnCustomer", 0f, spawnEvery);
    }

    private void SetTimers()
    {
        //spawnEvery = 3 * Random.Range(1, 4);
        spawnEvery = 20;
    }

    private void SpawnCustomer()
    {
        Transform queue = WaitingManager.Instance.transform;

        for (int i = 0; i < queue.childCount; i++)
        {
            Transform mNode = queue.GetChild(i);
            if (!mNode.GetComponentInChildren<Customer>())
            {
                GameObject _customer = Instantiate(customer, mNode);
                break;
            }
        }
    }
}
