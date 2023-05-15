using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrameGame : MonoBehaviour
{
    public Text frameRateText;

    private float deltaTime;

    private void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        frameRateText.text = string.Format("{0:0.} fps", fps);
    }
}
