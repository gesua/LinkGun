using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    GameObject BulletPrefab;
    Transform Target; // 마우스 위치

    SpriteRenderer GunImage;

    Transform BulletBox; // 총알 모아놓는 곳

    float CooldownTime = 0.2f; // 연사속도
    float CooldownCount = 0f; // 연사속도 세는거

    private void Start()
    {
        BulletPrefab = Resources.Load<GameObject>("Prefabs/P_Bullet");
        if (BulletPrefab == null)
        {
            Debug.LogError("BulletPrefab 못 찾음");
            return;
        }

        Target = GameObject.Find("MousePoint").transform;
        if (Target == null)
        {
            Debug.LogError("Target 못 찾음");
            return;
        }

        GunImage = GetComponentInChildren<SpriteRenderer>();
        if (GunImage == null)
        {
            Debug.LogError("GunImage 못 찾음");
            return;
        }

        BulletBox = GameObject.Find("P_Bullet").transform;
        if (BulletBox == null)
        {
            Debug.LogError("BulletBox 못 찾음");
            return;
        }
    }

    void Update()
    {
        // 마우스 포인터 바라봄
        LookTarget();

        // 쿨다운 증가
        if(CooldownCount != 0)
        {
            Cooldown();
        }

        // 총알 발사
        if (Input.GetMouseButton(0) && CooldownCount == 0)
        {
            Shoot();
        }
    }

    // 총알 발사
    void Shoot()
    {
        GameObject tempBullet = Instantiate(BulletPrefab);
        tempBullet.transform.parent = BulletBox; // 한 곳에 모아둠

        tempBullet.transform.position = transform.position + transform.forward * 0.5f; // 약간 앞에서 발사
        tempBullet.transform.LookAt(Target);

        // x축 회전 없앰
        Vector3 tempAngle = tempBullet.transform.eulerAngles;
        tempAngle.x = 0;
        tempBullet.transform.eulerAngles = tempAngle;

        // 쿨다운 시작
        CooldownCount += Time.deltaTime;
    }

    // 마우스 포인터 바라봄
    void LookTarget()
    {
        transform.LookAt(Target);

        // x축 회전 없앰
        Vector3 tempAngle = transform.eulerAngles;
        tempAngle.x = 0;
        transform.eulerAngles = tempAngle;
    }

    // 총 위치 설정
    public void SetPosition(int way)
    {
        switch (way)
        {
            case 1: // ↙
                transform.localPosition = new Vector3(-0.195f, 0, -0.12f);
                GunImage.sortingOrder = 3;
                break;
            case 2: // ↓
                transform.localPosition = new Vector3(-0.18f, 0, -0.15f);
                GunImage.sortingOrder = 3;
                break;
            case 3: // ↘
                transform.localPosition = new Vector3(-0.09f, 0, -0.2f);
                GunImage.sortingOrder = 3;
                break;
            case 4: // ←
                transform.localPosition = new Vector3(-0.2f, 0, -0.04f);
                GunImage.sortingOrder = 1;
                break;
            case 6: // →
                transform.localPosition = new Vector3(0.03f, 0, -0.2f);
                GunImage.sortingOrder = 3;
                break;
            case 7: // ↖
                transform.localPosition = new Vector3(0, 0, 0);
                GunImage.sortingOrder = 1;
                break;
            case 8: // ↑
                transform.localPosition = new Vector3(0.2f, 0, -0.038f);
                GunImage.sortingOrder = 1;
                break;
            case 9: // ↗
                transform.localPosition = new Vector3(0.1f, 0, -0.2f);
                GunImage.sortingOrder = 1;
                break;
        }
    }

    // 쿨다운 증가
    void Cooldown()
    {
        CooldownCount += Time.deltaTime;

        // 다 셌음
        if(CooldownCount >= CooldownTime)
        {
            CooldownCount = 0;
        }
    }
}
