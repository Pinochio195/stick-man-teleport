using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableLoadScene : MonoBehaviour
{
    // Start is called before the first frame update
    void DisableLoadScenePlay()
    {
        GameController.Instance.LoadingBarFill.transform.parent.gameObject.SetActive(false);
    }
}
