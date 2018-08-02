using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    GameObject BulletPrefab;
    Transform Target; // 마우스 위치

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
}
