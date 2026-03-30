using UnityEngine;

public class BusPlatform : MonoBehaviour
{
    // 승객이 버스 승차 구역(트리거)에 들어왔을 때 실행
    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트의 태그가 "Player"인지 확인
        if (other.CompareTag("Player"))
        {
            // 승객의 부모를 이 스크립트가 붙은 오브젝트(버스)로 설정
            other.transform.SetParent(transform);
            Debug.Log("승객이 버스에 탑승했습니다. (부모-자식 연결됨)");
        }
    }

    // 승객이 버스에서 내렸을 때(트리거를 벗어났을 때) 실행
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 부모 관계를 해제하여 승객이 다시 독립적으로 움직이게 함
            other.transform.SetParent(null);
            Debug.Log("승객이 버스에서 하차했습니다. (부모-자식 해제됨)");
        }
    }
}