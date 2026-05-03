using UnityEngine;

public class BusTriggerDetector : MonoBehaviour
{
    public BusMessageController messageController; // 메세지 스크립트 연결하기
    public string messageText; // 텍스트 인스펙터 창에서 적기 (수정가능)

    private void OnTriggerEnter(Collider other)
    {
        // 부딪힌 물체의 태그가 "Player" 혹은 "Bus"라면 실행
        if (other.CompareTag("Player") || other.CompareTag("Bus"))
        {
            messageController.ShowMessage(messageText);
        }
    }
}