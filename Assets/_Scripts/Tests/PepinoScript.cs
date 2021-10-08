using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PepinoScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"Character {transform.root.name} enter trigger of {other.name}");
        
        this.transform.parent = other.transform;
        this.transform.Translate(0, 2f, 0);

    }
}
