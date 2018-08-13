﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Bullet : MonoBehaviour
{
    // 총알 타입
    WeaponType W_Type = WeaponType.None;

    // 속도
    float Speed = 10f;

    // 파워
    int Power = 1;

    // Pool에 반환할 스크립트
    Gun GunScript;

    // 스프라이트
    SpriteRenderer BulletSR; // 스프라이트 그리는거
    Sprite[] BulletEffect; // 총알 이펙트

    // 부딪혔는지(이펙트와 총알을 구분/부메랑 돌아오는거)
    bool IsHit = false;

    // 날아갈 방향
    Vector3 Dir;

    // 플레이어 위치
    Transform PlayerTF;

    //총알생존시간(Invoke대체변수)
    float SurviveTime = 3f;
    float CurrentTime = 0f; // 누적 시간

    private void Awake()
    {
        BulletSR = GetComponentInChildren<SpriteRenderer>();
        if (BulletSR == null)
        {
            Debug.LogError("BulletSR 못 찾음");
            return;
        }

        // 총알 이펙트
        BulletEffect = Resources.LoadAll<Sprite>("Sprites/BulletEffect");
        if (BulletEffect == null)
        {
            Debug.LogError("BulletEffect 못 찾음");
            return;
        }

        PlayerTF = GameObject.Find("Player").transform;
        if (PlayerTF == null)
        {
            Debug.LogError("PlayerTF 못 찾음");
            return;
        }
    }

    // 켜졌을 때(맨 처음 만들 때도 생김)
    private void OnEnable()
    {
        // 시간 초기화
        CurrentTime = 0;

        // 방향 설정
        Dir = transform.forward;

        // 움직이게
        IsHit = false;
    }

    private void Update()
    {
        switch (W_Type)
        {
            case WeaponType.Gun:
                if (IsHit == false)
                {
                    SurviveTimeCheck();
                    Move();
                }
                break;
            case WeaponType.Boomerang:
                SurviveTimeCheck();
                Move();
                break;
        }
    }

    // 움직임
    private void Move()
    {
        switch (W_Type)
        {
            case WeaponType.Gun:
                transform.position += Dir * Speed * Time.deltaTime;
                break;
            case WeaponType.Boomerang:
                // 뭔가에 부딪히면 플레이어에게 돌아옴
                if (IsHit)
                {
                    Dir = PlayerTF.position - transform.position;
                    Dir.Normalize();
                }

                transform.position += Dir * Speed * Time.deltaTime;

                // 회전
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + Time.deltaTime * 1000f, 0);
                break;
        }
    }

    // Gun 스크립트 세팅
    public void SetGun(Gun gun)
    {
        GunScript = gun;
    }

    // 살아있는 시간 체크
    void SurviveTimeCheck()
    {
        //Invoke대체 if문
        CurrentTime += Time.deltaTime;

        // 일정시간 뒤 끔
        if (CurrentTime >= SurviveTime)
        {
            Hit();
        }
    }

    // 맞음
    public void Hit()
    {
        IsHit = true; // 뭔가에 부딪힘

        // 터지는 이펙트
        if (W_Type == WeaponType.Gun)
        {
            StartCoroutine("SpriteChange");
        }
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
        GunScript.BulletCollect(gameObject, W_Type);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 벽과 부딪히면 사라짐
        if (other.tag.Equals("Wall"))
        {
            Hit();
        }

        // 부메랑 돌아오면 회수
        if(W_Type == WeaponType.Boomerang && IsHit)
        {
            if (other.tag.Equals("Player"))
            {
                // 끄기
                gameObject.SetActive(false);
                // 풀에 넣음
                GunScript.BulletCollect(gameObject, W_Type);
            }
        }
    }

    // 속성 세팅
    public void SetAttribute(WeaponType _w_type, float _speed, int _power, float _surviveTime)
    {
        W_Type = _w_type;
        Speed = _speed;
        Power = _power;
        SurviveTime = _surviveTime;
    }

    // 공격력 반환
    public int GetPower()
    {
        return Power;
    }
}
