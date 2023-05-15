using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class DestroyMonster : MonoBehaviour
{
    public void Destroy_Monster()
    {
        GameController.Instance.isCheckMonster = false;
        Destroy(GameController.Instance.monster);
    }

    /*public void Move_LeftToRight()
    {
        GameController.Instance.animatorMonster = GameController.Instance.GO_monster.transform.GetChild(0).gameObject.GetComponent<Animator>();
        GameController.Instance.animatorMonster.SetTrigger("Move_LeftToRight");
        Debug.Log(GameController.Instance.animatorMonster.GetCurrentAnimatorClipInfo(0)[0].clip.name);
    }*/
}
