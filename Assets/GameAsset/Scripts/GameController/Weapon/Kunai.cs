using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai : MonoBehaviour
{
    public float rotationForce; // Lực xoay quanh đối tượng

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            GetComponent<Rigidbody>().AddTorque(Vector3.forward * rotationForce);
        }
    }
}