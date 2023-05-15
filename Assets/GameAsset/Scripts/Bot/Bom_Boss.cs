using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class Bom_Boss : MonoBehaviour
{
    [SerializeField] private GameObject particlePrefab;
    private SkinnedMeshRenderer meshRenderer;
    private void Awake()
    {
        meshRenderer = transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
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
            collision.gameObject.CompareTag("WallLeft") || collision.gameObject.CompareTag("WallRight") ||
            collision.gameObject.CompareTag("Player"))
        {
            GameController.Instance.list_musicBoom.Add(LeanPool.Spawn(GameController.Instance.audioSource, transform.position, Quaternion.identity));
            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale * .02f;
            
            GameObject particleObject = LeanPool.Spawn(particlePrefab, transform.position, Quaternion.identity);
            ParticleSystem particle = particleObject.transform.GetChild(2).GetComponent<ParticleSystem>();
            particle.Play();
            meshRenderer.enabled = false;
            LeanPool.Despawn(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        #region Check khi va chạm vào bullet

        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("LeftLeg") ||
            other.gameObject.CompareTag("RightLeg"))
        {
            ControllerShop.Instance.LoserGame();
        }

        #endregion
        if (other.gameObject.CompareTag("Weapon") || other.gameObject.CompareTag("Ground") ||
            other.gameObject.CompareTag("WallLeft") || other.gameObject.CompareTag("WallRight") ||
            other.gameObject.CompareTag("Player"))
        {
            GameController.Instance.list_musicBoom.Add(LeanPool.Spawn(GameController.Instance.audioSource, transform.position, Quaternion.identity));
            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale * .02f;
            
            GameObject particleObject = LeanPool.Spawn(particlePrefab, transform.position, Quaternion.identity);
            ParticleSystem particle = particleObject.transform.GetChild(2).GetComponent<ParticleSystem>();
            particle.Play();
            meshRenderer.enabled = false;
            LeanPool.Despawn(gameObject);
        }
    }
}