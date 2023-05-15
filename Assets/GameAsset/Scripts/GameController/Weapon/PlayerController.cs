using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using JetBrains.Annotations;
using Lean.Pool;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    private float TimeDuration = .35f;
    private float timeHoldingVelocity;
    private bool isCheckSlowMotion;
    float timeDelay =0;
    private bool isCheckVelocitySlowForEnemy;
    private GameObject topGameObject;
    private void FixedUpdate()
    {
        if (!gameObject.name.Equals("Weapon 5"))
        {


            if (GameController.Instance.currentRotateSpeed > 0)
            {

                Quaternion deltaRotation =
                    Quaternion.Euler(Vector3.forward * GameController.Instance.currentRotateSpeed *
                                     Time.unscaledDeltaTime);
                gameObject.GetComponent<Rigidbody>()
                    .MoveRotation(gameObject.GetComponent<Rigidbody>().rotation * deltaRotation);
                GameController.Instance.currentRotateSpeed = Mathf.Max(0,
                    GameController.Instance.currentRotateSpeed - (GameController.Instance.decreaseRate *
                                                                  GameController.Instance.currentRotateSpeed *
                                                                  Time.unscaledDeltaTime));
            }
            else
            {
                // Nếu tốc độ quay đã giảm xuống đến 0, ta ngưng xoay đối tượng game.
                gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }
        }
        if (!GameController.Instance.Weapon_Rigidbody.useGravity)
        {
            float timeDuration = .5f;
            timeDelay += Time.deltaTime;
            if (timeDelay > timeDuration)
            {
                GameController.Instance.Weapon_Rigidbody.useGravity = true;
                timeDelay = 0;
            }
        }
    }

    private void Update()
    {
        if (Mathf.Abs(GameController.Instance.Weapon_Rigidbody.velocity.magnitude) < 0.5f)
        {
            // speed gần bằng 0
            GameController.Instance.Weapon_Rigidbody.drag = 3;
        }

        #region Chỉnh làm chậm thời gian

        if (isCheckSlowMotion)
        {
            //Time.timeScale += (1f / 2f) * Time.unscaledDeltaTime;
            Time.timeScale += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
            Time.fixedDeltaTime = Time.timeScale * .02f;
            
            if (Time.timeScale == 1)
            {
                isCheckSlowMotion = false;
            }

            if (!GameController.Instance.isCheckSceneHaveBoss)
            {
                GameController.Instance.EnemyTouch.GetComponentsInChildren<Rigidbody>().ToList().ForEach(item =>
                {
                    if (item.velocity.magnitude > 0.1f) // kiểm tra vận tốc có đủ nhỏ để dừng hay không
                    {
                        item.velocity -=
                            item.velocity.normalized * 1f; // giảm dần vận tốc theo hướng đối với một giá trị damping
                        item.AddForce(
                            GameController.Instance.DirectionOfJoystick.normalized * Time.unscaledDeltaTime * 40,
                            ForceMode.Impulse);
                    }
                    else
                    {
                        item.velocity = Vector3.zero; // nếu vận tốc đủ nhỏ, đặt vận tốc bằng vector 0
                    }
                });
            }
        }

        if (Config.Instance.isCheckTimeSlowMotion)
        {
            isCheckSlowMotion = false;
            Config.Instance.timeDurationSlowMotion += Time.unscaledDeltaTime;
            SlowMotion();
        }

        #endregion

        if (isCheckVelocitySlowForEnemy)
        {
            #region Set cho các box lực bằng 0 để không bị bay đi

            List<Rigidbody> listRigidbodyEnemy =
                topGameObject.GetComponentsInChildren<Rigidbody>().ToList();
            listRigidbodyEnemy.ForEach(item =>
            {
                item.velocity *= 0.98f; // giảm lực đi .01%
                item.angularVelocity *= 0.98f; // giảm lực xoay đi .01%
            });

            #endregion
        }

        if (Time.timeScale == 1)
        {
            isCheckVelocitySlowForEnemy = false;
        }
        
    }

    void SlowMotion()
    {
        if (Config.Instance.timeDurationSlowMotion >= Config.Instance.timeSlowMotion)
        {
            if (!isCheckSlowMotion)
            {
                isCheckSlowMotion = true;
                Config.Instance.isCheckTimeSlowMotion = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameController.Instance.Weapon_Rigidbody.drag = 3;
        GameController.Instance.Weapon_Rigidbody.useGravity = true;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Coin"))
        {
            #region Tạo effect khi ăn coin

            Vector3 screenPosition = Camera.main.WorldToScreenPoint(collision.gameObject.transform.position);
            GameController.Instance.CoinEffect.transform.position = screenPosition;
            GameController.Instance.CoinEffect.GetComponent<ParticleSystem>().Play();
            #endregion
            #region Get value coin
            //GameController.Instance.MoneyPlayer++;
            int countCoin = int.Parse(ControllerShop.Instance.text_Coin.text);
            countCoin++;
            ControllerShop.Instance.text_Coin.text = countCoin.ToString();
            LeanPool.Despawn(collision.gameObject);
            #endregion

        }
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("WallRight") ||
            collision.gameObject.CompareTag("WallLeft"))
        {
            GameController.Instance.isCheckDragJoystick = false;
            GameController.Instance.isCheckTouchEnemyForForce = false;
            GameController.Instance.Weapon_Rigidbody.constraints = RigidbodyConstraints.None;
            GameController.Instance.Weapon_Rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
        }

        if (collision.gameObject.CompareTag("Glasses"))
        {
            collision.transform.GetChild(0).gameObject.SetActive(false);
            collision.transform.GetChild(1).gameObject.SetActive(true);
            List<Rigidbody> listRigidbodyEnemy =
                collision.transform.GetChild(1).gameObject.GetComponentsInChildren<Rigidbody>().ToList();
            listRigidbodyEnemy.ForEach(item =>
            {
                item.AddForce(Vector3.up * Random.Range(3,4), ForceMode.Impulse);
                item.AddTorque(Vector3.up * Random.Range(3,4), ForceMode.Impulse);
                //StartCoroutine(BlinkB(item.gameObject));
            });
        }

        if (collision.gameObject.CompareTag("Wood"))
        {
            collision.gameObject.GetComponentsInChildren<Rigidbody>().ToList().ForEach(item =>
            {
                item.AddForce(GameController.Instance.DirectionOfJoystick * 9, ForceMode.Impulse);
                StartCoroutine(BlinkB(item.gameObject));
                item.isKinematic = false;
            });
        }

        if (Config.Instance.Taglist_DieEnemy.Contains(collision.gameObject.tag))
        {
            Transform topLevelParent = GetTopLevelParent(collision.transform);
            if (topLevelParent.GetComponent<BotController_ALL>().life == 0)
            {
                return;
            }
            //chạm vào enemy
            GameController.Instance.EnemyTouch = topLevelParent.gameObject;
            topGameObject = topLevelParent.gameObject;

            #region Cho thành true để giảm lực tối đa cho enemy

            isCheckVelocitySlowForEnemy = true;
            
            #endregion
            if (GameController.Instance.isCheckSceneHaveBoss)
            {
                #region Cộng vàng

                int countCoin = int.Parse(ControllerShop.Instance.text_Coin.text);
                countCoin+=150;
                GameController.Instance.MoneyPlayer += countCoin;
                ControllerShop.Instance.text_Coin.text = countCoin.ToString();

                #endregion
                for (int i = 0; i < GameController.Instance.List_PrefabsBossImageLive.Count; i++)
                {
                    if (GameController.Instance.List_PrefabsBossImageLive[i].GetComponent<Image>().color != Color.black)
                    {
                        GameController.Instance.List_PrefabsBossImageLive[i].GetComponent<Image>().color = Color.black;
                        break;
                    }
                }
            }

            #region Kiểm tra va chạm cho lực weapon

            GameController.Instance.isCheckTouchEnemyForForce = true;

            #endregion

            GameController.Instance.indexEnemy++; //combo kill
            GameController.Instance.CountCombo++; //combo kill

            #region Effect khi trúng enemy

            Vector3 collisionPoint = collision.bounds.center;
            Instantiate(GameController.Instance.effectPrefabs, collisionPoint, Quaternion.identity);

            #endregion

            //lấy ra gameobject cao nhất của collison(enemy) khi va phải 1 bộ phận của enemy
            if (collision.gameObject.CompareTag("Die_Head") &&
                topLevelParent.GetComponent<BotController_ALL>().life != 0)
            {
                ControllerShop.Instance.HeadShot();

                if (!GameController.Instance.isCheckSceneHaveBoss)
                {
                    topLevelParent.GetComponentsInChildren<Transform>().ToList().ForEach(item => item.tag = "Untagged");
                    
                }
            }
            else if (collision.gameObject.CompareTag("Die_Other") &&
                     topLevelParent.GetComponent<BotController_ALL>().life != 0)
            {
                if (!ControllerShop.Instance.isCheckCoroutineOtherShot)
                {
                    ControllerShop.Instance.OtherShot();
                }
                else
                {
                    ControllerShop.Instance.GO_OtherShot.SetActive(false);
                    ControllerShop.Instance.StopCoroutineOtherShot();
                    ControllerShop.Instance.OtherShot();
                }

                if (!GameController.Instance.isCheckSceneHaveBoss)
                {
                topLevelParent.GetComponentsInChildren<Transform>().ToList().ForEach(item=>item.tag="Untagged");
                    
                }
            }

            if (topLevelParent.GetComponent<BotController_ALL>().life != 0 &&
                (GameController.Instance.CountCombo == GameController.Instance.CountComboBefore ||
                 GameController.Instance.CountCombo > GameController.Instance.CountComboBefore))
            {
                ControllerShop.Instance.randomCombo = Random.Range(0, 3);
                if (!ControllerShop.Instance.isCheckCoroutineComboShot)
                {
                    ControllerShop.Instance.ComboShot(ControllerShop.Instance.randomCombo);
                }
                else
                {
                    ControllerShop.Instance.GO_ComboKill[ControllerShop.Instance.randomCombo].SetActive(false);
                    ControllerShop.Instance.StopCoroutineComboShot();
                    ControllerShop.Instance.ComboShot(ControllerShop.Instance.randomCombo);
                }
            }


            #region Tắt scripts

            //bot thường
            if (topLevelParent.GetComponent<ZombieMove>() != null)
            {
                topLevelParent.DOKill();
                topLevelParent.GetComponent<ZombieMove>().enabled = false;
                Destroy(topLevelParent.GetComponent<ZombieMove>());//xóa hẳn để không vào trong script nữa
            }
            //tắt ngay botcontroller để tránh lỗi di chuyển
            if (topLevelParent.GetComponent<BotController>() != null)
            {
                topLevelParent.GetComponent<BotController>().enabled = false;
            }
            else if (topLevelParent.GetComponent<Enemy_FireController>() != null)
            {
                topLevelParent.GetComponent<Enemy_FireController>().enabled = false;
            }
            else if (topLevelParent.GetComponent<BossController>() != null)
            {
                topLevelParent.GetComponent<BossController>().enabled = false;
            }
            if (topLevelParent.GetComponent<EnemyMove>() != null)
            {
                GameController.Instance.isCheckEnemyDefend = true;
                topLevelParent.transform.DOKill();
                //topLevelParent.GetComponent<EnemyMove>().enabled = false;
            }
            if (topLevelParent.GetComponent<Bot_Leve10>() != null)
            {
                topLevelParent.GetComponent<Bot_Leve10>().enabled = false;
            }

            #endregion

            #region Check enemy để xóa trong danh sách

            GameController.Instance.countEnemy = GameController.Instance.listEnemy.Count;
            if (!GameController.Instance.isCheckSceneHaveBoss)
            {
                for (int i = 0; i < GameController.Instance.countEnemy; i++)
                {
                    if (topLevelParent.gameObject.GetInstanceID() ==
                        GameController.Instance.listEnemy[i].GetInstanceID())
                    {
                        GameController.Instance.listEnemy.RemoveAt(i);
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < GameController.Instance.countEnemy; i++)
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
                if (topLevelParent.GetComponent<BotController_ALL>().life != 0 &&
                    GameController.Instance.list_GO_imageEnemeDied[i] != null)
                {
                    GameController.Instance.list_GO_imageEnemeDied[i].GetComponent<Image>().color = Color.gray;
                    GameController.Instance.list_GO_imageEnemeDied.RemoveAt(i);
                    break;
                }
            }

            #endregion

            if (topLevelParent.GetComponent<Animator>() != null)
            {
                //Ngưng animation để bật ragdoll lên
                topLevelParent.GetComponent<Animator>().enabled = false;
                if (topLevelParent.GetComponent<Enemy_FireController>() != null &&
                    topLevelParent.GetComponent<Enemy_FireController>().enabled)
                {
                    topLevelParent.GetComponent<Enemy_FireController>().enabled = false;
                }

                if (!GameController.Instance.isCheckSceneHaveBoss)
                {
                    #region Ngưng thời gian khi Weapon chạm vào Enemy

                    ControllerShop.Instance.TouchEnemy.Play();
                    Time.timeScale = .015f;
                    Time.fixedDeltaTime = Time.timeScale * .02f;

                    #region Cài đặt thời gian cho slowmotion

                    Config.Instance.timeDurationSlowMotion = 0;
                    Config.Instance.isCheckTimeSlowMotion = true;

                    #endregion

                    #endregion

                    #region Thay màu cho enemy khi bị tấn công

                    List<SkinnedMeshRenderer> listMaterial =
                        topLevelParent.GetComponentsInChildren<SkinnedMeshRenderer>().ToList();
                    listMaterial.ForEach(item => item.material = GameController.Instance.colorEnemyDie[0]);

                    #endregion
                }
                else
                {
                    #region Ngưng thời gian khi Weapon chạm vào Enemy

                    ControllerShop.Instance.TouchEnemy.Play();
                    Time.timeScale = .375f;
                    Time.fixedDeltaTime = Time.timeScale * .02f;

                    #region Cài đặt thời gian cho slowmotion

                    Config.Instance.timeDurationSlowMotion = 0;
                    Config.Instance.isCheckTimeSlowMotion = true;

                    #endregion
                    #endregion
                }

                if (topLevelParent.GetComponent<BotController_ALL>() != null)
                {
                    topLevelParent.GetComponent<BotController_ALL>().life = 0;
                }

                if (!GameController.Instance.isCheckSceneHaveBoss)
                {
                    //Debug.Log(123);
                    StartCoroutine(Blink(topLevelParent.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>(),
                        topLevelParent));
                }
                else
                {
                    #region Kiểm tra xem scene có Boss không

                    //
                    GameController.Instance.GO_BOSS = topLevelParent.gameObject;
                    //
                    StartCoroutine(DelaySpamBoss(topLevelParent.gameObject));

                    #endregion
                }

                #region số lượng enemy về 0 , chiến thắng
                
                if (GameController.Instance.listEnemy.Count <= 0)
                {
                    if (GameController.Instance.Arrow.activeSelf)
                    {
                        GameController.Instance.Arrow.SetActive(false);
                    }
                    
                    if (!GameController.Instance.isWinnerOrLoser)
                    {
                        StartCoroutine(DelayWin());
                    }
                }
                
                #endregion
            }
        }
    }
    
    #region Delay thời gian hồi sinh của boss

    IEnumerator DelaySpamBoss(GameObject boss)
    {
        GameController.Instance.trails = LeanPool.Spawn(GameController.Instance.trailBOSS, GameController.Instance.GO_BOSS.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(.55f);
        
        //LeanPool.Despawn(boss);
        if (GameController.Instance.lastNumber >= 20)
        {
            if (GameController.Instance.listEnemy.Count == 3)
            {
                GameController.Instance.isCheckLiveBoss = true;
            }
            else if (GameController.Instance.listEnemy.Count == 2)
            {
                GameController.Instance.isCheckLiveBoss2 = true;
            }
            else if (GameController.Instance.listEnemy.Count == 1)
            {
                            
                GameController.Instance.isCheckLiveBoss3 = true;
            }
        }
        else
        {
            if (GameController.Instance.listEnemy.Count == 2)
            {
                GameController.Instance.isCheckLiveBoss = true;
            }
            else if (GameController.Instance.listEnemy.Count == 1)
            {
                GameController.Instance.isCheckLiveBoss2 = true;
               // Debug.Log(1);
            }
                        
        }
    }

        #endregion

    IEnumerator DelayWin()
    {
        yield return new WaitForSeconds(.2f);
        GameController.Instance.isWinnerOrLoser = true;
        GameController.Instance.WinGame();
    }

    IEnumerator Blink(SkinnedMeshRenderer myRenderer, Transform topLevelParent)
    {
        float timer = 0f;
        yield return new WaitForSecondsRealtime(2f);
        while (timer < 3f)
        {
            if (myRenderer != null)
            {
                // chạy trong 2 giây
                myRenderer.enabled = !myRenderer.enabled;
                yield return new WaitForSecondsRealtime(0.1f); // đợi 0.1 giây
                timer += 0.1f;
            }
        }

        myRenderer.enabled = false; // bật Renderer khi kết thúc
        if (topLevelParent != null)
        {
            //Destroy(topLevelParent.gameObject);
            List<Collider> list_ColliderEnemy = topLevelParent.GetComponentsInChildren<Collider>().ToList();
            list_ColliderEnemy.ForEach(item => item.enabled = false);
            yield return new WaitForSecondsRealtime(.25f);
            LeanPool.Despawn(topLevelParent);
        }
    }
    
    IEnumerator BlinkB(GameObject topLevelParent)
    {
        yield return new WaitForSecondsRealtime(3f);
        topLevelParent.SetActive(false);
    }
    /*private void OnTriggerEnter(Collider other)
    {
        GameController.Instance.collider_Ground = other.gameObject;
    }*/

    #region Fucntion : Lấy ra phần tử có Animator Component

    public static Transform GetTopLevelParent(Transform transform)
    {
        if (transform.gameObject.GetComponent<Animator>() != null)
        {
            // Nếu parent của transform là null, tức là transform đang nằm ở top-level của hierarchy
            return transform;
        }
        else
        {
            // Nếu transform vẫn còn parent, ta sẽ gọi đệ quy để tiếp tục tìm parent của parent đó
            return GetTopLevelParent(transform.parent);
        }
    }

    #endregion
}