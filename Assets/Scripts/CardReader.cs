using UnityEngine;

public class CardReader : MonoBehaviour
{
    public enum ReaderType
    {
        Boarding,   // 승차 단말기
        Exit        // 하차 단말기
    }

    [Header("단말기 타입")]
    public ReaderType readerType = ReaderType.Boarding;

    [Header("태그 상태")]
    public bool isTagged = false;

    [Header("VR 없이 테스트용")]
    public bool inspectorTestTag = false;

    [Header("하차 처리용")]
    public testscript TS;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip boardingSound;
    public float delayTime = 2.0f;

    private bool isProcessing = false;

    private void Update()
    {
        // VR 기기 없이 테스트할 때 Inspector에서 체크하면 카드 태그 처리
        if (inspectorTestTag == true && isTagged == false)
        {
            inspectorTestTag = false;
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

        Debug.Log(gameObject.name + " 카드 태그 성공 / 타입: " + readerType);

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

        // 핵심 추가 기능:
        // 이 카드리더가 하차 단말기일 때만 플레이어를 버스 자식에서 해제한다.
        if (readerType == ReaderType.Exit)
        {
            if (TS != null)
            {
                TS.ExitBus();
                Debug.Log(gameObject.name + " 하차 처리 완료: TS.ExitBus() 실행");
            }
            else
            {
                Debug.LogWarning(gameObject.name + " 하차 처리 실패: TS가 연결되지 않음");
            }
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

        // 중요:
        // 여기서 isTagged = false; 를 하지 않는다.
        // BusRoute.cs가 승차 카드리더의 isTagged 값을 보고 출발하기 때문이다.
        Debug.Log(gameObject.name + " 단말기 초기화 완료");
    }
}