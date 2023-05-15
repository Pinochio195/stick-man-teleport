using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateUI_Glow : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private RectTransform rectTransform;

    void Update()
    {
        rectTransform.Rotate(0, 0, 190f * Time.deltaTime);
    }
    
}
