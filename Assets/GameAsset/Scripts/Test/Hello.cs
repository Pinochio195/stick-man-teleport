using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hello : MonoBehaviour
{
    public GameObject objectToSpawn; // Prefab của object a
    public float spawnOffset = 0.1f; // Khoảng cách giữa object a và object gốc

    private void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Sprite sprite = spriteRenderer.sprite;
            if (sprite != null)
            {
                Vector2 spriteSize = sprite.bounds.size;
                Vector3 spawnPosition = transform.position + new Vector3(spriteSize.x * 0.5f, spriteSize.y * 0.5f, 0f) + (Vector3.right * spawnOffset);

                // Tạo object a trong vùng hình ảnh của object gốc
                GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
                // Gắn spawnedObject vào parent của object gốc (nếu cần)
                spawnedObject.transform.parent = transform;
            }
        }
    }

}
