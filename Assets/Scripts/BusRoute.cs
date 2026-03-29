using System.Collections;
using UnityEngine;

public class BusRouteMover : MonoBehaviour
{
    public Transform[] waypoints;
    public float moveSpeed = 3f;
    public float rotateSpeed = 3f;
    public float stopDistance = 0.2f;

    public int stopPointIndex = 1;   // Stop 포인트 인덱스
    public float waitTime = 7f;     // 정차 시간

    private int currentIndex = 0;
    private bool isMoving = false;
    private bool isWaiting = false;

    void Start()
    {
        StartMove();
    }

    public void StartMove()
    {
        if (waypoints == null || waypoints.Length == 0) return;
        isMoving = true;
    }

    public void StopMove()
    {
        isMoving = false;
    }

    IEnumerator WaitAndMove()
    {
        isWaiting = true;
        isMoving = false;

        yield return new WaitForSeconds(waitTime);

        isWaiting = false;
        isMoving = true;
    }

    void Update()
    {
        if (!isMoving) return;
        if (currentIndex >= waypoints.Length) return;

        Transform target = waypoints[currentIndex];

        Vector3 direction = target.position - transform.position;
        direction.y = 0f;

        if (direction.magnitude <= stopDistance)
        {
            transform.position = target.position;

            if (currentIndex == stopPointIndex && !isWaiting)
            {
                currentIndex++;
                StartCoroutine(WaitAndMove());
                return;
            }

            currentIndex++;

            if (currentIndex >= waypoints.Length)
            {
                isMoving = false;
            }
            return;
        }

        Vector3 moveDir = direction.normalized;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        if (moveDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotateSpeed * Time.deltaTime
            );
        }
    }
}