using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    public Image LoadingBarFill;
    void Start()
    {
        StartCoroutine(LoadSceneTestFirstRun(PlayerPrefs.GetInt("Level_", 1)));
    }
    private IEnumerator LoadSceneTestFirstRun(int load)
    {
        UnityEngine.AsyncOperation operation = SceneManager.LoadSceneAsync(load);
        operation.allowSceneActivation = false;
        float time = 0;
        
        while (time <=6)
        {
            time += Time.deltaTime;
            float progressValue = Mathf.Clamp01((time / 6f));
            LoadingBarFill.fillAmount = progressValue;
            yield return null;
        }
        operation.allowSceneActivation = true;
    }
}
