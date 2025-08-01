using UnityEngine;
using UnityEngine.UI;

public class TimerProgressBar : MonoBehaviour
{
    [Header("进度条设置")]
    public Slider progressBar; // 拖拽进度条Slider组件
    
    [Header("时间设置")]
    public float maxTime = 18f; // 最大时间18秒
    public bool autoStart = true; // 是否自动开始
    
    private float currentTime = 0f;
    private bool isRunning = false;
    
    void Start()
    {
        // 初始化进度条
        if (progressBar != null)
        {
            progressBar.minValue = 0f;
            progressBar.maxValue = maxTime;
            progressBar.value = 0f;
        }
        
        // 更新显示
        UpdateDisplay();
        
        // 自动开始
        if (autoStart)
        {
            StartTimer();
        }
    }
    
    void Update()
    {
        if (isRunning)
        {
            // 增加时间
            currentTime += Time.deltaTime;
            
            // 检查是否达到最大时间
            if (currentTime >= maxTime)
            {
                currentTime = 0f; // 重置为0，开始新的循环
                OnTimerComplete();
            }
            
            // 更新显示
            UpdateDisplay();
        }
    }
    
    // 更新进度条显示
    private void UpdateDisplay()
    {
        // 更新进度条
        if (progressBar != null)
        {
            progressBar.value = currentTime;
        }
    }
    
    // 开始计时器
    public void StartTimer()
    {
        isRunning = true;
        Debug.Log("计时器开始");
    }
    
    // 停止计时器
    public void StopTimer()
    {
        isRunning = false;
        Debug.Log("计时器停止");
    }
    
    // 重置计时器
    public void ResetTimer()
    {
        currentTime = 0f;
        UpdateDisplay();
        Debug.Log("计时器重置");
    }
    
    // 暂停计时器
    public void PauseTimer()
    {
        isRunning = false;
        Debug.Log("计时器暂停");
    }
    
    // 恢复计时器
    public void ResumeTimer()
    {
        isRunning = true;
        Debug.Log("计时器恢复");
    }
    
    // 设置当前时间
    public void SetTime(float time)
    {
        currentTime = Mathf.Clamp(time, 0f, maxTime);
        UpdateDisplay();
    }
    
    // 获取当前时间
    public float GetCurrentTime()
    {
        return currentTime;
    }
    
    // 获取剩余时间
    public float GetRemainingTime()
    {
        return maxTime - currentTime;
    }
    
    // 计时完成时的回调（可以在这里添加游戏逻辑）
    private void OnTimerComplete()
    {
        Debug.Log("18秒计时完成，开始新的循环");
        // 在这里可以添加计时完成时的逻辑
        // 比如：游戏结束、关卡切换等
    }
    
    // 获取进度百分比 (0-1)
    public float GetProgressPercentage()
    {
        return currentTime / maxTime;
    }
} 