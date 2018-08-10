using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

//보스의 이동경로지정
//기본으로 키입력을 받아서 이동하기
//캐릭터의 기본 스탯


public class Enemy : MonoBehaviour {
    //이동속도및체력
    public float moveSpeed = 1f;
    public int MaxHP = 100;
    public int CurrHP;

    //이동애니메이션표현
    enum ANI_STATE {
        E_IDLE,
        E_MU,
        E_MD,
        E_ML,
        E_AP1,
        E_AP2
    }
    ANI_STATE aniState = ANI_STATE.E_IDLE;
    //애니메이터
    Animator EnemyAnimator;

    /// <summary>
    /// //////////////////////////////////////////////////////////////이동관련
    
    //캐릭터의 방향을 체크해줄 값
    bool checkRight = false;
    //네비게이션
    NavMeshAgent agent;
    //플레이어의 위치를 받을값
    public Transform target;
    //플레이어의방향계산
    Vector3 dir;
    //상태판단
    bool attackState = false;
    //공격지속시간
    public float attackContinuousTime = 3f;
    //공격시간
    float currAtkTime = 0f;
    //이동시간(공격딜레이)
    public float attackDelayTime = 2.5f;
    float currAtkDyTime = 0f;

    //패턴종류
    //패턴종류랜덤지정
    int setRandomMove = 0;
    public int movePattern = 0;
    ////////////////패턴1 -> 플레이어의 위치를 지정시간마다 따라감
    //사용시간
    float currTimeMov1 = 0f;
    Vector3 tempDir;
    Vector3 tempPos;

    ////////////////패턴2 ->지정방향으로 순간이동,순간이동지정시에 플레이어 위치 주변반경 일정범위 내에 랜덤으로 나옴
    //사용시간
    float currTimeMov2 = 0f;
    public float Mov2SetTime = 1.0f;
    //사라졌다가 다시 나오는대기시간
    public float respawnTime = 1f;
    bool tempTel = false;
    Vector3 tempTelPos = Vector3.zero;
    /// </summary>


    /// <summary>
    /// //////////////////////////////////////////////////////////////공격관련
    ////////////////공격패턴위한 수치값
    public float currPatCheck = 0f;
    int onDamagedCount = 0;
    //공격패턴1(부채꼴) 발사조건 -> ~ 초내에 5이상을 맞았을경우
    public float atPat1Check = 1.5f;
    public int CheckPat1Dam = 5;
    //공격패턴2(원형) 발사조건 -> ~ 초내에 5발 이상 맞았을경우(체력50퍼센트이하)
    //50퍼센트 이하일땐 기본 공격이 부채꼴로 바뀜
    public float atPat2Check = 1.5f;
    public int CheckPat2Dam = 5;
    //공격패턴3 베지어곡선

    //공격패턴4(돌리기) 발사조건 -> 체력%이하


    /// <summary>

    //이펙트
    GameObject teleport;
    //스프라이트이미지
    SpriteRenderer teleportSprite;
    Sprite[] telSprite;


    //보스깜빡임데미지표현
    DamagedBlink blink;

    //보스HP바계산
    Image BossHp;

    //충돌체밀림방지
    Rigidbody rigid;

    // Use this for initialization
    void Start() {
        //네비
        agent = gameObject.GetComponent<NavMeshAgent>();
        //체력
        CurrHP = MaxHP;
        //할당
        EnemyAnimator = gameObject.GetComponentInChildren<Animator>();
        blink = gameObject.GetComponent<DamagedBlink>();
        BossHp = GameObject.Find("Bar").GetComponentInChildren<Image>();
        //bulletSpawner = GameObject.Find("E_BulletSpawner").GetComponent<BulletSpawner>();
        teleport = GameObject.Find("Teleport");
        //스프라이트지정
        teleportSprite = teleport.GetComponent<SpriteRenderer>();
        teleportSprite.enabled = false;
        SetTeleportSprite();
     
        //안튕기게하기
        rigid = gameObject.GetComponent<Rigidbody>();
        //속도초기화
        agent.speed = moveSpeed;
    }

