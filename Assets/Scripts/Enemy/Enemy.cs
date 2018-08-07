using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//보스의 이동경로지정
//기본으로 키입력을 받아서 이동하기
//캐릭터의 기본 스탯


public class Enemy : MonoBehaviour {
    //이동속도및체력
    public float moveSpeed = 1f;
    public int HP = 100;

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
    //플레이어의 공격 시간 체크
    public float attackDelay = 1.5f;
    //공격지속시간(attakDelay뺀시간)
    public float attackContinuousTime = 6f;
    float currADTime = 0f;
    //공격상태체크
    bool attackState = false;
    //이동상태체크
    bool moveState = true;
    BulletSpawner bulletSpawner;
    //캐릭터의 방향을 체크해줄 값
    bool checkRight = false;
    //플레이어의 위치를 받을값
    public Transform target;
    //플레이어의방향계산
    Vector3 dir;
    //사용시간
    float currTime = 0f;
    //패턴종류
    //패턴종류랜덤지정
    int setRandomMove = 0;
    public int movePattern = 0;
    ////////////////패턴1 -> 플레이어의 위치를 지정시간마다 따라감
    //public float maxTime1 = 2f;
    Vector3 tempDir;
    Vector3 tempPos;

    ////////////////패턴2 ->지정방향으로 순간이동,순간이동지정시에 플레이어 위치 주변반경 일정범위 내에 랜덤으로 나옴
    public float maxTime2 = 1.5f;
    //사라졌다가 다시 나오는대기시간
    public float respawnTime = 4.0f;
    bool tempTel = false;
    Vector3 tempTelPos = Vector3.zero;

    //보스깜빡임데미지표현
    DamagedBlink blink;

    //보스HP바계산
    Image BossHp;

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

    // Use this for initialization
    void Start() {
        //할당
        EnemyAnimator = gameObject.GetComponentInChildren<Animator>();
        blink = gameObject.GetComponent<DamagedBlink>();
        BossHp = GameObject.Find("Bar").GetComponentInChildren<Image>();
        bulletSpawner = GameObject.Find("E_BulletSpawner").GetComponent<BulletSpawner>();
    }

    // Update is called once per frame
    void Update() {

        //행동지정
        SetEnemyBehavState();
        //공격패턴지정
        SetAttackPattern();
        //무브패턴지정
        SetMovePattern();


        //애니메이터
        PlayAnimator();
    }

    void SetEnemyBehavState() {
        //무브상태체크
        if (currADTime > attackDelay) {
            //공격시간재기
            currADTime += Time.deltaTime;
            //상태변경
            attackState = true;
            moveState = false;
            if (attackState) {
                aniState = ANI_STATE.E_AP1;
                //bulletSpawner.attackState = this.attackState;
            }
            if (currADTime > attackContinuousTime) {
                //시간끝 공격중지
                currADTime = 0;
                attackState = false;
                moveState = true;
                currTime = 0;
                Debug.Log("위치재선정1");
                setRandomMove = Random.Range(0, 100);
                //bulletSpawner.attackState = this.attackState;

                //공격패턴0초기화
                if (HP > 50) {
                    bulletSpawner.bulletState = 0;
                }
                else if (HP <= 50) {
                    bulletSpawner.bulletState = 1;
                }
            }
        }

    }

    void SetMovePattern() {
        if (HP > 12) {
            if (setRandomMove > 50) {
                movePattern = 0;
            }
            else {
                //위치재선정재필요

                movePattern = 1;
            }
        }
        else {
            movePattern = 2;
        }


        if (moveState) {
            currADTime += Time.deltaTime;
            Move();
            //보스의이동
            switch (movePattern) {
                case 0:
                    //플레이어따라감
                    this.transform.position += dir * moveSpeed * Time.deltaTime;
                    break;
                case 1:
                    MovePattern1();
                    break;

                case 2:
                    MovePattern2();
                    break;

            }
        }
    }
    void Move() {
        //방향계산
        dir = target.position - this.transform.position;
        dir.Normalize();
        //Debug.Log(dir);
        if (dir.x >= 0.9f || dir.x <= -0.9f) {
            //Debug.Log("이런");
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
        if (HP > 50) {
            if (currPatCheck < atPat1Check) {
                if (onDamagedCount > CheckPat1Dam) {
                    bulletSpawner.bulletState = 1;
                }
            }
            else {
                onDamagedCount = 0;
                currPatCheck = 0;
            }
        }
        else if (HP <= 50) {
            if (currPatCheck < atPat2Check) {
                if (onDamagedCount > CheckPat2Dam) {
                    bulletSpawner.bulletState = 2;
                }
            }
            else {
                onDamagedCount = 0;
                currPatCheck = 0;
            }
        }
        else {
            //공격패턴3,4짬뽕

        }
    }
    void MovePattern1() {
        //패턴1의 시간 계산
        currTime += Time.deltaTime;
        //시간이 0일때만 dir의 방향을 받음
        if (currTime <= 0.5) {
            tempPos = target.transform.position;
            tempDir = tempPos - this.transform.position;
        }

        this.transform.position += tempDir.normalized * moveSpeed * Time.deltaTime;
        if ((this.transform.position.x - 1 < tempPos.x && this.transform.position.x + 1 > tempPos.x) && (this.transform.position.z - 1 < tempPos.z && this.transform.position.z + 1 > tempPos.z)) {
            currTime = 0;
            Debug.Log("위치재선정");
        }

    }
    void MovePattern2() {
        //텔레포트
        //임시지정
        //애니메이션 필요(방향지정,슈웅사라짐)
        currTime += Time.deltaTime;
        if (currTime > maxTime2) {
            //해당위치에 나올지점 이펙트 찍어주기
            if (tempTel == false) {
                tempTelPos = target.position;
                Debug.Log(tempTelPos);
                tempTel = true;
                Debug.Log("순간이동시전,위치 기존이동");
                //순간이동위치에 찍어주기
            }
            if (currTime > respawnTime) {
                Debug.Log("순간이동대기시간끝,완료");
                this.transform.position = tempTelPos;
                currTime = 0;
                tempTel = false;
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
        this.HP -= power;
        BossHp.fillAmount = this.HP / 100f;
        blink.BlinkStart();

        if (HP <= 0)
        {
            //보스hp가 0이하가 되면 ResultManager에서 true호출하여 게임을 종료시킴
            ResultManager.Instance.GameSet(true);
        }
    }

    public void AllStop()
    {
        //총알 멈춰주는 작업이 필요함
        bulletSpawner.AllBulletOff();
        EnemyAnimator.enabled = false;
    }
}