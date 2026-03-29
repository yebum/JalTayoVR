using UnityEngine;

public class CardReader : MonoBehaviour
{
    public bool isTagged = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isTagged) return;

        if (other.CompareTag("BusCard"))
        {
            isTagged = true;
            Debug.Log("카드 태그 성공");

            // 여기서 소리, UI, 다음 단계 진행 연결 가능
        }
    }
}