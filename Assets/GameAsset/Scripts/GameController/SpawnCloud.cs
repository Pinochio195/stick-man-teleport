using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnCloud : MonoBehaviour
{
    public List<GameObject> Cloud;
    public float timeDuration;
    private float timeDurationLeft;
    private float timeLoading;
    private int count;
    private void Start()
    {
        timeLoading = 0;
        count = 0;
    }

    void Update()
    {
        timeLoading += Time.unscaledDeltaTime;
        if (timeLoading > timeDurationLeft)
        {
            timeDurationLeft = timeDuration + Time.unscaledTime;
            count++;
            if (count > 6)
            {
                LeanPool.Spawn(Cloud[Random.Range(0, 4)], transform.position, Quaternion.identity);
                count = 0;
            }
        }
    }
}