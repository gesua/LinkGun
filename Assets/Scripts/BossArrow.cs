using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArrow : MonoBehaviour
{
    // 위치
    Transform PlayerTF; // 플레이어
    Transform BossTF;   // 보스

    // 화면에 보이는지
    bool BossScreen = false;

    // 화살표
    GameObject Arrow;

    void Start()
    {
        PlayerTF = GameObject.Find("Player").transform;
        if (PlayerTF == null)
        {
            Debug.LogError("PlayerTF 못 찾음");
            return;
        }

        BossTF = GameObject.Find("Enemy").transform;
        if (BossTF == null)
        {
            Debug.LogError("BossTF 못 찾음");
            return;
        }

        Arrow = transform.GetChild(0).gameObject;
        if (Arrow == null)
        {
            Debug.LogError("Arrow 못 찾음");
            return;
        }
    }

    void Update()
    {
        // 보스의 위치를 스크린 좌표로 바꿈
        Vector3 bossPos = Camera.main.WorldToScreenPoint(BossTF.position);

        // 화살표 표시할 위치
        Vector3 tempPos = Vector3.zero;

        // 보스 스크린에 보이는지 체크
        BossScreen = false;

        // 화살표 위치 잡아줌
        if (bossPos.x < -40)
        {
            tempPos.x = -900;
            BossScreen = true;
        }
        else if (bossPos.x > 1260)
        {
            tempPos.x = -900;
            BossScreen = true;
        }

        if (bossPos.y < -40)
        {
            tempPos.y = -500;
            BossScreen = true;
        }
        else if (bossPos.y > 750)
        {
            tempPos.y = 500;
            BossScreen = true;
        }

        //Debug.Log(Camera.main.WorldToScreenPoint(BossTF.localPosition));
        Vector3 temp = Camera.main.WorldToScreenPoint(BossTF.localPosition);
        //Debug.Log(temp.x);
        temp.x -= 612.5f;
        temp.y = 0;
        temp.z = 0;

        // 보스가 화면에 안 잡히면 화살표 표시
        if (BossScreen)
        {
            Arrow.SetActive(true);

            Vector3 dir = BossTF.position - PlayerTF.position;

            float rotateDegree = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
            Arrow.transform.rotation = Quaternion.Euler(0f, 0f, rotateDegree + 90f);

            transform.localPosition = tempPos;
            transform.localPosition += temp;
        }
        else
        {
            Arrow.SetActive(false);
        }
    }
}
