using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBox : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            GameController.Instance.Player_Position.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}