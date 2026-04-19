using UnityEngine;

public class VRFollowUI_Mission : MonoBehaviour
{
    public Transform targetCamera;
    public float distance = 2.0f;
    public float followSpeed = 2.0f;
    public float sideOffset = 1.5f; // 오른쪽으로 밀어낼 거리

    void OnEnable() // 미션창이 활성화(SetActive(true))되는 순간 실행
    {
        if (targetCamera != null)
        {
            // 켜지는 즉시 오른쪽 위치로 강제 이동
            Vector3 targetPos = targetCamera.position + (targetCamera.forward * distance) + (targetCamera.right * sideOffset);
            transform.position = targetPos;
            transform.rotation = targetCamera.rotation;
        }
    }

    void Update()
    {
        if (targetCamera == null) return;

        // 매 프레임 오른쪽 간격 유지하며 따라오기
        Vector3 targetPos = targetCamera.position + (targetCamera.forward * distance) + (targetCamera.right * sideOffset);
        transform.position = Vector3.Slerp(transform.position, targetPos, Time.deltaTime * followSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetCamera.rotation, Time.deltaTime * followSpeed);
    }
}