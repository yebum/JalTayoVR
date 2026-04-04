using UnityEngine;

public class PlayerSeatController : MonoBehaviour
{
    [Header("XR Origin Root")]
    [SerializeField] private Transform xrOriginRoot;

    [Header("Movement")]
    [SerializeField] private MonoBehaviour locomotionScript;
    [SerializeField] private CharacterController characterController;

    [Header("Optional")]
    [SerializeField] private float autoExitMoveThreshold = 0.2f;

    private BusSeat currentSeat;
    private bool isSeated = false;

    public bool IsSeated => isSeated;

    private void Update()
    {
        if (!isSeated) return;

        // 이동 입력이 들어오면 자동 하차
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector2 moveInput = new Vector2(moveX, moveZ);

        if (moveInput.magnitude > autoExitMoveThreshold)
        {
            ExitSeat();
        }
    }

    public void SitOnSeat(BusSeat seat)
    {
        if (seat == null) return;
        if (!seat.TrySit()) return;

        if (currentSeat != null)
        {
            currentSeat.LeaveSeat();
        }

        currentSeat = seat;
        isSeated = true;

        // 이동 비활성화
        if (locomotionScript != null)
            locomotionScript.enabled = false;

        if (characterController != null)
            characterController.enabled = false;

        // XR Origin을 좌석 위치로 이동
        xrOriginRoot.position = seat.SeatAnchor.position;
        xrOriginRoot.rotation = seat.SeatAnchor.rotation;

        // 다시 켜기
        if (characterController != null)
            characterController.enabled = true;
    }

    public void ExitSeat()
    {
        if (!isSeated) return;

        if (currentSeat != null)
        {
            currentSeat.LeaveSeat();
            currentSeat = null;
        }

        isSeated = false;

        if (locomotionScript != null)
            locomotionScript.enabled = true;
    }
}