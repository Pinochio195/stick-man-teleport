using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class CoinRotate : MonoBehaviour
{
    public float rotationSpeed = 100f; // Tốc độ xoay của object
    public int Index;

    

    void Update()
    {
        // Xoay object quanh trục Y theo tốc độ và thời gian đã cho
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            //GameController.Instance.list_Coin.Add(Index);
            ManagerCoin.Instance.listManager.Remove(this);
            GameController.Instance.list_Coin.Add(this.Index*1);
            //Debug.Log(GameController.Instance.list_Coin.Count);
        }
    }
}
