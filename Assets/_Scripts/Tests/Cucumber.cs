using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cucumber : MonoBehaviour
{
    private InputManager inputManager;

    [SerializeField]
    private bool _haveCucumber;

    private BoxCollider boxCollider;
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    
    void Update()
    {
        if (_haveCucumber)
        {
            boxCollider.enabled = false;
        }
        else
        {
            boxCollider.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