    // Update is called once per frame
    void Update() {
        //안튕겨나가게
        if (rigid.velocity != Vector3.zero) {
            rigid.velocity = Vector3.zero;
        }
        //안돌리기
        this.transform.rotation = Quaternion.Euler(0, 0, 0);
        //시간재기
        if (attackState) {
            //공격중
            currAtkTime += Time.deltaTime;
        } else {
            currAtkDyTime += Time.deltaTime;
        }
        //공격상태 이동상태 구분
        if (currAtkTime > attackContinuousTime) {
            //공격시간지나서끝남
            attackState = false;
            currAtkTime = 0f;
            //이동상태(0,1)일때 랜덤지정
            setRandomMove = Random.Range(0, 100);
            //이동속도초기화
            agent.speed = moveSpeed;
        }
        if (currAtkDyTime > attackDelayTime) {
            //공격딜레이(이동) 끝나서 공격전환
            attackState = true;
            currAtkDyTime = 0f;
            //이동패턴1시간초기화
            currTimeMov1 = 0f;
            currTimeMov2 = 0f;
            //네비게이션 밀리는것 없애기
            agent.speed = 0f;
            agent.velocity = Vector3.zero;
            
        }

        //상태판단
        switch (attackState) {
            case true:
                SetAttackPattern();
                break;
            case false:
                SetMovePattern();
                break;
        }

        //애니메이터
        PlayAnimator();

    }


    void SetMovePattern() {
        //체력따라 정해주는 패턴
        if (CurrHP > MaxHP*0.12f) {
            if (setRandomMove > 50) {
                movePattern = 0;
            }
            else {
                //위치재선정재필요
                movePattern = 1;
            }
        }
        else {
            agent.destination = Vector3.zero;
            attackDelayTime = respawnTime + Mov2SetTime;
            movePattern = 2;
        }

        Move();

        //보스의이동
        switch (movePattern) {
            case 0:
                //네비게이션
                //플레이어따라감
                agent.destination = target.position;
                break;
            case 1:
                MovePattern1();
                break;

            case 2:
                MovePattern2();
                break;

        }

    }

    void Move() {
        //방향계산
        dir = target.position - this.transform.position;
        dir.Normalize();

        if (dir.x >= 0.9f || dir.x <= -0.9f) {

            //오른쪽인경우
            if (dir.x >= 0) {
                aniState = ANI_STATE.E_ML;
                checkRight = true;
            }
            else {
                aniState = ANI_STATE.E_ML;
                //왼쪽인경우
                checkRight = false;
            }
        }
        else {
            if (dir.z > 0) {
                //위로
                aniState = ANI_STATE.E_MU;
            }
            else if (dir.z <= 0) {
                //아래로
                aniState = ANI_STATE.E_MD;
            }
        }
    }

