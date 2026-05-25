using UnityEngine;

public class CardReader : MonoBehaviour
{
    [Header("태그 상태")]
    public bool isTagged = false;

    [Header("VR 없이 테스트용")]
    public bool inspectorTestTag = false;

    public AudioSource audioSource;
    public AudioClip boardingSound;
    public float delayTime = 2.0f;

    private bool isProcessing = false;

    private void Update()
    {
        // VR 기기 없이 테스트할 때 Inspector에서 체크하면 카드 태그 처리
        if (inspectorTestTag == true && isTagged == false)
        {
            TestCardTag();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isTagged || isProcessing) return;

        if (other.CompareTag("BusCard"))
        {
            CardTag();
        }
    }

    private void CardTag()
    {
        isProcessing = true;
        isTagged = true;

        Debug.Log(gameObject.name + " 카드 태그 성공");

        if (audioSource != null && boardingSound != null)
        {
            audioSource.clip = boardingSound;
            audioSource.Play();
            Debug.Log(gameObject.name + " 오디오 재생");
        }
        else
        {
            Debug.LogWarning(gameObject.name + " AudioSource 또는 boardingSound가 비어 있음");
        }

        Invoke(nameof(ResetReader), delayTime);
    }

    private void TestCardTag()
    {
        Debug.Log(gameObject.name + " Inspector 테스트 태그 실행");
        CardTag();
    }

    private void ResetReader()
    {
        isProcessing = false;
        Debug.Log(gameObject.name + " 단말기 초기화 완료");
    }
}