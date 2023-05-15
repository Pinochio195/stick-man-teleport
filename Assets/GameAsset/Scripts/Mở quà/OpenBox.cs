using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class OpenBox : MonoBehaviour
{
    public int randomGold;
    private int randomX;
    private void Start()
    {
        randomX = Random.Range(0,9);
        randomGold = randomX * 100+100;
    }
    
    
}
