using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    // 화면 넘어갈 수 있는 크기
    float MinX = -5;
    float MaxX = 5;
    float MinZ = -3;
    float MaxZ = 3;

    Transform target; // 플레이어 위치

    // Use this for initialization
    void Start()
    {
        target = GameObject.Find("Player").transform;
        if (target == null)
        {
            Debug.LogError("Player 못 찾음");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 myPos = transform.position;
        Vector3 targetPos = target.position;
        Vector3 tempPos = Vector3.zero;

        float tempX = myPos.x - targetPos.x;
        float tempZ = myPos.z - targetPos.z;

        Debug.Log(tempX + ", " + tempZ);

        if(tempX < -MinX)
        {
            tempPos.x = targetPos.x - (myPos.x + Mathf.Abs(MinX));
        }

        //if (tempX > MaxX || tempX < MinX)
        //{
        //    tempPos.x = myPos.x - targetPos.x;
        //}
        //if(tempZ > MaxZ || tempZ < MinZ)
        //{
        //    tempPos.z = myPos.z - targetPos.z;
        //}

        transform.position += tempPos;
    }
}
