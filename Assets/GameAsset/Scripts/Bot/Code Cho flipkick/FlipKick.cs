using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
public class FlipKick : MonoBehaviour
{
    private bool check;

    private int num;

    [SerializeField] private Animator anim;
    [Range(0, 40)] public float speedWalk;
    [SerializeField]private Transform posMove1;
    [SerializeField]private Transform posMove2;
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
                transform.DOMove(posMove2.position, time).SetEase(Ease.Linear).OnComplete(() =>
                {
                    check = false;
                });
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
                transform.DOMove(posMove1.position, time).SetEase(Ease.Linear).OnComplete(() =>
                {
                    check = true;
                });
            }

        }
    }
}