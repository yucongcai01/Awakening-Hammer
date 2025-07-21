/*
    This script is used to implement the head follow feature of the character in the main menu.
    The character's head will follow the audiences passing by captured by the device's camera.
*/
/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class HeadFollow : MonoBehaviour
{
    public Transform XROrigin;
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
        if (arHumanBodyManager == null || head == null || XROrigin == null) return;

        ARHumanBody closestBody = null;
        float minDistance = float.MaxValue;
        Vector3 cameraPos = Camera.main.transform.position;
        
        // 检查ARHumanBodyManager的状态
        Debug.Log($"[HeadFollow] pose3DRequested={arHumanBodyManager.pose3DRequested}, pose3DEnabled={arHumanBodyManager.pose3DEnabled}");

        foreach (var body in arHumanBodyManager.trackables)
        {
            if (body == null || body.joints == null || body.joints.Length <= headIndex) continue;

            var headPose = body.joints[headIndex].anchorPose;
            Vector3 worldJointPos = XROrigin.TransformPoint(headPose.position);

            float distance = Vector3.Distance(cameraPos, worldJointPos);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestBody = body;
            }
        }



        if (closestBody != null)
        {
            var localPose = closestBody.joints[headIndex].anchorPose;
            Vector3 worldJointPos = XROrigin.TransformPoint(localPose.position);
            Debug.Log("Head detected");
            /*
            var headPose = closestBody.joints[headIndex].anchorPose;
            Quaternion localTarget = Quaternion.Inverse(head.parent.rotation) * headPose.rotation;
            head.localRotation = Quaternion.Slerp(head.localRotation, localTarget, Time.deltaTime * headFollowSpeed);
            */
            // 世界空间里的人的头部位置
            /*
            Vector3 direction = head.position - worldJointPos;
            direction.y = 0;

            if (direction.sqrMagnitude > 0.001f) // 防止过小的移动
            {
                Quaternion worldLook = Quaternion.LookRotation(direction, Vector3.up);
                Quaternion localLook = Quaternion.Inverse(head.parent.rotation) * worldLook;
                Quaternion finalLocal = initLocalRot * localLook; // 保持初始旋转
                //head.localRotation = Quaternion.Slerp(head.localRotation, finalLocal, Time.deltaTime * headFollowSpeed);
                head.localRotation = finalLocal; // 直接设置为计算后的旋转

                Debug.Log($"Head position: {worldJointPos}, Direction: {direction}, Local Rotation: {head.localRotation}");
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
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class HeadFollow : MonoBehaviour
{
    public Transform XROrigin;
    public ARHumanBodyManager arHumanBodyManager;

    // The transform of the character's head bone
    public Transform head;
    // The index for the head joint in ARKit/ARCore
    // Note: 51 is a common index but can vary. Double-check documentation if needed.
    public int headIndex = 51;
    public float headFollowSpeed = 5f; // A slightly slower speed feels more natural

    private Quaternion initialWorldRotation;

    void Start()
    {
        if (head == null) {
            Debug.LogError("Head transform is not assigned!");
            return;
        }
        // Store the initial world rotation to return to when no one is detected
        initialWorldRotation = head.rotation;
    }
    
    // Use LateUpdate to run after the Animator updates.
    void LateUpdate()
    {
        if (arHumanBodyManager == null || head == null || XROrigin == null) return;

        ARHumanBody closestBody = FindClosestHumanBody();

        if (closestBody != null)
        {
            Debug.Log("Human body detected, head will follow.");
            // A human body is detected, so make the head follow.
            var localPose = closestBody.joints[headIndex].anchorPose;
            Vector3 worldJointPos = XROrigin.TransformPoint(localPose.position);

            // --- FIX 1: Corrected direction vector ---
            // Calculate the direction from the character's head TO the tracked person.
            Vector3 directionToTarget = worldJointPos - head.position;

            // Optional: Keep the head level by ignoring vertical differences.
            directionToTarget.y = 0;

            // If the direction is valid (not a zero vector)
            if (directionToTarget.sqrMagnitude > 0.001f)
            {
                // Create a rotation that looks in the calculated direction.
                Quaternion targetWorldRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);

                // --- FIX 2: Smoother rotation in world space ---
                // Slerp (spherically interpolate) the head's current rotation towards the target rotation.
                // This is simpler and less error-prone than calculating local rotations.
                head.rotation = Quaternion.Slerp(head.rotation, targetWorldRotation, Time.deltaTime * headFollowSpeed);
            }
            Debug.Log($"Head position: {worldJointPos}, Direction: {directionToTarget}, Head Rotation: {head.rotation}");
        }
        else
        {
            // No human body is detected, so smoothly return to the initial rotation.
            head.rotation = Quaternion.Slerp(head.rotation, initialWorldRotation, Time.deltaTime * headFollowSpeed);
        }
    }

    private ARHumanBody FindClosestHumanBody()
    {
        ARHumanBody closestBody = null;
        float minDistance = float.MaxValue;
        Vector3 cameraPos = Camera.main.transform.position;

        foreach (var body in arHumanBodyManager.trackables)
        {
            if (body == null || body.joints == null || body.joints.Length <= headIndex) continue;

            // Get the position of the tracked head in world space to calculate distance.
            var headPose = body.joints[headIndex].anchorPose;
            Vector3 worldJointPos = XROrigin.TransformPoint(headPose.position);

            float distance = Vector3.Distance(cameraPos, worldJointPos);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestBody = body;
            }
        }
        return closestBody;
    }
}