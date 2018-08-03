﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // 체력
    int HP = 3; // 잔기
    Image[] HP_Sprite; // HP 이미지
    Sprite DamageHPSprite; // 데미지 입은 HP 이미지

    // 이동속도
    public float Speed = 3f;

    // 애니메이션 상태
    enum ANI_STATE
    {
        Idle,
        Move
    }
    ANI_STATE PrevState = ANI_STATE.Idle; // 이전 상태
    ANI_STATE NextState = ANI_STATE.Idle; // 다음 상태
    Animator PlayerAnimator;

    Transform ImageTF; // 이미지 위치(좌우반전)

    int Way = 0; // 어디 바라보고 있는지(1~9 키패드, 5는 안 씀)

    // 공격당한 뒤 무적 시간
    float InvincibleTime = 1f; // 무적 시간
    float InvincibleTimeCount = 0; // 세는거

    Gun GunScript;
    RedBlink Blink;

    private void Start()
    {
        // 애니메이터
        PlayerAnimator = GetComponentInChildren<Animator>();
        if (PlayerAnimator == null)
        {
            Debug.LogError("PlayerAnimator 못 찾음");
            return;
        }

        // 이미지 위치(좌우반전)
        ImageTF = transform.Find("Image");
        if (ImageTF == null)
        {
            Debug.LogError("ImageTF 못 찾음");
            return;
        }

        // Gun스크립트
        GunScript = GetComponentInChildren<Gun>();
        if (GunScript == null)
        {
            Debug.LogError("GunScript 못 찾음");
            return;
        }

        // HP 이미지 찾기
        Transform tempTF = GameObject.Find("PlayerHP").transform;
        HP_Sprite = tempTF.GetComponentsInChildren<Image>();
        for (int i = 0; i < HP_Sprite.Length; i++)
        {
            if (HP_Sprite[i] == null)
            {
                Debug.Log(HP_Sprite[i].name + " 못 찾음");
                return;
            }
        }

        // 데미지 입은 HP 찾기
        Sprite[] tempSprite = Resources.LoadAll<Sprite>("Sprites/PokemonUI");
        for (int i = 0; i < tempSprite.Length; i++)
        {
            // 데미지 입은 HP
            if (tempSprite[i].name.Equals("HP_OFF"))
            {
                DamageHPSprite = tempSprite[i];
                break; // 1개만 찾으면 되니 바로 나옴
            }
        }
        if (DamageHPSprite == null)
        {
            Debug.Log("DamageHPImage 못 찾음");
            return;
        }

        // 깜빡이는 스크립트
        Blink = GetComponent<RedBlink>();
        if (Blink == null)
        {
            Debug.Log("Blink 못 찾음");
            return;
        }
    }

    void Update()
    {
        // 이동
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            NextState = ANI_STATE.Move;
            Move();
        }
        else // 기본
        {
            NextState = ANI_STATE.Idle;
        }

        PlayAnimator();
    }

    // 이동(WASD)
    void Move()
    {
        Vector3 moveVector = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            moveVector.z += Speed * Time.deltaTime;
            Way = 8;

            // 이미지 좌우반전
            if (ImageTF.localScale.x < 0)
            {
                Vector3 temp = ImageTF.localScale;
                temp.x *= -1;
                ImageTF.localScale = temp;
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moveVector.z -= Speed * Time.deltaTime;
            Way = 2;

            // 이미지 좌우반전
            if (ImageTF.localScale.x < 0)
            {
                Vector3 temp = ImageTF.localScale;
                temp.x *= -1;
                ImageTF.localScale = temp;
            }
        }


        if (Input.GetKey(KeyCode.A))
        {
            moveVector.x -= Speed * Time.deltaTime;

            // 대각선 이동인지 판별
            if (Input.GetKey(KeyCode.W)) Way = 7;
            else if (Input.GetKey(KeyCode.S)) Way = 1;
            else Way = 4;

            // 이미지 좌우반전
            if (ImageTF.localScale.x > 0)
            {
                Vector3 temp = ImageTF.localScale;
                temp.x *= -1;
                ImageTF.localScale = temp;
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveVector.x += Speed * Time.deltaTime;

            // 대각선 이동인지 판별
            if (Input.GetKey(KeyCode.W)) Way = 9;
            else if (Input.GetKey(KeyCode.S)) Way = 3;
            else Way = 6;

            // 이미지 좌우반전
            if (ImageTF.localScale.x < 0)
            {
                Vector3 temp = ImageTF.localScale;
                temp.x *= -1;
                ImageTF.localScale = temp;
            }
        }

        transform.position += moveVector;

        // 총 위치 변경
        GunScript.SetPosition(Way);
    }

    // 애니메이터 재생
    void PlayAnimator()
    {
        // 애니메이션 변경
        if (PrevState != NextState)
        {
            PrevState = NextState;
            PlayerAnimator.SetInteger("state", (int)PrevState);
        }

        if (PrevState == ANI_STATE.Idle) // 멈춰있는 상태
        {
            switch (Way)
            {
                case 8: // ↑
                    PlayerAnimator.Play("Idle", -1, 0.4f);
                    break;
                case 9: // ↗
                case 7:
                    PlayerAnimator.Play("Idle", -1, 0.8f);
                    break;
                case 6: // →
                case 4:
                    PlayerAnimator.Play("Idle", -1, 0.2f);
                    break;
                case 3:// ↘
                case 1:
                    PlayerAnimator.Play("Idle", -1, 0.6f);
                    break;
                case 2: // ↓
                    PlayerAnimator.Play("Idle", -1, 0f);
                    break;
            }
        }
        else if (PrevState == ANI_STATE.Move) // 이동이면 방향값 전달
        {
            if (Way == 1 || Way == 4 || Way == 7) Way += 2; // 오른쪽 보는 값

            PlayerAnimator.SetInteger("way", Way);
        }
    }

    // 피격
    private void OnTriggerEnter(Collider other)
    {
        if (HP <= 0) return; // 이미 죽음
        //if (InvincibleTimeCount != 0) return; // 무적시간

        // 적 총알일 경우
        if (other.tag.Equals("E_Bullet"))
        {
            // 체력 깎임
            HP -= other.GetComponent<E_Bullet>().power;

            // 넉백
            //Vector3 dir = transform.position - other.transform.position;
            //transform.position += dir.normalized;

            // 깜빡임
            Blink.BlinkStart();

            // 무적 시간 시작
            InvincibleTimeCount += Time.deltaTime;

            // 이미지 변경
            HP_Sprite[HP].sprite = DamageHPSprite;

            // 총알 삭제
            Destroy(other.gameObject);
        }
    }
}
