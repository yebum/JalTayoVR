using UnityEngine;

public class PlayerOnBus : MonoBehaviour
{
    // 플레이어가 버스 탑승 구역(Trigger)에 들어왔을 때
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어를 버스의 자식으로 설정하여 함께 이동하게 함
            other.transform.parent = transform;
            Debug.Log("버스의 자식으로 이동 완료");
        }
    }

    // 플레이어가 버스 탑승 구역에서 나갔을 때
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 부모 관계를 해제(null)하여 플레이어가 다시 독립적으로 움직이게 함
            other.transform.parent = null;
            Debug.Log("버스 자식 해제");
        }
    }
}