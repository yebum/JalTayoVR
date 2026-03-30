using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class BusRoute : MonoBehaviour
{
    [Header("Route Points")]
    [SerializeField] private Transform midPoint;
    [SerializeField] private Transform stopPoint;

    [Header("Timing")]
    [SerializeField] private float startDelay = 7f;
    [SerializeField] private float midWaitTime = 7f;

    [Header("Move Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float arriveDistance = 0.1f;

    private Rigidbody rb;
    private Transform currentTarget;
    private bool isMoving = false;
    private bool routeFinished = false;

    private enum RouteState
    {
        WaitingToStart,
        MovingToMid,
        WaitingAtMid,
        MovingToStop,
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
        if (midPoint == null || stopPoint == null)
        {
            Debug.LogError("Mid Point와 Stop Point를 모두 연결해야 합니다.");
            enabled = false;
            return;
        }

        StartCoroutine(RouteRoutine());
    }

    private IEnumerator RouteRoutine()
    {
        currentState = RouteState.WaitingToStart;
        isMoving = false;

        Debug.Log("버스 출발 전 대기");
        yield return new WaitForSeconds(startDelay);

        currentState = RouteState.MovingToMid;
        currentTarget = midPoint;
        isMoving = true;
        Debug.Log("버스 출발 - 중간지점으로 이동");

        while (currentState == RouteState.MovingToMid)
        {
            yield return null;
        }

        currentState = RouteState.WaitingAtMid;
        isMoving = false;
        Debug.Log("중간지점 도착 - 대기 시작");
        yield return new WaitForSeconds(midWaitTime);

        currentState = RouteState.MovingToStop;
        currentTarget = stopPoint;
        isMoving = true;
        Debug.Log("최종지점으로 다시 출발");

        while (currentState == RouteState.MovingToStop)
        {
            yield return null;
        }

        currentState = RouteState.Finished;
        isMoving = false;
        routeFinished = true;
        Debug.Log("최종지점 도착");
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

            if (currentState == RouteState.MovingToMid)
            {
                isMoving = false;
                currentState = RouteState.WaitingAtMid;
            }
            else if (currentState == RouteState.MovingToStop)
            {
                isMoving = false;
                currentState = RouteState.Finished;
            }
        }
    }
}