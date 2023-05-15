using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Camera2D : MonoBehaviour
{
    public List<Sprite> backGround;
    public SpriteRenderer bg;

    void Start()
    {
        float Al = (float)(9f / 16f);
        float Bt = (float)Screen.width / (float)Screen.height;
        if (Bt < Al)
        {
            Camera.main.orthographicSize = (float)(Al * 9.6f) / (float)Bt;
        }

        if (GameController.Instance.isCheckLoadScene && !SceneManager.GetActiveScene().name.Equals("ARMS"))
        {
            GameController.Instance.isCheckLoadScene = false;
            // Lấy kí tự cuối cùng
            string sceneName = SceneManager.GetActiveScene().name;
            string numberString = sceneName.Substring(sceneName.LastIndexOf("_") + 1);
            Debug.Log(sceneName);
            int lastNumber = int.Parse(numberString);
            int index = (lastNumber - 1) / 5;
            bg.sprite = backGround[index];
        }
    }
}