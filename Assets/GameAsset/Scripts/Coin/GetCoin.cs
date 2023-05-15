using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GetCoin : MonoBehaviour
{
    private bool isCheckTouch;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon") && !isCheckTouch)
        {
            isCheckTouch = true;
            for (int i = 0; i < GameController.Instance.list_Key.Count; i++)
            {
                if (GameController.Instance.list_Key[i].GetComponent<Image>().sprite != GameController.Instance.key)
                {
                    GameController.Instance.list_Key[i].GetComponent<Image>().sprite = GameController.Instance.key;
                    GameController.Instance.Key_Count++;
                    LeanPool.Despawn(gameObject);
                    break;
                }
            }
        }
    }
    
}