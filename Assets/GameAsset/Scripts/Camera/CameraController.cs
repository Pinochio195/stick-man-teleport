using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //public Transform target; // player's transform
    public float smoothSpeed = 0.125f;

    public Vector3 offset;
    public float minX = -3f; // giới hạn trục X bên trái
    public float maxX = 3f; // giới hạn trục X bên phải
    private float maxY; // giới hạn trục Y
    public float minDistance = 5f; // khoảng cách tối thiểu giữa target và camera

    public Renderer cameraBound; // GameObject giới hạn camera không được vượt quá

    private Vector3 _velocity;
    private Camera mainCamera;
    private float SizeCam;
    private Vector3 desiredPosition;
    private Vector3 desiredPosition1;
    private Vector3 smoothedPosition;
    private Vector3 smoothedPosition1;
    private float clampedX;
    private bool isVisible;
    private void Start()
    {
        maxY = transform.position.y;
        mainCamera = Camera.main;
        SizeCam = mainCamera.orthographicSize;
        
    }

    private void Update()
    {
        // Kiểm tra nếu Renderer và Camera tồn tại
        if (cameraBound != null && mainCamera != null)
        {
            // Kiểm tra nếu đối tượng lọt vào cửa sổ xem của camera
            isVisible = GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(mainCamera), cameraBound.bounds);
            /*// Kiểm tra isVisible
            if (isVisible)
            {
                Debug.Log("Đối tượng nằm trong tầm nhìn của camera");
            }
            else
            {
                Debug.Log("Đối tượng không nằm trong tầm nhìn của camera");
            }*/
        }
    }

    private void FixedUpdate()
    {
       
        if (!GameController.Instance.isLoser)
        {
            if (GameController.Instance.Weapon != null)
            {
                desiredPosition = GameController.Instance.Weapon.transform.position + offset;
                float distance = Vector3.Distance(desiredPosition, transform.position);

                // Nếu khoảng cách giữa target và camera lớn hơn minDistance, camera theo dõi target
                if (distance > minDistance)
                {
                    smoothedPosition =
                        Vector3.SmoothDamp(transform.position, desiredPosition, ref _velocity, smoothSpeed);
                }
                // Ngược lại, camera giữ vị trí hiện tại
                else
                {
                    smoothedPosition = transform.position;
                }

                float clampedX = Mathf.Clamp(smoothedPosition.x, minX, maxX);
                float newY = Mathf.Max(smoothedPosition.y, maxY);

                // Lấy giới hạn trên của camera không được vượt quá khu vực cho phép
                float cameraBoundY = cameraBound.transform.position.y - (SizeCam * 0.5f);

                // Nếu vị trí Y mới lớn hơn giới hạn trên của camera, đặt vị trí Y mới của camera là giới hạn trên
                /*if (newY > cameraBoundY)
                {
                    newY = cameraBoundY;
                }*/
                if (isVisible && newY >= transform.position.y)
                {
                    newY = transform.position.y;
                }
                
                transform.position = new Vector3(clampedX, newY, transform.position.z);
            }
        }
        else
        {
            if (mainCamera.orthographicSize >=4)
            {
                mainCamera.orthographicSize = Mathf.MoveTowards(mainCamera.orthographicSize, 4f, Time.deltaTime * 7f);
            }
            if (GameController.Instance.Player_Position != null)
            {
                desiredPosition1 = GameController.Instance.Player_Position.transform.position+new Vector3(0,2,0);
                float distance = Vector3.Distance(desiredPosition1, transform.position);
                // Nếu khoảng cách giữa target và camera lớn hơn minDistance, camera theo dõi target
                if(distance > minDistance)
                {
                    smoothedPosition1 = Vector3.Lerp(transform.position, desiredPosition1, Time.deltaTime * 2f);
                }

                float targetX = -3;
                float currentX = transform.position.x;
                float smoothX = 0.8f;
                float clampedX = Mathf.Clamp(smoothedPosition.x, minX, maxX);
                float smoothDampX = Mathf.SmoothDamp(currentX, targetX, ref clampedX, smoothX);
                transform.position = new Vector3(smoothedPosition1.x, smoothedPosition1.y, transform.position.z);
            }
        }
            
        
    }
}