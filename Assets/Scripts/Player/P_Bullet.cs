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

    // 스프라이트
    SpriteRenderer BulletSR; // 스프라이트 그리는거
    Sprite BulletSprite; // 총알 이미지
    Sprite[] BulletEffect; // 총알 이펙트

    // 움직이는지(이펙트와 총알을 구분)
    bool IsMove = false;

    private void Awake()
    {
        BulletSR = GetComponentInChildren<SpriteRenderer>();
        if (BulletSR == null)
        {
            Debug.LogError("BulletSR 못 찾음");
            return;
        }

        // 총알 스프라이트
        BulletSprite = BulletSR.sprite;
        if (BulletSprite == null)
        {
            Debug.LogError("BulletSprite 못 찾음");
            return;
        }

        // 총알 이펙트
        BulletEffect = Resources.LoadAll<Sprite>("Sprites/BulletEffect");
        if (BulletEffect == null)
        {
            Debug.LogError("BulletEffect 못 찾음");
            return;
        }
    }

    // 켜졌을 때(맨 처음 만들 때도 생김)
    private void OnEnable()
    {
        // 일단 전부 끔
        CancelInvoke();

        // 총알 스프라이트로 변경
        BulletSR.sprite = BulletSprite;

        // 움직이게
        IsMove = true;

        // 3초 뒤 사라짐
        Invoke("Off", 3f);
    }

    private void Update()
    {
        if (IsMove)
        {
            Move();
        }
    }

    // 움직임
    private void Move()
    {
        // 전방으로 움직임
        Vector3 dir = transform.forward;
        transform.position += dir * speed * Time.deltaTime;
    }

    // Gun 스크립트 세팅
    public void SetGun(Gun gun)
    {
        GunScript = gun;
    }

    // 인보크 끄기
    public void InvokeOff()
    {
        CancelInvoke();
    }

    // 걍 끄기
    public void Off()
    {
        // 끄기
        gameObject.SetActive(false);

        // 풀에 넣음
        GunScript.AddBulletPool(gameObject);
    }

    // 맞음
    public void Hit()
    {
        IsMove = false; // 안 움직이게

        StartCoroutine("SpriteChange");
    }

    // 이펙트로 변경
    IEnumerator SpriteChange()
    {
        BulletSR.sprite = BulletEffect[0];
        for (int i = 1; i < BulletEffect.Length; i++)
        {
            yield return new WaitForSeconds(0.05f);
            BulletSR.sprite = BulletEffect[i];
        }

        // 끄기
        gameObject.SetActive(false);

        // 풀에 넣음
        GunScript.AddBulletPool(gameObject);
    }
}
