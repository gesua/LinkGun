using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    // 화면 넘어갈 수 있는 크기
    //float MinX = -3;
    //float MaxX = 3;
    //float MinZ = -1;
    //float MaxZ = 1;

    Transform Target; // 플레이어 위치
    Transform MouseTF; // 마우스 위치

    void Start()
    {
        Target = GameObject.Find("Player").transform;
        if (Target == null)
        {
            Debug.LogError("Player 못 찾음");
            return;
        }

        MouseTF = GameObject.Find("MousePoint").transform;
        if (MouseTF == null)
        {
            Debug.LogError("MouseTF 못 찾음");
            return;
        }
    }

    void Update()
    {
        Vector3 tempPos = (MouseTF.position + Target.position) / 2;
        tempPos += (Target.position - MouseTF.position) / 4;
        tempPos.y = 0;
        transform.position = tempPos;
    }

    // 카메라가 따라다니는 거
    //void CameraWork()
    //{
    //    Vector3 myPos = transform.position; // 카메라 위치
    //    Vector3 targetPos = Target.position; // 플레이어 위치
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
