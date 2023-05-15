using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Lean.Pool;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    private bool check;
    private bool isCheck;
    private int num;
    public GameObject particleGO;
    private ParticleSystem particle;
    [SerializeField] private Animator anim;
    [Range(0, 40)] public float speedWalk;
    [SerializeField] private Transform posMove1;
    [SerializeField] private Transform posMove2;
    [SerializeField] private GameObject woodDefend;

    void Start()
    {
        num = 1;
    }

    // Update is called once per frame
    void Update()
    {
        Enemy_Move();
        if (woodDefend != null)
        {
            if (!anim.enabled)
            {
                woodDefend.GetComponent<Rigidbody>().isKinematic = false;
                woodDefend.transform.SetParent(null);
                
            }
        }

        if (woodDefend != null)
        {
            if (GameController.Instance.isCheckEnemyDefend)
            {
                isCheck = true;
                woodDefend.GetComponent<Rigidbody>().isKinematic = false;
                woodDefend.GetComponent<Rigidbody>().AddForce(transform.forward * 10, ForceMode.Impulse);
                Destroy(woodDefend.GetComponent<DiePlayerForObstacle>());
                GameController.Instance.isCheckEnemyDefend = false;
            }
        }
    }

    private void Enemy_Move()
    {
        if (!isCheck)
        {
            if (check)
            {
                if (num == 0)
                {
                    num = 1;
                    anim.Play("Walking");
                    Vector3 direction1 = posMove2.position - transform.position;
                    float angle1 = Mathf.Atan2(direction1.x, direction1.z) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.AngleAxis((angle1), Vector3.up);
                    float time = Vector3.Distance(transform.position, posMove2.position) / speedWalk;
                    transform.DOKill();
                    transform.DOMove(posMove2.position, time).SetEase(Ease.Linear).OnComplete(() => { check = false; });
                }
            }
            else
            {
                if (num == 1)
                {
                    num = 0;
                    anim.Play("Walking");

                    Vector3 direction1 = posMove1.position - transform.position;
                    float angle1 = Mathf.Atan2(direction1.x, direction1.z) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.AngleAxis((angle1), Vector3.up);

                    float time = Vector3.Distance(transform.position, posMove1.position) / speedWalk;

                    transform.DOKill();
                    transform.DOMove(posMove1.position, time).SetEase(Ease.Linear).OnComplete(() => { check = true; });
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
    }

    //cho zombie
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log(collision.gameObject.name);

        #region Cho zombie

        isCheck = true;
        GetComponent<Animator>().SetBool("isWalking", true);
        // Tính toán hướng từ object A đến object B
        Vector3 direction = collision.transform.position - transform.position;
        direction.z = 0f;

        // Tính toán góc quay từ hướng tính toán được
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Quay object A về hướng object B
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        #endregion
    }

    public void HitPlayer()
    {
        isCheck = true;
        particle = particleGO.GetComponent<ParticleSystem>();
        particle.Play();
    }
}