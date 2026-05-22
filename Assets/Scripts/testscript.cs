using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testscript : MonoBehaviour
{
    [SerializeField] private Rigidbody busRigidbody;

    private CharacterController cc;
    private Vector3 lastBusPosition;
    private bool isOnBus = false;

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!isOnBus || busRigidbody == null) return;

        Vector3 busDelta = busRigidbody.position - lastBusPosition;
        lastBusPosition = busRigidbody.position;

        // 버스 이동량을 CC에 전달 (플레이어 자체 이동은 XR이 알아서 처리)
        cc.Move(busDelta);
    }

    public void BoardBus(Rigidbody busRb)
    {
        busRigidbody = busRb;
        lastBusPosition = busRb.position;
        isOnBus = true;
    }

    public void ExitBus()
    {
        busRigidbody = null;
        isOnBus = false;
    }
}