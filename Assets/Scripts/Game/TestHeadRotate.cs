using UnityEngine;

public class HeadTestRotate : MonoBehaviour
{
    public Transform head;
    public float angleRange = 45f;   // 最大左右角度
    public float speed = 1f;         // 摇头速度

    Quaternion initRot;

    void Start()
    {
        if (head != null)
            initRot = head.localRotation;
    }

    void Update()
    {
        if (head == null) return;

        float angle = Mathf.Sin(Time.time * speed) * angleRange; // -angleRange~+angleRange
        // 在原始旋转的基础上左右摇头
        head.localRotation = initRot * Quaternion.Euler(0, angle, 0);
    }
}
