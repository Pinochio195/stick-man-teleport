using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class BomBossLevel10 : MonoBehaviour
{
    [SerializeField] private GameObject particlePrefab;
    private SkinnedMeshRenderer meshRenderer;
    private Transform Effect_TransForm;

    private void Awake()
    {
        Effect_TransForm = transform.GetChild(1).GetComponent<Transform>();
    }
    private void OnEnable()
    {
        Effect_TransForm.rotation = Quaternion.Euler(0, 0, 0);
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
            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale * .02f;
            ContactPoint contact = collision.contacts[0];
            Vector3 point = contact.point;
            // Phát ra particle
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
            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale * .02f;

            // Phát ra particle
            GameObject particleObject = LeanPool.Spawn(particlePrefab, transform.position, Quaternion.identity);
            ParticleSystem particle = particleObject.GetComponent<ParticleSystem>();
            particle.Play();
            meshRenderer.enabled = false;
            LeanPool.Despawn(gameObject);
        }
    }
}
