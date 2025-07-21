/*
    This script is used to implement the head follow feature of the character in the main menu.
    The character's head will follow the audiences passing by captured by the device's camera.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class HeadFollow : MonoBehaviour
{
    public ARHumanBodyManager arHumanBodyManager;
    // 角色头部骨骼
    public Transform head;
    public int headIndex = 51;
    public float headFollowSpeed = 10f;

    Quaternion initLocalRot;

    void Start(){
        initLocalRot = head.localRotation;
    }
    
    void Update(){
        if (arHumanBodyManager == null || head == null) return;

        ARHumanBody closestBody = null;
        float minDistance = float.MaxValue;
        Vector3 cameraPos = Camera.main.transform.position;
        
        // 检查ARHumanBodyManager的状态
        Debug.Log($"[HeadFollow] pose3DRequested={arHumanBodyManager.pose3DRequested}, pose3DEnabled={arHumanBodyManager.pose3DEnabled}");

        foreach (var body in arHumanBodyManager.trackables)
        {
            if (body == null || body.joints == null || body.joints.Length <= headIndex) continue;

            var headPose = body.joints[headIndex].anchorPose;
            float distance = Vector3.Distance(cameraPos, headPose.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestBody = body;
            }
        }



        if (closestBody != null)
        {
            Debug.Log("Head detected");

            // 计算角度使角色头部看向现实中的观众而非摄像头画面中的观众
            /*
            var headPose = closestBody.joints[headIndex].anchorPose;
            Vector3 targetWorldPos = headPose.position;
            Vector3 direction = targetWorldPos - head.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            Quaternion localTarget = Quaternion.Inverse(head.parent.rotation) * targetRotation;
            head.localRotation = Quaternion.Slerp(head.localRotation, localTarget, Time.deltaTime * headFollowSpeed);
            */
            /*
            var headPose = closestBody.joints[headIndex].anchorPose;
            Quaternion localTarget = Quaternion.Inverse(head.parent.rotation) * headPose.rotation;
            head.localRotation = Quaternion.Slerp(head.localRotation, localTarget, Time.deltaTime * headFollowSpeed);
            */
            // 世界空间里的人的头部位置
            Vector3 targetWorldPos = closestBody.joints[headIndex].anchorPose.position;
            Vector3 direction = targetWorldPos - head.position;
            direction.y = 0; //只在水平面看

            if (direction.sqrMagnitude > 0.01f) // 防止过小的移动
            {
                Quaternion worldLook = Quaternion.LookRotation(direction, Vector3.up);
                Quaternion localLook = Quaternion.Inverse(head.parent.rotation) * worldLook;
                Quaternion finalLocal = initLocalRot * localLook; // 保持初始旋转
                head.localRotation = Quaternion.Slerp(head.localRotation, finalLocal, Time.deltaTime * headFollowSpeed);

                Debug.Log($"Head position: {targetWorldPos}, Direction: {direction}, Local Rotation: {head.localRotation}");
            }
            else
            {
                head.localRotation = initLocalRot; // 如果没有移动，保持初始旋转
            }

            
            
        }
        else
        {
            Debug.Log("No head detected");
        }
    }
}