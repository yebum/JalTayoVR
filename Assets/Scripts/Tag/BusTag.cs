using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusTag : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource audioSource;
    public AudioClip boardingSound;

    private bool isTag = false;
    public float delayTime = 2.0f;

    private void OnTriggerEnter(Collider other)
    {
        // 1. 들어온 오브젝트의 태그가 "BusCard"인지 확인하고, 현재 처리 중이 아닌지 확인
        if (other.CompareTag("BusCard") && !isTag)
        {
            // 2. 사운드 재생
            if (audioSource != null && boardingSound != null)
            {
                audioSource.PlayOneShot(boardingSound);
            }

            // 3. 승차 신호 처리
            ProcessBoarding();
        }
    }

    private void ProcessBoarding()
    {
        isTag = true;

        // 콘솔 창에서 확인용 로그
        Debug.Log("승차 신호 수신 완료! 승차 처리 로직이 이곳에 들어갑니다.");

        // TODO: 요금 차감, UI 업데이트, 버스 문 닫기 등의 로직을 여기 추가

        Invoke("ResetReader", delayTime);
    }

    private void ResetReader()
    {
        isTag = false;
        Debug.Log("단말기 초기화 완료. 다시 태그 가능합니다.");
    }
}
