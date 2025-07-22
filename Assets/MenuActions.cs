using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuActions : MonoBehaviour
{
    // 按钮 OnClick 绑定
    public void OnStartGame()
    {
        // TODO: 把“GameScene”换成你的游戏场景名
        //SceneManager.LoadScene("GameScene");
        Debug.Log("开始游戏");
    }

    //
    //public void OnShowLeaderboard()
    //{
        // TODO: 弹出排行榜 UI 或跳转
    //    Debug.Log("打开排行榜");
    //    // e.g. LeaderboardUI.SetActive(true);
    //}
}