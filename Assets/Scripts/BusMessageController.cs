using UnityEngine;
using TMPro; // 텍스트연결하려면 있어야 함
using System.Collections; // 시간 조절하려면 필요함 (코루틴)

public class BusMessageController : MonoBehaviour
{
    public TextMeshProUGUI infoText; // 텍스트 연결

    // 메시지를 띄우는 신호를 받는 함수
    public void ShowMessage(string message)
    {
        StopAllCoroutines(); // 실행 중인 효과가 있다면 끄고 새로 시작
        StartCoroutine(FadeRoutine(message)); // 페이드인아웃 효과
    }

    IEnumerator FadeRoutine(string message)
    {
        infoText.text = message; // 글자 내용 바꾸기

        // 1. 나타나기 (투명도 0 -> 1)
        for (float f = 0; f <= 1; f += Time.deltaTime)
        {
            infoText.alpha = f;
            yield return null;
        }

        // 2. 3초간 기다리기
        yield return new WaitForSeconds(3f);

        // 3. 사라지기 (투명도 1 -> 0)
        for (float f = 1; f >= 0; f -= Time.deltaTime)
        {
            infoText.alpha = f;
            yield return null;
        }
    }
}
