using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyEvent : MonoBehaviour
{
    public static DontDestroyEvent Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
