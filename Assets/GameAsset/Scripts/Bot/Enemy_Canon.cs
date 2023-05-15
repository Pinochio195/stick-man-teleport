using Lean.Pool;
using UnityEngine;

public class Enemy_Canon : MonoBehaviour
{
    public int life;
    [SerializeField] private Transform BossPosition;
    [SerializeField] float lookRadius; // Khoảng cách nhìn của bot
    [SerializeField] LayerMask WhatIsGround; // Layer của player
    [SerializeField] bool isPlayerInSight; // Kiểm tra xem player có trong tầm nhìn của bot hay không

    public GameObject rocketPrefab; // Đối tượng tên lửa để bắn ra
    public Transform firePoint; // Vị trí để bắn đối tượng tên lửa ra
    public float fireRate; // Tốc độ bắn (bắn một lần mỗi giây)
    private Rigidbody rocketRigidbody; // Component Rigidbody của đối tượng tên lửa
    private float timeSinceLastShot = 0f; // Thời gian đã trôi qua kể từ lần bắn tên lửa trước đó
    public Animator enemy;
    private void Start()
    {
        life = 1;
    }

    private void Update()
    {
        if (!enemy.enabled)
        {
            gameObject.GetComponent<Enemy_Canon>().enabled = false;
        }
        if (GameController.Instance.Player != null)
        {
            float distance = Vector3.Distance(GameController.Instance.Player.transform.position, BossPosition.position);

            if (distance <= lookRadius)
            {
                bool isPlayerInSight = true;
                RaycastHit hit;
                if (Physics.Linecast(BossPosition.position, GameController.Instance.Player.transform.position, out hit,
                        WhatIsGround))
                {
                    if (hit.collider.CompareTag("Ground"))
                    {
                        isPlayerInSight = false;
                    }
                    else
                    {
                        isPlayerInSight = true;
                    }
                }

                if (isPlayerInSight)
                {
                    // Lấy vị trí của player
                    Vector3 targetPosition = GameController.Instance.Player_Position.position;

                    // Đưa vị trí player vào hàm LookAt để khẩu pháo hướng về player

                    Vector3 relativePosition = GameController.Instance.Player_Position.position - transform.position;
                    float angle = Mathf.Atan2(relativePosition.y, relativePosition.x) * Mathf.Rad2Deg;
                    Debug.Log(angle);
                    if (angle < -19 && angle > -150  || angle > 60 && angle < 130)
                    {
                        return;
                    }
                    transform.LookAt(targetPosition);
                    
                    #region Bắn đạn theo thời gian

                    // Tính toán thời gian đã trôi qua kể từ lần bắn tên lửa trước đó
                    timeSinceLastShot += Time.deltaTime;

                    // Nếu đã đủ thời gian giữa các lần bắn tên lửa
                    if (timeSinceLastShot >= fireRate)
                    {
                        // Đặt thời gian đã trôi qua về 0 để bắn tên lửa lần tiếp theo
                        timeSinceLastShot = 0f;
                        // Bắn tên lửa
                        var obj = LeanPool.Spawn(rocketPrefab, firePoint.position, transform.rotation);
                        obj.transform.position = firePoint.position;
                        obj.GetComponent<Rigidbody>().velocity = relativePosition * 50 * Time.deltaTime;
                        LeanPool.Despawn(obj,5);

                    }

                    #endregion
                    
                }
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(BossPosition.position, lookRadius);
    }
}
