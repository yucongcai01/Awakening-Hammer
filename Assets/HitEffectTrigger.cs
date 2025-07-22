using UnityEngine;

public class HitEffectTrigger : MonoBehaviour
{
    public GameObject hitEffectPrefab; // 拖拽你的特效预制体

    void OnTriggerEnter(Collider other)
    {
        // 只有当自己是关节，碰到的是手，才触发
        if (gameObject.CompareTag("TargetJointTrigger") &&
            (other.CompareTag("LeftHandTrigger") || other.CompareTag("RightHandTrigger")))
        {
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        }
    }
}