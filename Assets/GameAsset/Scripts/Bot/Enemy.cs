using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //public static List<GameObject> enemyList = new List<GameObject>();

    private void Start()
    {
        // Thêm đối tượng này vào danh sách enemyList
        GameController.Instance.listEnemy.Add(gameObject);
    }

    private void OnDestroy()
    {
        // Xóa đối tượng này khỏi danh sách enemyList
        GameController.Instance.listEnemy.Remove(gameObject);
    }
}

