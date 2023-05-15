using System;
using System.Collections;
using Lean.Pool;
using UnityEngine;

public class BulletForEnemyFireLooping : MonoBehaviour
{
    [SerializeField] private GameObject particlePrefab;
    private Rigidbody rigidbody;
    private Transform Effect_TransForm;
    private SkinnedMeshRenderer meshRenderer;

    private void Awake()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
        meshRenderer = transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
        Effect_TransForm = transform.GetChild(1).GetComponent<Transform>();
    }


    private void OnEnable()
    {
        meshRenderer.enabled = true;
        Effect_TransForm.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void FixedUpdate()
    {
        rigidbody.velocity = -transform.forward * 250 * Time.deltaTime;
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
            #region Cho về timeScale như ban đầu

            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale * .02f;

            #endregion

            #region Tắt mũi tên đi nếu còn hiển thị

            if (GameController.Instance.Arrow.activeSelf)
            {
                GameController.Instance.Arrow.SetActive(false);
            }

            #endregion

            // Phát ra particle
            GameObject particleObject = LeanPool.Spawn(particlePrefab, transform.position, Quaternion.identity);
            ParticleSystem particle = particleObject.GetComponent<ParticleSystem>();
            GameController.Instance.list_musicBoom.Add(LeanPool.Spawn(GameController.Instance.audioSource, transform.position, Quaternion.identity));
            particle.Play();
            meshRenderer.enabled = false;
            LeanPool.Despawn(gameObject);
        }
    }


    IEnumerator delaymusic(GameObject A)
    {
        yield return new WaitForSeconds(.5f);
        LeanPool.Despawn(A);
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
            collision.gameObject.CompareTag("WallLeft") || collision.gameObject.CompareTag("WallRight") ||
            collision.gameObject.CompareTag("Player"))
        {
            GameController.Instance.list_musicBoom.Add(LeanPool.Spawn(GameController.Instance.audioSource, transform.position, Quaternion.identity));
            
            #region Cho về timeScale như ban đầu

            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale * .02f;

            #endregion

            #region Tắt mũi tên đi nếu còn hiển thị

            if (GameController.Instance.Arrow.activeSelf)
            {
                GameController.Instance.Arrow.SetActive(false);
            }

            #endregion

            // Phát ra particle
            GameObject particleObject = LeanPool.Spawn(particlePrefab, transform.position, Quaternion.identity);
            ParticleSystem particle = particleObject.GetComponent<ParticleSystem>();
            particle.Play();
            meshRenderer.enabled = false;
            LeanPool.Despawn(gameObject);
        }
    }
}