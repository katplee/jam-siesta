using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CustomerSpawner : Singleton<CustomerSpawner>
{
    [SerializeField] private GameObject customer = null;
    [SerializeField] private CustomerScriptable[] customerProfiles; //array containing customerscriptables
    private Queue<GameObject> customersToSpawn = new Queue<GameObject>();
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
        Customer _customer = null;
        Transform mNode = null;

        for (int i = 0; i < queue.childCount; i++)
        {
            mNode = queue.GetChild(i);
            if (!mNode.GetComponentInChildren<Customer>())
            {
                _customer = Instantiate(customer, mNode).GetComponent<Customer>();
                break;
            }
        }

        if (!_customer)
        {
            //if there are no available waiting nodes left, instantiate, set destination but temporarily hide
            _customer = Instantiate(customer, mNode).GetComponent<Customer>();
            Queue(_customer.gameObject);
        }

        //set customer profile
        int index = Random.Range(0, 8);
        _customer.CallToInitialize(customerProfiles[index]);

        //set customer destination
        CustomerController controller = _customer.GetComponent<CustomerController>();
        WaitingManager.Instance.SetQueueDestination(controller);
    }

    private void Queue(GameObject customer)
    {
        CustomerPatience patience = customer.GetComponent<CustomerPatience>();
        SpriteRenderer renderer = customer.GetComponent<SpriteRenderer>();
        
        Debug.Log(patience.GetInstanceID());

        customersToSpawn.Enqueue(customer);
        renderer.enabled = false;
        patience.SetPatienceInteractibility(false);
    }

    public CustomerController Dequeue()
    {
        if (customersToSpawn.Count != 0)
        {
            //calls the first customer in the to-spawn list
            GameObject customer = customersToSpawn.Dequeue();

            CustomerController controller = customer.GetComponent<CustomerController>();
            Debug.Log(controller.GetInstanceID());
            CustomerPatience patience = customer.GetComponent<CustomerPatience>();
            SpriteRenderer renderer = customer.GetComponent<SpriteRenderer>();

            renderer.enabled = true;
            patience.SetPatienceInteractibility(true);

            return controller;
        }

        return null;
    }

    public bool HasCustomersToSpawn()
    {
        bool spawn = (customersToSpawn.Count != 0) ? true : false;
        return spawn;
    }
}
