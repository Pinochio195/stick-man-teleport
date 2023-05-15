using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class Enemy_FireController : MonoBehaviour
{
    public int life;
    public GameObject rocketPrefab; // Đối tượng tên lửa để bắn ra
    public Transform firePoint; // Vị trí để bắn đối tượng tên lửa ra
    public float fireRate; // Tốc độ bắn (bắn một lần mỗi giây)
    private Rigidbody rocketRigidbody; // Component Rigidbody của đối tượng tên lửa
    private float timeSinceLastShot = 0f; // Thời gian đã trôi qua kể từ lần bắn tên lửa trước đó

    [SerializeField] private float timeStart;

    private bool isCheckTimeStart;

    //private GameObject bullet;
    private void Start()
    {
        life = 1;
    }

    private void Update()
    {
        if (!isCheckTimeStart)
        {
            if (timeSinceLastShot > timeStart)
            {
                isCheckTimeStart = true;
            }
        }

        timeSinceLastShot += Time.deltaTime;
        if (isCheckTimeStart)
        {
            // Tính toán thời gian đã trôi qua kể từ lần bắn tên lửa trước đó

            // Nếu đã đủ thời gian giữa các lần bắn tên lửa
            if (timeSinceLastShot >= fireRate)
            {
                // Đặt thời gian đã trôi qua về 0 để bắn tên lửa lần tiếp theo
                timeSinceLastShot = 0f;
                // Bắn tên lửa
                GameController.Instance.directionEnemy = transform;
                
                var obj = LeanPool.Spawn(rocketPrefab, firePoint.position, transform.rotation);
                gameObject.GetComponent<Animator>().SetBool("isCheckFire",true);
                Debug.Log(123);
                // Lấy vị trí của A và B trong không gian thế giới
                
                Vector3 posA = Camera.main.transform.position;
                Vector3 posB = obj.transform.position;

                // Tính toán hướng nhìn của obj
                Vector3 lookDir = (posA - posB).normalized;

                // Lấy góc hiện tại của obj trên trục Y
                float currentAngle = obj.transform.eulerAngles.y;

                // Tính toán góc mới của obj
                float newAngle = currentAngle;
                if (Vector3.Dot(obj.transform.forward, lookDir) > 0)
                {
                    newAngle = currentAngle + 180f;
                }

                // Áp dụng góc mới cho obj
                obj.transform.eulerAngles = new Vector3(0f, newAngle, 0f);
                LeanPool.Despawn(obj, 5);
            }
        }
    }

    void endAnimation()
    {
        gameObject.GetComponent<Animator>().SetBool("isCheckFire",false);
    }
    private void FixedUpdate()
    {
    }
}