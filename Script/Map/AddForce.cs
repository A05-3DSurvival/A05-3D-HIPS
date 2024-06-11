using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForce : MonoBehaviour
{
    Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    
    void Start()
    {
        rigidbody.AddForce(Vector3.up * 15, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
