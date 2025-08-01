using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    
    [Header("UI 设置")]
    public Text scoreText; // 拖拽UI中的Text组件来显示得分
    
    private int currentScore = 0;
    
    void Awake()
    {
        // 单例模式，确保只有一个得分管理器
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        UpdateScoreDisplay();
    }
    
    // 增加得分
    public void AddScore(int points)
    {
        currentScore += points;
        UpdateScoreDisplay();
        Debug.Log($"得分增加 {points} 分，当前总分: {currentScore}");
    }
    
    // 重置得分
    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreDisplay();
        Debug.Log("得分已重置");
    }
    
    // 获取当前得分
    public int GetCurrentScore()
    {
        return currentScore;
    }
    
    // 更新UI显示
    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = currentScore.ToString();
        }
    }
}
