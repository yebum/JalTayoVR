using UnityEngine;

public class CardReader : MonoBehaviour
{
    public bool isTagged = false;

    public AudioSource audioSource;   // 단말기에 붙은 AudioSource
    public AudioClip boardingSound;   // mp3 파일
    public float delayTime = 2.0f;

    private bool isProcessing = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isTagged || isProcessing) return;

        if (other.CompareTag("BusCard"))
        {
            isProcessing = true;
            isTagged = true;

            Debug.Log("카드 태그 성공");

            if (audioSource != null && boardingSound != null)
            {
                audioSource.clip = boardingSound;  // mp3 파일 넣기
                audioSource.Play();                // 재생
                Debug.Log("오디오 재생");
            }
            else
            {
                Debug.LogWarning("AudioSource 또는 boardingSound가 비어 있음");
            }

            Invoke(nameof(ResetReader), delayTime);
        }
    }

    private void ResetReader()
    {
        isProcessing = false;
        Debug.Log("단말기 초기화 완료"); 
    }
}