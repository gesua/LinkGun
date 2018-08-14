using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArrow : MonoBehaviour
{
    // 위치
    Transform PlayerTF; // 플레이어
    Transform BossTF;   // 보스

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

        // 보스가 화면에 안 잡히면 화살표 표시
        if (bossPos.x < -30 || bossPos.x > 1250 || bossPos.y < -30 || bossPos.y > 740)
        {
            // 화살표 방향 잡음
            Vector3 dir = BossTF.position - PlayerTF.position;

            float rotateDegree = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
            Arrow.transform.rotation = Quaternion.Euler(0f, 0f, rotateDegree + 90f);

            // 화살표 위치 잡음
            transform.position = Camera.main.WorldToScreenPoint(PlayerTF.position + dir.normalized);

            // 화살표 띄움
            Arrow.SetActive(true);
        }
        else
        {
            Arrow.SetActive(false);
        }
    }
}
