using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossController : MonoBehaviour
{
    public int life;
    [SerializeField] private Transform BossPosition;
    [SerializeField] float lookRadius; // Khoảng cách nhìn của bot
    [SerializeField] LayerMask WhatIsGround; // Layer của player
    [SerializeField] Animator animator; // Animator của bot
    [SerializeField] Transform WhereIsFireBom;
    public GameObject GO_BOM;
    [SerializeField] bool isPlayerInSight; // Kiểm tra xem player có trong tầm nhìn của bot hay không
    [SerializeField] private GameObject BomStatic;
    public Transform sinhEnemyPosition1;
    public Transform sinhEnemyPosition2;
    public Transform sinhEnemyPosition3;
    public static GameObject BOM;
    

    private void Start()
    {
        life = 1;
    }

    private void Update()
    {
        if (!GameController.Instance.isCheckGetPositionBoss && GameController.Instance.isCheckSceneHaveBoss)
        {
            GameController.Instance.Position1 = sinhEnemyPosition1;
            GameController.Instance.Position2 = sinhEnemyPosition2;
            GameController.Instance.Position1.position = sinhEnemyPosition1.position;
            GameController.Instance.Position2.position = sinhEnemyPosition2.position;
            if (GameController.Instance.lastNumber >= 20)
            {
                GameController.Instance.Position3 = sinhEnemyPosition3;
                GameController.Instance.Position3.position = sinhEnemyPosition3.position;
            }
            GameController.Instance.isCheckGetPositionBoss = true;
        }
        if (GameController.Instance.Player != null)
        {
            float distance = Vector3.Distance(GameController.Instance.Player_Position.transform.position, BossPosition.position);

            if (distance <= lookRadius)
            {
                bool isPlayerInSight = true;
                RaycastHit hit;
                if (Physics.Linecast(BossPosition.position, GameController.Instance.Player_Position.transform.position, out hit,
                        WhatIsGround))
                {
                    if (hit.collider.CompareTag("Ground"))
                    {
                        isPlayerInSight = false;
                        Debug.DrawLine(BossPosition.position, GameController.Instance.Player.transform.position,Color.red);
                        Debug.Log(hit.collider.name);
                    }
                    else
                    {
                        Debug.Log(2);
                        isPlayerInSight = true;
                    }
                }

                animator.SetBool("isWalking", isPlayerInSight);
                if (isPlayerInSight)
                {
                    transform.LookAt(
                        new Vector3(GameController.Instance.Player_Position.transform.position.x, transform.position.y,
                            GameController.Instance.Player_Position.transform.position.z), Vector3.up);
                }
            }
            else
            {
                animator.SetBool("isWalking", false);
            }
        }
    }

    public void SpawnBom()
    {
        BOM = LeanPool.Spawn(GO_BOM, WhereIsFireBom.position, Quaternion.Euler(0,90,0));
        BOM.transform.GetChild(0).gameObject.SetActive(true);
        GameController.Instance.meshRenderer = BOM.transform.GetChild(0).GetComponent<MeshRenderer>();
        GameController.Instance._collider = BOM.GetComponent<Collider>();
        GameController.Instance._collider.enabled = true;
        GameController.Instance.meshRenderer.enabled = true;
        Vector3 directionBom = GameController.Instance.Player.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(directionBom);
        BOM.transform.rotation = rotation;
        Vector3 direction = (new Vector3(GameController.Instance.Player.transform.position.x,GameController.Instance.Player.transform.position.y+1,GameController.Instance.Player.transform.position.z)-BOM.transform.position).normalized;
        BOM.GetComponent<Rigidbody>().velocity = direction * 750f*Time.deltaTime;
        BomStatic.SetActive(false);
        //StartCoroutine(MyCoroutine());
    }

    public void MyBombSpam()
    {
        BomStatic.SetActive(true);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(BossPosition.position, lookRadius);
    }

    
}