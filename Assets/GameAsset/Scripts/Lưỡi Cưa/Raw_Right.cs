using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raw_Right : MonoBehaviour
{
    public float moveDistance = 2f; // khoảng cách di chuyển
    public float moveSpeed = 2f; // tốc độ di chuyển
    public float delayTime; // thời gian dừng lại (s)
    private Vector3 startPosition;
    private bool movingForward = false;
    private float timeLeft;

    void Start()
    {
        startPosition = transform.position;
        timeLeft = delayTime;
    }

    void Update()
    {
        float delta = moveSpeed * Time.deltaTime;
        if (movingForward)
        {
            if (transform.position.x >= startPosition.x)
            {
                if (timeLeft <= 0f)
                {
                    movingForward = false;
                    timeLeft = delayTime;
                }
                else
                {
                    timeLeft -= Time.deltaTime;
                }
            }
            else
            {
                transform.position += new Vector3(delta, 0, 0);
            }
        }
        else
        {
            transform.position -= new Vector3(delta, 0, 0);
            if (Mathf.Abs(transform.position.x - startPosition.x) >= moveDistance)
            {
                movingForward = true;
            }
        }
    }

}
