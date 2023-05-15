using UnityEngine;
using System.Collections.Generic;

public class CloudGenerator : MonoBehaviour
{
    public GameObject parentGameObject; // GameObject để gán vào con của đối tượng được chỉ định
    public List<Sprite> sprites; // danh sách sprite được nạp vào danh sách
    [Tooltip("Số lượng các mây trên màn hình")]public int SoLuongMay;
    private void Start()
    {
        for (int i = 0; i < SoLuongMay; i++)
        {
            // Khởi tạo đối tượng và add component SpriteRenderer
            GameObject spriteObject = new GameObject("SpriteObject");
            SpriteRenderer spriteRenderer = spriteObject.AddComponent<SpriteRenderer>();
            spriteObject.transform.parent = parentGameObject.transform;

            // Random chọn sprite từ danh sách
            int randomIndex = Random.Range(0, sprites.Count);
            spriteRenderer.sprite = sprites[randomIndex];
            float bottomY = Camera.main.orthographicSize + Screen.height / 2;
            Vector3 bottomWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(0, bottomY, 0));
            // Set vị trí của đối tượng để nằm trong camera
            Vector3 position = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width),
                0, Camera.main.nearClipPlane));
            spriteObject.transform.position = position;
            // Gán đối tượng được tạo vào trong parent GameObject
            spriteObject.transform.localPosition = new Vector3(spriteObject.transform.localPosition.x,bottomWorldPos.y-15+(i%3==0?i:(i%5==0?i+5:(i%8==0?i+5:
                (i%12==0?i+5:(i+10))))),-5);
        }
    }
}