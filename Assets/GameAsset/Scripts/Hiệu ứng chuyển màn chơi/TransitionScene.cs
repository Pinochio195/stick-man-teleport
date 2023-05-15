using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionScene : MonoBehaviour
{
    //đưa hàm sau vào trong animation để chuyển cảnh luôn
    public void LoadSceneWithAnimation()
    {
        StartCoroutine(LoadSceneAndDoSomething("Level_" + GameController.Instance.level));
    }
    //cho phép người chơi nhấn vào màn hình
    public void ActiveClick()
    {
        GameController.Instance.isWinGame = false;
    }
    IEnumerator LoadSceneAndDoSomething(string sceneName)
    {
        bool fadeOutCompleted = false;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        // Chờ đợi việc load scene hoàn thành
        while (!asyncLoad.isDone)
        {
            if (!fadeOutCompleted && asyncLoad.progress >= 0.9f) //Kiểm tra xem animation Fade_Out đã hoàn thành chưa
            {
                fadeOutCompleted = true;
                #region Mở key lên

                if (!GameController.Instance.GO_ParentKey.activeSelf)
                {
                    GameController.Instance.GO_ParentKey.SetActive(true);
                }

                #endregion

                #region Gán Level cho text

                ControllerShop.Instance.LevelGame.text = "Level " + GameController.Instance.level;

                #endregion
                GameController.Instance.transitionScene.SetTrigger("Fade_In"); //Chạy animation Fade_In khi load scene hoàn thành và animation Fade_Out đã hoàn thành
            }
            yield return null;
        }

        // Thực hiện code của bạn ở đây
    }
}
