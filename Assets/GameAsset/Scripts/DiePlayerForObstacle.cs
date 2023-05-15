using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiePlayerForObstacle : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ControllerShop.Instance.LoserGame();
        }
    }
}