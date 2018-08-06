using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Bullet : MonoBehaviour
{
    // 속도
    public float speed = 10f;

    // 파워
    public int power = 1;

    // Pool에 반환할 스크립트
    Gun GunScript;

    // 켜졌을 때(맨 처음 만들 때도 생김)
    private void OnEnable()
    {
        // 일단 전부 끔
        CancelInvoke();

        // 3초 뒤 사라짐
        Invoke("Off", 3f);
    }

    private void Update()
    {
        Vector3 dir = transform.forward;

        // 날아감
        transform.position += dir * speed * Time.deltaTime;
    }

    // Gun 스크립트 세팅
    public void SetGun(Gun gun)
    {
        GunScript = gun;
    }

    // 끄고 Pool에 넣음
    void Off()
    {
        gameObject.SetActive(false);

        GunScript.AddBulletPool(gameObject);
    }
}
