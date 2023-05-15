using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bom1 : MonoBehaviour
{
    public Collider col;

    [SerializeField] private float range;

    private void OnEnable()
    {
        col.enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        explode();
    }

    public void explode()
    {
        col.enabled = false;
        Collider[] enemy_col = Physics.OverlapSphere(transform.position, range);

        foreach (Collider enemy in enemy_col)
        {
            if (enemy.GetComponent<Enemy_BY_Bom>() != null)
            {
                enemy.GetComponent<Enemy_BY_Bom>().die(transform.position,
                    GameController.Instance.Player_Position.GetComponentsInChildren<Rigidbody>().ToList());
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}