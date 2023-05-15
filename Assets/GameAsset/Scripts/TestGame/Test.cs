using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private MeshFilter meshFilter;

    void Start () {
        meshFilter = GetComponent<MeshFilter>();
    }
    
    void Update () {
        if (Input.GetKeyDown(KeyCode.K)) {
            Destroy(meshFilter.mesh);
        }
    }
}
