using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class BotController : MonoBehaviour
{
    public int life;
    public float speed = 15f;
    public float raycastDistance = 1.2f;
    public LayerMask groundLayer;
    public Transform GO_CheckGround;
    public bool isGrounded;
    public bool isTurning;
    private Animator animator;
    public float timeBetweenShots;
    public float timeSinceLastShot;

    private void Start()
    {
        life = 1;
        animator = GetComponent<Animator>();
        timeBetweenShots = 5f;
    }

    private void Update()
    {
        // Kiểm tra va chạm với ground
        RaycastHit groundHit;
        isGrounded = Physics.Raycast(GO_CheckGround.position, Vector3.down, out groundHit, raycastDistance,
            groundLayer);
        if (isGrounded)
        {
            if (isTurning)
            {
                isTurning = false;
            }

            // Di chuyển thẳng
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            if (!animator.GetBool("isWalking"))
            {
                animator.SetBool("isWalking", true);
            }

        }
        else
        {
            if (animator.GetBool("isWalking"))
            {
                animator.SetBool("isWalking", false);
            }

            if (!isTurning)
            {
                // Tính toán thời gian đã trôi qua kể từ lần bắn tên lửa trước đó
                timeSinceLastShot += Time.deltaTime;
                // Nếu đã đủ thời gian giữa các lần bắn tên lửa
                if (timeSinceLastShot >= timeBetweenShots)
                {
                    // Đặt thời gian đã trôi qua về 0 để bắn tên lửa lần tiếp theo
                    transform.Rotate(0f, 180f, 0f);
                    timeSinceLastShot = 0f;
                    timeBetweenShots = Random.Range(1f, 3f);
                    isTurning = true;
                }
            }
        }
    }
}