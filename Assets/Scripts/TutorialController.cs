using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    public GameObject tutorialCanvas;
    public GameObject missionCanvas; // 미션창 오브젝트 연결
    public Image tutorialDisplay;
    public Sprite[] tutorialImages;
    private int currentIndex = 0;

    public void NextPage()
    {
        currentIndex++;
        if (currentIndex < tutorialImages.Length)
        {
            tutorialDisplay.sprite = tutorialImages[currentIndex];
        }
        else
        {
            CloseTutorial();
        }
    }

    //실행하기 전에 무조건 미션창 캔버스 인스펙터에서 끄고 시작하기!!!!
    //안그러면 처음부터 계속 미션창이 붙어다님
    public void CloseTutorial()
    {
        tutorialCanvas.SetActive(false); // 튜토리얼 끄기
        if (missionCanvas != null)
        {
            missionCanvas.SetActive(true); // 미션창 켜기 (이때 미션 스크립트가 작동)
        }
    }
}