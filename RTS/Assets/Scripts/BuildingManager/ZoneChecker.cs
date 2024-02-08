using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneChecker : MonoBehaviour
{
    public string targetTag = "Foliage"; // Replace with your specific tag

    private void Start() {
        Invoke("MethodWithDelay", 1);
    }

    public void MethodWithDelay() {
        gameObject.GetComponent<SphereCollider>().radius = 5f;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag(targetTag))
        {
            Destroy(other.gameObject);
        }
    }
}
