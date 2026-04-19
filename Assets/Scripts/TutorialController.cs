using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    public GameObject tutorialCanvas;
    public Image tutorialDisplay;   // 이미지가 교체될 UI Image 컴포넌트
    public Sprite[] tutorialImages; // 이미지 넣기

    private int currentIndex = 0;   // 몇 번째 장인지 기억

    void Start()
    {
        // 시작할 때 첫 번째 이미지
        if (tutorialImages.Length > 0)
        {
            tutorialDisplay.sprite = tutorialImages[0];
        }
    }

    // [Next 버튼에 연결할 함수]
    public void NextPage()
    {
        currentIndex++; // 다음 장으로 번호 증가

        // 아직 보여줄 이미지가 남았다면 교체
        if (currentIndex < tutorialImages.Length)
        {
            tutorialDisplay.sprite = tutorialImages[currentIndex];
            Debug.Log("다음 페이지: " + currentIndex);
        }
        else
        {
            // 마지막 장에서 더 누르면 튜토리얼 종료
            CloseTutorial();
        }
    }

    // [닫기 버튼에 연결할 함수]
    public void CloseTutorial()
    {
        tutorialCanvas.SetActive(false);
        Debug.Log("튜토리얼 종료!");
    }
}