using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class HandMenuSelector : MonoBehaviour
{
    [Header("AR")]
    public ARHumanBodyManager humanBodyManager;

    [Header("UI")]
    public Canvas uiCanvas;
    public RectTransform hoverBall;      // 悬浮球
    public Button startButton;
    //public Button leaderboardButton;

    [Header("Settings")]
    public float selectionDuration = 3f; // 停留3秒触发

    const int RIGHT_HAND_JOINT_INDEX = 22; // ARKit 默认 “right hand” 在 joints 中的 index

    string currentOption = null;
    float hoverTimer = 0f;

    void Update()
    {
        // 1. 找到第一个被 tracking 的 ARHumanBody
        ARHumanBody body = null;
        foreach (var b in humanBodyManager.trackables)
        {
            if (b.trackingState == TrackingState.Tracking)
            {
                body = b;
                break;
            }
        }
        if (body == null)
        {
            HideBall();
            return;
        }

        // 2. 从 body.joints 拿到所有关节
        var joints = body.joints; // XRHumanBodyJoint[]
        if (joints == null || joints.Length == 0)
        {
            HideBall();
            return;
        }
        XRHumanBodyJoint handJoint = default;
        bool found = false;
        for (int i = 0; i < joints.Length; i++)
        {
            if (joints[i].index == RIGHT_HAND_JOINT_INDEX)
            {
                handJoint = joints[i];
                found = true;
                break;
            }
        }
        if (!found || !handJoint.tracked)
        {
            HideBall();
            return;
        }

        // 3. 世界坐标→屏幕坐标→Canvas 本地坐标
        Vector3 worldPos = handJoint.anchorPose.position;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        RectTransform canvasRect = uiCanvas.transform as RectTransform;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect, screenPos, Camera.main, out Vector2 localPos);
        hoverBall.anchoredPosition = localPos;
        hoverBall.gameObject.SetActive(true);

        // 4. 检测悬浮球是否在按钮区域
        if (RectTransformUtility.RectangleContainsScreenPoint(
                startButton.GetComponent<RectTransform>(), screenPos, Camera.main))
        {
            UpdateHover("start");
        }
        // else if (RectTransformUtility.RectangleContainsScreenPoint(
        //         leaderboardButton.GetComponent<RectTransform>(), screenPos, Camera.main))
        // {
        //     UpdateHover("leaderboard");
        // }
        else
        {
            ResetHover();
        }
    }

    void UpdateHover(string option)
    {
        if (currentOption != option)
        {
            currentOption = option;
            hoverTimer = 0f;
        }
        else
        {
            hoverTimer += Time.deltaTime;
            if (hoverTimer >= selectionDuration)
            {
                // 触发对应按钮的 onClick
                if (option == "start") startButton.onClick.Invoke();
                //else if (option == "leaderboard") leaderboardButton.onClick.Invoke();
                ResetHover();
            }
        }
    }

    void ResetHover()
    {
        currentOption = null;
        hoverTimer = 0f;
    }

    void HideBall()
    {
        hoverBall.gameObject.SetActive(false);
        ResetHover();
    }
}