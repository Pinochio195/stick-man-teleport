using UnityEngine;

public class ScaleToCamera : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Camera mainCamera;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
        ScaleSprite();
    }

    void ScaleSprite()
    {
        float spriteHeight = spriteRenderer.bounds.size.y;
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float scale = cameraHeight / spriteHeight;

        transform.localScale = new Vector3(scale+.35f, scale+.35f, 1f);
    }
}