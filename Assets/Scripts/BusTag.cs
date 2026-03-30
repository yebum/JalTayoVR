using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusTag : MonoBehaviour
{
    // Start is called before the first frame update
    
    public AudioSource audioSource;   // 사운드를 재생할 오디오 소스
    public AudioClip boardingSound;   // "승차입니다" 음원 파일

    private bool isProcessing = false;
    public float delayTime = 2.0f;

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BusCard") && !isProcessing)
        {
            
            if (audioSource != null && boardingSound != null)
            {
                audioSource.PlayOneShot(boardingSound);
            }

            
            ProcessBoarding();
        }
    }

    private void ProcessBoarding()
    {
        isProcessing = true;

       
        Debug.Log("승차 신호 수신 완료");

        // UI 업데이트, 버스 문 닫기 등의 로직을 여기에 추가

        // 일정 시간(delayTime) 후에 다시 카드를 찍을 수 있도록 상태를 초기화
        Invoke("ResetReader", delayTime);
    }

    private void ResetReader()
    {
        isProcessing = false;
        Debug.Log("단말기 초기화 완료. 다시 태그 가능합니다.");
    }
}
