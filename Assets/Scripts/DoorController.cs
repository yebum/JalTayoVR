using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour
{
    [Header("Door Objects")]
    [SerializeField] private Transform door01;
    [SerializeField] private Transform door02;

    [Header("Open Offset - Local Position 기준")]
    [SerializeField] private Vector3 door01OpenOffset = new Vector3(0f, 0f, -1.1f);
    [SerializeField] private Vector3 door02OpenOffset = new Vector3(0f, 0f, -1.1f);

    [Header("Move Settings")]
    [SerializeField] private float moveTime = 1f;

    private Vector3 door01ClosedPos;
    private Vector3 door02ClosedPos;
    private Coroutine doorRoutine;

    private void Start()
    {
        if (door01 != null)
            door01ClosedPos = door01.localPosition;

        if (door02 != null)
            door02ClosedPos = door02.localPosition;
    }

    public void OpenDoor()
    {
        if (door01 == null || door02 == null) return;

        if (doorRoutine != null)
            StopCoroutine(doorRoutine);

        doorRoutine = StartCoroutine(MoveDoors(
            door01ClosedPos + door01OpenOffset,
            door02ClosedPos + door02OpenOffset
        ));

        Debug.Log("버스 문 열림");
    }

    public void CloseDoor()
    {
        if (door01 == null || door02 == null) return;

        if (doorRoutine != null)
            StopCoroutine(doorRoutine);

        doorRoutine = StartCoroutine(MoveDoors(
            door01ClosedPos,
            door02ClosedPos
        ));

        Debug.Log("버스 문 닫힘");
    }

    private IEnumerator MoveDoors(Vector3 door01Target, Vector3 door02Target)
    {
        float timer = 0f;

        Vector3 door01Start = door01.localPosition;
        Vector3 door02Start = door02.localPosition;

        while (timer < moveTime)
        {
            timer += Time.deltaTime;
            float t = timer / moveTime;

            door01.localPosition = Vector3.Lerp(door01Start, door01Target, t);
            door02.localPosition = Vector3.Lerp(door02Start, door02Target, t);

            yield return null;
        }

        door01.localPosition = door01Target;
        door02.localPosition = door02Target;
    }
}