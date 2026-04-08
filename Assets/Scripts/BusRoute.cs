using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class BusRoute : MonoBehaviour
{
    [System.Serializable]
    public class RoutePoint
    {
        [Header("목표 지점")]
        public Transform target;

        [Header("정류장 여부")]
        public bool isStopPoint = false;

        [Header("정류장일 때 대기 시간")]
        public float waitTime = 10f;
    }

    [Header("Route Points")]
    [SerializeField] private List<RoutePoint> routePoints = new List<RoutePoint>();

    [Header("Timing")]
    [SerializeField] private float startDelay = 7f;

    [Header("Move Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float arriveDistance = 0.1f;

    private Rigidbody rb;
    private int currentIndex = 0;
    private bool isMoving = false;
    private bool routeFinished = false;
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

        for (int i = 0; i < routePoints.Count; i++)
        {
            if (routePoints[i].target == null)
            {
                Debug.LogError($"Route Points의 {i}번 Target이 비어 있습니다.");
                enabled = false;
                return;
            }
        }

        StartCoroutine(RouteRoutine());
    }

    private IEnumerator RouteRoutine()
    {
        currentState = RouteState.WaitingToStart;
        isMoving = false;
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

            Debug.Log($"{currentIndex + 1}번째 지점으로 이동 시작");

            while (currentState == RouteState.Moving)
            {
                yield return null;
            }

            isMoving = false;

            if (point.isStopPoint)
            {
                currentState = RouteState.WaitingAtStop;
                Debug.Log($"{currentIndex + 1}번째 정류장 도착 - {point.waitTime}초 대기");
                yield return new WaitForSeconds(point.waitTime);
            }

            currentIndex++;
        }

        currentState = RouteState.Finished;
        routeFinished = true;
        isMoving = false;
        Debug.Log("모든 경로 이동 완료");
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

        Vector3 direction = targetPosition - currentPosition;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
            rb.MoveRotation(targetRotation);
        }

        if (Vector3.Distance(nextPosition, targetPosition) <= arriveDistance)
        {
            rb.MovePosition(targetPosition);
            isMoving = false;
            currentState = RouteState.WaitingAtStop;
        }
    }
}