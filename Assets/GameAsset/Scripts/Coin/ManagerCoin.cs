using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerCoin : MonoBehaviour
{
    public List<CoinRotate> listManager;
    public static ManagerCoin Instance { get; private set; }

    private void Start()
    {
        
    }

    private void Awake()
    {
        Instance = this;
    }
}
