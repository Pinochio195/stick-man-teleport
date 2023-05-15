using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BY_Bom : MonoBehaviour
{
    public void die(Vector3 huong, List<Rigidbody> ragdollBody)
    {
        Vector3 newhuong = huong - transform.position;

        if (newhuong.x >= 1)
        {
            newhuong.x = 1;
        }
        else if (newhuong.x <= -1)
        {
            newhuong.x = -1;
        }

        if (newhuong.y >= 1)
        {
            newhuong.y = 1;
        }
        else if (newhuong.y <= -1)
        {
            newhuong.y = -1;
        }

        if (newhuong.z >= 1)
        {
            newhuong.z = 1;
        }
        else if (newhuong.z <= -1)
        {
            newhuong.z = -1;
        }
        foreach (Rigidbody rb in ragdollBody)
        {
            rb.AddExplosionForce(100f*Config.Instance.DameOfBomb, new Vector3(0 , 0 , -1f), 5f, 5f, ForceMode.Impulse);
        }
    }
}
