using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Lean.Pool;
using UnityEngine;
using UnityEngine.UI;

public class Bomb : MonoBehaviour
{
    public float radius = 5f;
    public float power = 10f;
    public float upwardModifier = 3f;
    public GameObject explosionPrefab;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            // Kiểm tra va chạm với các đối tượng trong hình cầu
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

            foreach (Collider hit in colliders)
            {
                //lấy ra enemy nằm trong vùng và cho nó nổ

                if (hit.gameObject.CompareTag("Die_Other"))
                {
                    Transform enemy = PlayerController.GetTopLevelParent(hit.transform);
                    if (enemy.GetComponent<BotController_ALL>().life == 1)
                    {
                        GameController.Instance.indexEnemy++;
                    }
                    //bot thường
                    if (enemy.GetComponent<BotController>() != null)
                    {
                        enemy.GetComponent<BotController>().enabled = false;
                    }

                    //bot bắn xa
                    if (enemy.GetComponent<Enemy_FireController>() != null)
                    {
                        enemy.GetComponent<Enemy_FireController>().enabled = false;
                    }

                    //boss
                    if (enemy.GetComponent<BossController>() != null)
                    {
                        enemy.GetComponent<BossController>().enabled = false;
                    }

                    enemy.GetComponent<BotController_ALL>().enabled = false;
                    enemy.GetComponent<Animator>().enabled = false;
                    StartCoroutine(Blink(enemy.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>(),
                        enemy));

                    #region Thay màu cho enemy

                    List<SkinnedMeshRenderer> listMaterial =
                        enemy.GetComponentsInChildren<SkinnedMeshRenderer>().ToList();
                    listMaterial.ForEach(item => item.material = GameController.Instance.colorEnemyDie[0]);

                    #endregion

                    #region Check enemy để xóa trong danh sách

                    int countEnemy = GameController.Instance.listEnemy.Count;
                    for (int i = 0; i < countEnemy; i++)
                    {
                        if (enemy.gameObject.GetInstanceID() == GameController.Instance.listEnemy[i].GetInstanceID())
                        {
                            GameController.Instance.listEnemy.RemoveAt(i);
                            break;
                        }
                    }

                    #endregion

                    #region Hiển thị lên thanh đã chết 1 mục tiêu

                    int count = GameController.Instance.list_GO_imageEnemeDied.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (enemy.GetComponent<BotController_ALL>().life != 0 &&
                            GameController.Instance.list_GO_imageEnemeDied[i] != null)
                        {
                            GameController.Instance.list_GO_imageEnemeDied[i].GetComponent<Image>().color = Color.gray;
                            GameController.Instance.list_GO_imageEnemeDied.RemoveAt(i);
                            break;
                        }
                    }

                    #endregion

                    if (enemy.GetComponent<BotController_ALL>() != null)
                    {
                        enemy.GetComponent<BotController_ALL>().life = 0;
                    }

                    #region Check win cho game

                    bool isCheckLife = false;
                    for (int i = 0; i < GameController.Instance.listEnemy.Count; i++)
                    {
                        if (GameController.Instance.listEnemy[i].GetComponent<BotController_ALL>().life == 1)
                        {
                            isCheckLife = false;
                            break;
                        }
                        else
                        {
                            isCheckLife = true;
                        }
                    }

                    isCheckLife = GameController.Instance.listEnemy.Count == 0 ? true : false;
                    if (isCheckLife)
                    {
                        if (GameController.Instance.Arrow.activeSelf)
                        {
                            GameController.Instance.Arrow.SetActive(false);
                        }

                        GameController.Instance.WinGame();
                    }

                    #endregion
                }

                //cho player ảnh hưởng bởi vụ nổ
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Animator animator = hit.GetComponent<Animator>();
                    BotController botController = hit.GetComponent<BotController>();
                    BotController_ALL botControllerAll = hit.GetComponent<BotController_ALL>();
                    if (botController != null)
                    {
                        botController.enabled = false;
                    }

                    if (botControllerAll != null)
                    {
                        botControllerAll.enabled = false;
                    }

                    if (animator != null)
                    {
                        animator.enabled = false;
                    }

                    rb.AddExplosionForce(power, transform.position, radius, upwardModifier, ForceMode.Impulse);
                }
            }


            // Hiển thị hiệu ứng nổ
            LeanPool.Spawn(explosionPrefab, transform.position, Quaternion.identity);

            // Destroy GameObject của quả bom
            Destroy(gameObject);
        }
    }

    IEnumerator Blink(SkinnedMeshRenderer myRenderer, Transform topLevelParent)
    {
        float timer = 0f;
        yield return new WaitForSeconds(1f);
        while (timer < 2f)
        {
            if (myRenderer != null)
            {
                // chạy trong 2 giây
                myRenderer.enabled = !myRenderer.enabled;
                yield return new WaitForSeconds(0.1f); // đợi 0.1 giây
                timer += 0.1f;
            }
        }

        myRenderer.enabled = false; // bật Renderer khi kết thúc
        if (topLevelParent != null)
        {
            //Destroy(topLevelParent.gameObject);
            List<Collider> list_ColliderEnemy = topLevelParent.GetComponentsInChildren<Collider>().ToList();
            list_ColliderEnemy.ForEach(item => item.enabled = false);
            yield return new WaitForSeconds(.5f);
            LeanPool.Despawn(topLevelParent);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Vẽ hình cầu để kiểm tra xem những đối tượng nào nằm trong đó
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}