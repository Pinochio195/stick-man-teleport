using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class BomBossWhite : MonoBehaviour
{
    [SerializeField] private GameObject particlePrefab;
    private MeshRenderer meshRenderer;
    private void Awake()
    {
        meshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
    }

    private void OnEnable()
    {
        meshRenderer.enabled = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        #region Check khi va chạm vào bullet

        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("LeftLeg") ||
            collision.gameObject.CompareTag("RightLeg"))
        {
            ControllerShop.Instance.LoserGame();
        }

        #endregion
        if (collision.gameObject.CompareTag("Weapon") || collision.gameObject.CompareTag("Ground") ||
            collision.gameObject.CompareTag("WallLeft") || collision.gameObject.CompareTag("WallRight")|| collision.gameObject.CompareTag("Player"))
        {
            GameController.Instance.list_musicBoom.Add(LeanPool.Spawn(GameController.Instance.audioSource, transform.position, Quaternion.identity));
            #region Vùng nổ
            // Kiểm tra va chạm với các đối tượng trong hình cầu
            Collider[] colliders = Physics.OverlapSphere(transform.position, .5f);

            foreach (Collider hit in colliders)
            {
                //lấy ra enemy nằm trong vùng và cho nó nổ

                if (hit.gameObject.CompareTag("Player"))
                {
                    ControllerShop.Instance.LoserGame();
                }

                #endregion
            }

            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale * .02f;
            //
            GameObject particleObject = LeanPool.Spawn(particlePrefab, transform.position, Quaternion.identity);
            ParticleSystem particle = particleObject.GetComponent<ParticleSystem>();
            particle.Play();
            meshRenderer.enabled = false;
            LeanPool.Despawn(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        #region Check khi va chạm vào bullet

        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("LeftLeg") ||
            collision.gameObject.CompareTag("RightLeg"))
        {
            ControllerShop.Instance.LoserGame();
        }

        #endregion
        if (collision.gameObject.CompareTag("Weapon") || collision.gameObject.CompareTag("Ground") ||
            collision.gameObject.CompareTag("WallLeft") || collision.gameObject.CompareTag("WallRight")|| collision.gameObject.CompareTag("Player"))
        {
            GameController.Instance.list_musicBoom.Add(LeanPool.Spawn(GameController.Instance.audioSource, transform.position, Quaternion.identity));
            #region Vùng nổ
            // Kiểm tra va chạm với các đối tượng trong hình cầu
            Collider[] colliders = Physics.OverlapSphere(transform.position, .5f);

            foreach (Collider hit in colliders)
            {
                //lấy ra enemy nằm trong vùng và cho nó nổ

                if (hit.gameObject.CompareTag("Player"))
                {
                    ControllerShop.Instance.LoserGame();
                }

                #endregion
            }

            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale * .02f;
            //
            GameObject particleObject = LeanPool.Spawn(particlePrefab, transform.position, Quaternion.identity);
            ParticleSystem particle = particleObject.GetComponent<ParticleSystem>();
            particle.Play();
            meshRenderer.enabled = false;
            LeanPool.Despawn(gameObject);
        }
    }
}
