using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class BusRoute : MonoBehaviour
{
    public testscript TS;

    public enum StopType
    {
        Boarding,
        Optional,
        Final
    }

    [System.Serializable]
    public class RoutePoint
    {
        [Header("목표 지점")]
        public Transform target;

        [Header("정류장 타입")]
        public StopType stopType = StopType.Optional;

        [Header("정차 시간")]
        public float waitTime = 10f;
    }

    [Header("Route Points")]
    [SerializeField] private List<RoutePoint> routePoints = new List<RoutePoint>();

    [Header("Timing")]
    [SerializeField] private float startDelay = 3f;

    [Header("Move Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float arriveDistance = 0.1f;

    [Header("Bell System")]
    [SerializeField] private BellController bellController;

    [Header("Door System")]
    [SerializeField] private DoorController doorController;

    [Header("Card Reader")]
    [SerializeField] private CardReader cardReader;

    private Rigidbody rb;
    private int currentIndex = 0;
    private bool isMoving = false;
    private bool routeFinished = false;
    private bool stopRequested = false;
    private Transform currentTarget;

    private enum RouteState
    {
        WaitingToStart,
        Moving,
        WaitingAtStop,
        Finished
    }

    private RouteState currentState = RouteState.WaitingToStart;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
    }

    private void Start()
    {
        if (routePoints == null || routePoints.Count == 0)
        {
            Debug.LogError("Route Points가 비어 있습니다.");
            enabled = false;
            return;
        }

        StartCoroutine(RouteRoutine());
    }

    public void RequestStop()
    {
        if (routeFinished) return;

        stopRequested = true;
        Debug.Log("[하차벨 인식됨] 다음 정류장 정차 요청");
    }

    private IEnumerator RouteRoutine()
    {
        currentState = RouteState.WaitingToStart;
        routeFinished = false;

        Debug.Log("버스 출발 전 대기");
        yield return new WaitForSeconds(startDelay);

        currentIndex = 0;

        while (currentIndex < routePoints.Count)
        {
            RoutePoint point = routePoints[currentIndex];
            currentTarget = point.target;

            currentState = RouteState.Moving;
            isMoving = true;

            Debug.Log($"{currentIndex + 1}번 지점으로 이동 시작");

            while (currentState == RouteState.Moving)
            {
                yield return null;
            }

            isMoving = false;

            bool shouldStop = ShouldStopAtPoint(point);

            if (shouldStop)
            {
                yield return StartCoroutine(StopRoutine(point));
            }
            else
            {
                Debug.Log($"[하차벨 인식안됨] {currentIndex + 1}번 정류장 통과");
            }

            currentIndex++;
        }

        currentState = RouteState.Finished;
        routeFinished = true;
        isMoving = false;

        Debug.Log("모든 경로 이동 완료");

        TS.ExitBus();
    }

    private bool ShouldStopAtPoint(RoutePoint point)
    {
        if (point.stopType == StopType.Boarding)
        {
            Debug.Log("[탑승 정류장] 벨 입력 없이 무조건 정차");
            return true;
        }

        if (point.stopType == StopType.Final)
        {
            Debug.Log("[목적지] 무조건 정차");
            return true;
        }

        if (point.stopType == StopType.Optional)
        {
            if (stopRequested)
            {
                Debug.Log("[하차벨 인식됨] 정류장 정차");
                return true;
            }
            else
            {
                Debug.Log("[하차벨 인식안됨] 정류장 통과");
                return false;
            }
        }

        return false;
    }

    private IEnumerator StopRoutine(RoutePoint point)
    {
        currentState = RouteState.WaitingAtStop;

        Debug.Log($"{currentIndex + 1}번 지점 정차");

        if (doorController != null)
        {
            doorController.OpenDoor();
        }

        if (point.stopType == StopType.Boarding)
        {
            Debug.Log("탑승 정류장: 카드 태그 대기");

            if (cardReader != null)
            {
                while (!cardReader.isTagged)
                {
                    yield return null;
                }

                Debug.Log("카드 태그 완료: 버스 출발 준비");
                TS.BoardBus(GetComponent<Rigidbody>());
            }
            else
            {
                Debug.LogWarning("CardReader가 연결되지 않았습니다. waitTime만큼 대기합니다.");
                yield return new WaitForSeconds(point.waitTime);
            }
        }
        else
        {
            yield return new WaitForSeconds(point.waitTime);
        }

        stopRequested = false;

        if (bellController != null)
        {
            bellController.ResetBell();
        }

        if (doorController != null)
        {
            doorController.CloseDoor();
        }

        yield return new WaitForSeconds(1f);
    }

    private void FixedUpdate()
    {
        if (!isMoving || routeFinished || currentTarget == null)
            return;

        MoveToTarget();
    }

    private void MoveToTarget()
    {
        Vector3 currentPosition = rb.position;
        Vector3 targetPosition = currentTarget.position;

        Vector3 nextPosition = Vector3.MoveTowards(
            currentPosition,
            targetPosition,
            moveSpeed * Time.fixedDeltaTime
        );

        rb.MovePosition(nextPosition);

        // Vector3 direction = targetPosition - currentPosition;
        // direction.y = 0f;
        //
        // if (direction.sqrMagnitude > 0.0001f)
        // {
        //     Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
        //     rb.MoveRotation(targetRotation);
        // }

        if (Vector3.Distance(nextPosition, targetPosition) <= arriveDistance)
        {
            rb.MovePosition(targetPosition);
            isMoving = false;
            currentState = RouteState.WaitingAtStop;
        }
    }
}