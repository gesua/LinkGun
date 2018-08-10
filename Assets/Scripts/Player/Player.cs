using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // 체력
    int HP = 3; // 잔기
    Image[] HP_Sprite; // HP 이미지 위치
    Sprite DamageHPSprite; // 데미지 입은 HP 이미지

    // 폭탄
    int BombCount = 2; // 갯수
    Image[] BombImage; // 폭탄 이미지 위치
    SpriteRenderer BombSR; // 폭탄 랜더러
    Sprite[] BombSprite; // 폭탄 스프라이트

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

    // 블링크
    bool IsBlinked = false; // 블링크 중인지
    public float BlinkDelayTime = 0.5f; // 후 딜레이
    float BlinkDelayCount = 0; // 세는거
    Vector3 BlinkDir; // 블링크 방향
    public float BlinkDistance = 1; // 블링크 거리
    GameObject BlinkAfterimagePrefab; // 블링크 이펙트 프리팹

    Transform MousePoint; // 마우스 위치

    public bool GodMode = false; // 무적 모드

    GameObject HitPoint; // 피격점

    Gun GunScript;
    DamagedBlink DamageBlink;

    Rigidbody Rigid; // 리지드바디

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

        // HP 위치 찾기
        Transform tempTF = GameObject.Find("PlayerHP").transform;
        HP_Sprite = tempTF.GetComponentsInChildren<Image>();
        if (HP_Sprite == null)
        {
            Debug.LogError("HP_Sprite 못 찾음");
            return;
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
            Debug.LogError("DamageHPImage 못 찾음");
            return;
        }

        // 데미지 받았을 때 깜빡이는 스크립트
        DamageBlink = GetComponent<DamagedBlink>();
        if (DamageBlink == null)
        {
            Debug.LogError("Blink 못 찾음");
            return;
        }

        // 폭탄 위치 찾기
        tempTF = GameObject.Find("PlayerBomb").transform;
        BombImage = tempTF.GetComponentsInChildren<Image>();
        if (BombImage == null)
        {
            Debug.LogError("BombSprite 못 찾음");
            return;
        }

        // 폭탄 터지는 위치 찾기
        BombSR = transform.Find("Bomb").GetComponent<SpriteRenderer>();
        if (BombSR == null)
        {
            Debug.LogError("BombPos 못 찾음");
            return;
        }

        // 폭탄 스프라이트 가져옴
        BombSprite = Resources.LoadAll<Sprite>("Sprites/BombEffect");
        if (BombSprite == null)
        {
            Debug.LogError("BombSprite 못 찾음");
            return;
        }

        // 마우스 위치
        MousePoint = GameObject.Find("MousePoint").transform;
        if (MousePoint == null)
        {
            Debug.LogError("MousePoint 못 찾음");
            return;
        }

        // 리지드바디
        Rigid = GetComponent<Rigidbody>();
        if (Rigid == null)
        {
            Debug.LogError("Rigid 못 찾음");
            return;
        }

        // 블링크 효과 프리팹
        BlinkAfterimagePrefab = Resources.Load<GameObject>("Prefabs/BlinkAfterimage");
        if (BlinkAfterimagePrefab == null)
        {
            Debug.LogError("BlinkAfterimagePrefab 못 찾음");
            return;
        }

        // 피격점
        HitPoint = transform.Find("HitPoint").GetChild(0).gameObject;
        if (HitPoint == null)
        {
            Debug.LogError("HitPointTF 못 찾음");
            return;
        }

        // 총 위치 초기화
        Way = 2;
        GunScript.SetPosition(Way);
    }

    void Update()
    {
        // 리지드바디 안 튕기게
        if (Rigid.velocity != Vector3.zero)
        {
            Rigid.velocity = Vector3.zero;
        }

        // 이동
        if (IsBlinked == false && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
        {
            NextState = ANI_STATE.Move;
            Move();

            // 블링크(우클릭)
            if (Input.GetMouseButtonDown(1))
            {
                BlinkUse();
            }
        }
        else // 기본
        {
            NextState = ANI_STATE.Idle;

            // 블링크 딜레이
            if (IsBlinked)
            {
                Blink();
            }
        }

        // 폭탄
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (BombCount > 0)
            {
                Bomb();
            }
        }

        // 애니메이터
        PlayAnimator();

        // 무적시간 계산
        if (InvincibleTimeCount != 0)
        {
            Invincible();
        }

        // 피격점 보여줄지 말지
        HitPointCheck();
    }

    // 이동(WASD)
    void Move()
    {
        Vector3 moveVector = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            moveVector.z += Speed * Time.deltaTime;
            Way = 8;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moveVector.z -= Speed * Time.deltaTime;
            Way = 2;
        }


        if (Input.GetKey(KeyCode.A))
        {
            moveVector.x -= Speed * Time.deltaTime;

            // 대각선 이동인지 판별
            if (Input.GetKey(KeyCode.W)) Way = 7;
            else if (Input.GetKey(KeyCode.S)) Way = 1;
            else Way = 4;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveVector.x += Speed * Time.deltaTime;

            // 대각선 이동인지 판별
            if (Input.GetKey(KeyCode.W)) Way = 9;
            else if (Input.GetKey(KeyCode.S)) Way = 3;
            else Way = 6;
        }

        // 이미지 반전
        if (Way == 1 || Way == 4 || Way == 7) // 왼쪽
        {
            // 이미지 좌우반전
            if (ImageTF.localScale.x > 0)
            {
                Vector3 temp = ImageTF.localScale;
                temp.x *= -1;
                ImageTF.localScale = temp;
            }
        }
        else // 오른쪽
        {
            // 이미지 좌우반전
            if (ImageTF.localScale.x < 0)
            {
                Vector3 temp = ImageTF.localScale;
                temp.x *= -1;
                ImageTF.localScale = temp;
            }
        }

        // 장애물과 충돌했는지
        Ray ray = new Ray(transform.position, moveVector.normalized);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, moveVector.magnitude * 2, 1 << 9))
        {
            transform.position = hitInfo.point - (moveVector.normalized * moveVector.magnitude * 1.1f);
        }
        else
        {
            // 움직임 계산
            transform.position += moveVector;
        }

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
        if (InvincibleTimeCount != 0) return; // 무적시간

        // 적 총알일 경우
        if (other.tag.Equals("E_Bullet"))
        {
            // 체력 깎임
            if (GodMode == false)
            {
                HP--;
            }

            // 깜빡임
            DamageBlink.BlinkStart();

            // 무적 시간 시작
            InvincibleTimeCount += Time.deltaTime;

            // 이미지 변경
            if (GodMode == false)
            {
                HP_Sprite[HP].sprite = DamageHPSprite;
            }

            // 죽음
            if (HP <= 0)
            {
                ResultManager.Instance.GameSet(false);
            }
        }
    }

    // 무적 시간 세기
    void Invincible()
    {
        InvincibleTimeCount += Time.deltaTime;

        // 다 셌음
        if (InvincibleTimeCount >= InvincibleTime)
        {
            InvincibleTimeCount = 0;
        }
    }

    // 블링크 시작
    void BlinkUse()
    {
        BlinkDir = Vector3.zero;

        // 키보드 방향으로 블링크
        switch (Way)
        {
            case 1: // ↙
                BlinkDir = Vector3.left + Vector3.back;
                break;
            case 2: // ↓
                BlinkDir = Vector3.back;
                break;
            case 3: // ↘
                BlinkDir = Vector3.right + Vector3.back;
                break;
            case 4: // ←
                BlinkDir = Vector3.left;
                break;
            case 6: // →
                BlinkDir = Vector3.right;
                break;
            case 7: // ↖
                BlinkDir = Vector3.left + Vector3.forward;
                break;
            case 8: // ↑
                BlinkDir = Vector3.forward;
                break;
            case 9: // ↗
                BlinkDir = Vector3.right + Vector3.forward;
                break;
        }


        BlinkDir *= BlinkDistance; // 거리 증가

        // 블링크 사용
        if (BlinkDir != Vector3.zero)
        {
            Vector3 tempPos = transform.position;

            // 장애물과 충돌했는지
            Ray ray = new Ray(transform.position, BlinkDir.normalized);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, BlinkDir.magnitude, 1 << 9))
            {
                transform.position = hitInfo.point;// + (BlinkDir.normalized / 100f);
            }
            else
            {
                // 움직임 계산
                transform.position += BlinkDir;
            }

            // 블링크 잔상 생성
            CreateBlinkAfterimage(tempPos, transform.position);

            IsBlinked = true;
        }
    }

    // 블링크
    void Blink()
    {
        // 블링크 딜레이 시작
        BlinkDelayCount += Time.deltaTime;

        // 다 셌음
        if (BlinkDelayCount >= BlinkDelayTime)
        {
            BlinkDelayCount = 0;
            IsBlinked = false;
        }
    }

    // 블링크 잔상 생성(현재 거리 1에 맞춰져있음)
    void CreateBlinkAfterimage(Vector3 prevPos, Vector3 nextPos)
    {
        GameObject temp = Instantiate(BlinkAfterimagePrefab);
        temp.transform.position = prevPos;
        temp.GetComponent<BlinkAfterimage>().SetInitialization((prevPos - nextPos).magnitude, BlinkDelayTime); // 블링크 이펙트 설정(이미지 위치, 사라지는 시간)

        temp.transform.LookAt(nextPos);

        // x축 회전 없앰
        Vector3 tempAngle = temp.transform.eulerAngles;
        tempAngle.x = 0;
        temp.transform.eulerAngles = tempAngle;
    }

    // 폭탄
    void Bomb()
    {
        BombCount--;
        BombImage[BombCount].enabled = false;

        BulletSpawner.Instance.AllBulletDisable();
        GasterSpawner.Instance.AllGasterDisable();
        LaserSpawner.Instance.AllLaserDisable();

        StartCoroutine("BombEffect");
    }

    // 이펙트 이미지
    IEnumerator BombEffect()
    {
        BombSR.gameObject.SetActive(true);

        for (int i = 0; i < BombSprite.Length; i++)
        {
            BombSR.sprite = BombSprite[i];
            BombSR.transform.localScale = new Vector3(i, i, 1); // 크기 커지게
            yield return new WaitForSeconds(0.01f);
        }

        BombSR.gameObject.SetActive(false);
    }

    // 피격점 보여줄지 말지
    void HitPointCheck()
    {
        // 총알이 근처에 있으면 피격점 보여줌
        Collider[] temp = Physics.OverlapSphere(transform.position, 1f, 1 << 8);

        if (temp.Length > 0)
        {
            if (HitPoint.activeSelf == false)
            {
                HitPoint.SetActive(true);
            }
        }
        else if (HitPoint.activeSelf == true)
        {
            HitPoint.SetActive(false);
        }
    }
}
