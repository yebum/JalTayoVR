using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRFollowUI : MonoBehaviour
{

    public Transform targetCamera; // 메타퀘스트의 CenterEyeAnchor를 연결
    public float distance = 2.0f;  // UI와 플레이어 사이의 거리
    public float followSpeed = 2.0f; // 따라오는 부드러움 정도

    void Start()
    {
        // 시작하자마자 카메라 앞 distance 거리 위치에 UI를 딱 갖다 놓습니다.
        transform.position = targetCamera.position + targetCamera.forward * distance;
    }
    void Update()
    {
        // 1. 카메라 앞 지점 계산
        Vector3 targetPos = targetCamera.position + targetCamera.forward * distance;

        // 2. 부드럽게 위치 이동
        transform.position = Vector3.Slerp(transform.position, targetPos, Time.deltaTime * followSpeed);

        // 3. 회전 설정 (핵심 수정 부분)
        // LookAt 대신 카메라의 회전값(Rotation)을 그대로 복사하면 
        // 억지로 180도 돌릴 필요 없이 정면을 제대로 바라보게 됩니다.
        transform.rotation = Quaternion.Slerp(transform.rotation, targetCamera.rotation, Time.deltaTime * followSpeed);
    }
}
