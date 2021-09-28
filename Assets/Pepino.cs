using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pepino : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Character {transform.root.name} enter trigger of {other.name}");
    }
}
