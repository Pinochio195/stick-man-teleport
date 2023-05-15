using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class Bot_BumerangController : MonoBehaviour
{
    [SerializeField] private Transform BossPosition;
    [SerializeField] float lookRadius; // Khoảng cách nhìn của bot
    [SerializeField] LayerMask WhatIsGround; // Layer của player
    [SerializeField] Animator animator; // Animator của bot
    [SerializeField] Transform WhereIsFireBom;
    [SerializeField] GameObject GO_BOM;
    [SerializeField] GameObject Player;
    [SerializeField] bool isPlayerInSight; // Kiểm tra xem player có trong tầm nhìn của bot hay không

    private void Update()
    {
        
            float distance = Vector3.Distance(Player.transform.position, BossPosition.position);

            if (distance <= lookRadius)
            {
                bool isPlayerInSight = true;
                RaycastHit hit;
                if (Physics.Linecast(BossPosition.position, Player.transform.position, out hit,
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

                animator.SetBool("isWalking", isPlayerInSight);
                if (isPlayerInSight)
                {
                    transform.LookAt(
                        new Vector3(Player.transform.position.x, transform.position.y,
                            Player.transform.position.z), Vector3.up);
                }
            }
            else
            {
                animator.SetBool("isWalking", false);
            }
        
    }

    public void SpawnBom()
    {
        GameObject BOM = LeanPool.Spawn(GO_BOM, WhereIsFireBom.position, Quaternion.identity);
        //Vector3 direction = (new Vector3(Player.transform.position.x,Player.transform.position.y+1,Player.transform.position.z)-BOM.transform.position).normalized;
        BOM.GetComponent<BulletBumerang>().Shoot();
        //BOM.GetComponent<Rigidbody>().AddForce(direction * 5f, ForceMode.Impulse);
        LeanPool.Despawn(BOM,2);
        
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(BossPosition.position, lookRadius);
    }
}
