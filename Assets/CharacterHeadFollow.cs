using UnityEngine;

public class CharacterHeadFollow : MonoBehaviour
{
    [Tooltip("角色头部骨骼")]
    public Transform headBone;
    [Tooltip("朝向平滑速度")]
    public float turnSpeed = 5f;

    Transform cam;

    void Start()
    {
        cam = Camera.main.transform;
    }

    void Update()
    {
        if (headBone == null || cam == null) return;

        // 计算从头骨骼到摄像机的方向
        Vector3 dir = cam.position - headBone.position;
        if (dir.sqrMagnitude < 0.001f) return;

        // 只在水平面上转头（可根据需求移除 y 轴锁定）
        dir.y = 0;

        Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
        headBone.rotation = Quaternion.Slerp(headBone.rotation, targetRot, Time.deltaTime * turnSpeed);
    }
}