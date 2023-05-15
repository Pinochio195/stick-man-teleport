using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class ZombieMove : MonoBehaviour
{
    private bool check;
    public bool isCheck;
    private int num;
    public GameObject particleGO;
    private ParticleSystem particle;
    [SerializeField] private Animator anim;
    [Range(0, 40)] public float speedWalk;
    [SerializeField] private Transform posMove1;
    [SerializeField] private Transform posMove2;
    private bool isCheckKill;
    public Transform killPosition;

    void Start()
    {
        num = 1;
    }

    // Update is called once per frame
    void Update()
    {
        Enemy_Move();
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

//Thu sau khi bị cắn
    private void LoserGame()
    {
        ControllerShop.Instance.LoserGame();
    }

    public void EnableRagdoll()
    {
        GameController.Instance.Player_Position.GetComponent<Rigidbody>().isKinematic = false;
    }
    //cho zombie
    private void OnTriggerEnter(Collider collision)
    {
        /*if (collision.gameObject.CompareTag("Player"))
        {
            #region Cho tất cả các box disable để thực hiện animation

            gameObject.GetComponentsInChildren<Collider>().ToList().ForEach(item => item.enabled = false);

            #endregion
            if (!isCheckKill)
            {
                isCheckKill = true;

                #region Cho zombie

                isCheck = true;
                transform.DOKill();
                GetComponent<Animator>().SetBool("isWalking", true);
                //float offset = direction.x > 0 ? -1.5f : 1.5f;
                collision.transform.root.GetComponent<Rigidbody>().isKinematic = true;
                collision.transform.root.transform.position = killPosition.position;
                float offset1 = (Mathf.Abs(transform.position.x) - Mathf.Abs(collision.transform.GetChild(0).position.x)) < 0 ? -90 : 90;
                Debug.Log((Mathf.Abs(transform.position.x) - Mathf.Abs(collision.transform.GetChild(0).position.x)));
                collision.transform.root.transform.rotation = Quaternion.Euler(new Vector3(0f, offset1, 0f));


                #endregion
            }
        }*/
    }
   

    public void HitPlayer()
    {
        isCheck = true;
        particle = particleGO.GetComponent<ParticleSystem>();
        particle.Play();
    }
}