using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    // 튜토리얼 Canvas 전체를 드래그해서 넣을 변수
    public GameObject tutorialCanvas;

    // 버튼에 연결할 함수 (Public이어야 함)
    public void CloseTutorial()
    {
        // Canvas 객체를 비활성화해서 화면에서 숨김
        tutorialCanvas.SetActive(false);

        // (선택) 닫기 사운드 재생이나 다른 로직 추가 가능
        Debug.Log("Tutorial Closed!");
    }
}
