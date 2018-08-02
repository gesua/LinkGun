using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    GameObject BulletPrefab;
    Transform Target; // 마우스 위치

    SpriteRenderer GunImage;

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
    }

    void Update()
    {
        // 마우스 포인터 바라봄
        LookTarget();

        // 총알 발사
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    // 총알 발사
    void Shoot()
    {
        GameObject tempBullet = Instantiate(BulletPrefab);
        tempBullet.transform.position = transform.position + transform.forward * 0.5f; // 약간 앞에서 발사
        tempBullet.transform.LookAt(Target);

        // x축 회전 없앰
        Vector3 tempAngle = tempBullet.transform.eulerAngles;
        tempAngle.x = 0;
        tempBullet.transform.eulerAngles = tempAngle;
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
            case 2: // 아래
                transform.localPosition = new Vector3(-0.18f, 0, -0.15f);
                GunImage.sortingOrder = 3;
                break;
            case 8: // 위
                transform.localPosition = new Vector3(0.2f, 0, -0.038f);
                GunImage.sortingOrder = 1;
                break;
            default:
                transform.localPosition = new Vector3(0, 0, 0);
                GunImage.sortingOrder = 1;
                break;
        }
    }
}