    void SetAttackPattern() {
        currPatCheck += Time.deltaTime;
        //공격패턴판단(데미지받은것기준)
        //패턴1기준
        if (CurrHP > MaxHP * 0.50) {
            if (currPatCheck < atPat1Check) {
                //데미지축적치이상일때
                if (onDamagedCount > CheckPat1Dam) {
                    //부채꼴
                    BulletSpawner.Instance.bulletState = 1;
                    
                } else {
                    //직선
                    BulletSpawner.Instance.bulletState = 0;
                }
            } else {
                onDamagedCount = 0;
                currPatCheck = 0;
            }
            //애니메이션1
            aniState = ANI_STATE.E_AP1;
        } else if (CurrHP <= MaxHP * 0.50 && CurrHP > MaxHP * 0.12) {
            if (currPatCheck < atPat2Check) {
                //데미지축적치이상일때
                if (onDamagedCount > CheckPat2Dam) {
                    //원형
                    BulletSpawner.Instance.bulletState = 2;

                } else {
                    //부채꼴
                    BulletSpawner.Instance.bulletState = 1;
                }
            } else {
                onDamagedCount = 0;
                currPatCheck = 0;
            }
            //애니메이션1
            aniState = ANI_STATE.E_AP1;
        } else if (CurrHP <= MaxHP * 0.12) {
            BulletSpawner.Instance.bulletState = 3;
            aniState = ANI_STATE.E_AP2;
        }

    }
    void MovePattern1() {
        //패턴1의 시간 계산
        currTimeMov1 += Time.deltaTime;
        //Debug.Log("위치재선정패턴(무브패턴1)");
        //시간이 0일때만 dir의 방향을 받음
        if (currTimeMov1 <= 0.1) {
            tempPos = target.transform.position;
            //tempDir = tempPos - this.transform.position;
            //Debug.Log("위치재선정1,시간부족");
        }
        agent.destination = tempPos;
        //this.transform.position += tempDir.normalized * moveSpeed * Time.deltaTime;
        if ((this.transform.position.x - 1 < tempPos.x && this.transform.position.x + 1 > tempPos.x) && (this.transform.position.z - 1 < tempPos.z && this.transform.position.z + 1 > tempPos.z)) {
            currTimeMov1 = 0;
            //Debug.Log("위치재선정2,플레이어위치도달");
        }

    }
    void MovePattern2() {
        //텔레포트
        //위치지정시간
        currTimeMov2 += Time.deltaTime;
        if (currTimeMov2 > Mov2SetTime) {
            //해당위치에 나올지점 이펙트 찍어주기
            if (tempTel == false) {
                //지정시간 초기화
                //위치지정
                tempTelPos = target.position;
                //들어오는것제한
                tempTel = true;
                //시간초기화
                currTimeMov2 = 0;
                //순간이동위치에 찍어주기
                teleport.transform.position = tempTelPos;
                teleportSprite.enabled = true;
                StartCoroutine("teleportAlert");
                Debug.Log("순간이동위치지정");
            }
        }
    }
    void PlayAnimator() {
        switch (aniState) {
            case ANI_STATE.E_IDLE:
                EnemyAnimator.SetInteger("State", 0);
                break;
            case ANI_STATE.E_MU:
                EnemyAnimator.SetInteger("State", 1);
                break;
            case ANI_STATE.E_MD:
                EnemyAnimator.SetInteger("State", 2);
                break;
            case ANI_STATE.E_ML:
                EnemyAnimator.SetInteger("State", 3);
                if (checkRight) {
                    if (transform.Find("Image").localScale.x > 0) {
                        Vector3 temp = transform.Find("Image").localScale;
                        temp.x *= -1;
                        transform.Find("Image").localScale = temp;
                    }
                }
                else {
                    if (transform.Find("Image").localScale.x < 0) {
                        Vector3 temp = transform.Find("Image").localScale;
                        temp.x *= -1;
                        transform.Find("Image").localScale = temp;
                    }
                }
                break;
            case ANI_STATE.E_AP1:
                EnemyAnimator.SetInteger("State", 4);
                break;
            case ANI_STATE.E_AP2:
                EnemyAnimator.SetInteger("State", 5);
                break;
        }

    }
    private void OnTriggerEnter(Collider other) {
        if (other.tag.Equals("P_Bullet")) {
            Damage(other.GetComponent<P_Bullet>().power);
            other.GetComponent<P_Bullet>().Hit();
        }
    }

    //데미지받는것
    public void Damage(int power) {
        onDamagedCount += power;
        this.CurrHP -= power;
        BossHp.fillAmount = (float) this.CurrHP / this.MaxHP ;
        blink.BlinkStart();

        if (CurrHP <= 0) {
            //보스hp가 0이하가 되면 ResultManager에서 true호출하여 게임을 종료시킴
            ResultManager.Instance.GameSet(true);
        }
    }

    public void AllStop() {
        //총알 멈춰주는 작업이 필요함
        BulletSpawner.Instance.AllBulletOff();
        GasterSpawner.Instance.AllGasterOff();
        LaserSpawner.Instance.AllLaserOff();
        EnemyAnimator.enabled = false;
        agent.enabled = false;
    }

    void SetTeleportSprite() {
        Sprite[] tempSprite = Resources.LoadAll<Sprite>("Sprites/telEffect");
        int teleportCount = 0;
        telSprite = new Sprite[8];
        for (int i = 0; i < tempSprite.Length; i++) {
            if (tempSprite[i].name.Contains("teleport")) {
                telSprite[teleportCount++] = tempSprite[i];
            }
        }

    }

    
    IEnumerator teleportAlert() {
        teleportSprite.sprite = telSprite[0];
        for (int i = 1; i < telSprite.Length; i++) {
            yield return new WaitForSeconds(respawnTime/telSprite.Length);
            teleportSprite.sprite = telSprite[i];
        }
        yield return new WaitForSeconds(0.15f);
        Debug.Log("순간이동대기시간끝,완료");
        this.transform.position = tempTelPos;
        tempTel = false;
        teleportSprite.enabled = false;

    }
}