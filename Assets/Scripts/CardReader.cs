using UnityEngine;

public class CardReader : MonoBehaviour
{
    public bool isTagged = false;
    public AudioSource audioSource;
    public AudioClip boardingSound;
    public float delayTime = 2f;

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
                audioSource.PlayOneShot(boardingSound);
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