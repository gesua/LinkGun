using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    // 화면 넘어갈 수 있는 크기
    //float MinX = -4;
    //float MaxX = 4;
    //float MinZ = -2;
    //float MaxZ = 2;

    Transform target; // 플레이어 위치

    void Start()
    {
        target = GameObject.Find("Player").transform;
        if (target == null)
        {
            Debug.LogError("Player 못 찾음");
            return;
        }
    }

    void Update()
    {
        //CameraWork();

        // 캐릭터가 항상 카메라 중앙에 옴
        Vector3 temp = target.position;
        temp.y = 0;
        transform.position = temp;
    }

    // 카메라가 따라다니는 거(이제 안씀)
    //void CameraWork()
    //{
    //    Vector3 myPos = transform.position; // 카메라 위치
    //    Vector3 targetPos = target.position; // 플레이어 위치
    //    Vector3 tempPos = Vector3.zero; // 계산 값

    //    float tempX = targetPos.x - myPos.x; // X좌표 계산
    //    float tempZ = targetPos.z - myPos.z; // Z좌표 계산

    //    // X
    //    if (tempX < MinX)
    //    {
    //        tempPos.x = targetPos.x - (myPos.x + MinX);
    //    }
    //    else if (tempX > MaxX)
    //    {
    //        tempPos.x = targetPos.x - (myPos.x + MaxX);
    //    }

    //    // Z
    //    if (tempZ < MinZ)
    //    {
    //        tempPos.z = targetPos.z - (myPos.z + MinZ);
    //    }
    //    else if (tempZ > MaxZ)
    //    {
    //        tempPos.z = targetPos.z - (myPos.z + MaxZ);
    //    }

    //    // 값 더함
    //    transform.position += tempPos;
    //}
}
