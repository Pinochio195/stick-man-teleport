using UnityEngine;
using System.Collections;

public class Raw : MonoBehaviour
{
    public float moveDistance = 2f; // khoảng cách di chuyển
    public float moveSpeed = 2f; // tốc độ di chuyển
    public float delayTime; // thời gian dừng lại (s)
    private Vector3 startPosition;
    private bool movingForward = true;
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
            transform.position += new Vector3(delta, 0, 0);
            if (transform.position.x - startPosition.x >= moveDistance)
            {
                movingForward = false;
            }
        }
        else
        {
            if (transform.position.x <= startPosition.x)
            {
                if (timeLeft <= 0f)
                {
                    movingForward = true;
                    timeLeft = delayTime;
                }
                else
                {
                    timeLeft -= Time.deltaTime;
                }
            }
            else
            {
                transform.position -= new Vector3(delta, 0, 0);
            }
        }
    }
}