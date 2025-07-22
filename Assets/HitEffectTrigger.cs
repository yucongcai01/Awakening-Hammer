using UnityEngine;

public class HitEffectTrigger : MonoBehaviour
{
    public GameObject hitEffectPrefab; // 拖拽你的特效预制体

    void OnTriggerEnter(Collider other)
    {
        // 判断是否是手部Trigger
        if (other.CompareTag("LeftHandTrigger") || other.CompareTag("RightHandTrigger"))
        {
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        }
    }
}