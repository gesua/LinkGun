using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // 체력
    public int HP = 100;

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

    Sprite[] IdleSprite; // 기본(상하우)
    SpriteRenderer PlayerSprite;
    Transform ImageTF; // 이미지 위치

    int Way = 0; // 어디 바라보고 있는지(1~9 키패드)

    Gun GunScript;

    private void Start()
    {
        PlayerAnimator = GetComponentInChildren<Animator>();
        if (PlayerAnimator == null)
        {
            Debug.LogError("PlayerAnimator 못 찾음");
            return;
        }

        // 기본 상태 이미지
        Sprite[] tempSprite = Resources.LoadAll<Sprite>("Sprites/LinkImage");
        IdleSprite = new Sprite[3];
        for (int i = 0; i < tempSprite.Length; i++)
        {
            // 위
            if (tempSprite[i].name.Equals("Link_IU"))
            {
                IdleSprite[0] = tempSprite[i];
            }
            // 아래
            else if (tempSprite[i].name.Equals("Link_ID"))
            {
                IdleSprite[1] = tempSprite[i];
            }
            // 오른쪽
            else if (tempSprite[i].name.Equals("Link_IR"))
            {
                IdleSprite[2] = tempSprite[i];
            }
        }
        // 오류 체크
        for (int i = 0; i < IdleSprite.Length; i++)
        {
            if (IdleSprite[i] == null)
            {
                Debug.LogError(IdleSprite[i].ToString() + " 못 찾음");
                return;
            }
        }

        PlayerSprite = GetComponentInChildren<SpriteRenderer>();
        if (PlayerSprite == null)
        {
            Debug.LogError("PlayerSprite 못 찾음");
            return;
        }

        ImageTF = transform.Find("Image");
        if (ImageTF == null)
        {
            Debug.LogError("ImageTF 못 찾음");
            return;
        }

        GunScript = GetComponentInChildren<Gun>();
        if(GunScript == null)
        {
            Debug.LogError("GunScript 못 찾음");
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

            // 대각선 이동인지 판별(6과 같은 스프라이트)
            if (Input.GetKey(KeyCode.W)) Way = 9;
            else if (Input.GetKey(KeyCode.S)) Way = 3;
            else Way = 6;

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
                case 8: // 상
                    PlayerAnimator.Play("Idle", -1, 0.9f);
                    break;
                case 2: // 하
                    PlayerAnimator.Play("Idle", -1, 0f);
                    break;
                case 6: // 우
                    PlayerAnimator.Play("Idle", -1, 0.4f);
                    break;
            }
        }
        else if (PrevState == ANI_STATE.Move) // 이동이면 방향값 전달
        {
            PlayerAnimator.SetInteger("way", Way);
        }
    }

    // 피격
    private void OnTriggerEnter(Collider other)
    {
        // 적 총알일 경우
        if (other.tag.Equals("E_Bullet"))
        {
            // 체력 깎임
            HP -= other.GetComponent<E_Bullet>().power;

            // 총알 삭제
            Destroy(other.gameObject);
        }
    }
}
