using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//보스의 이동경로지정
//기본으로 키입력을 받아서 이동하기
//캐릭터의 기본 스탯


public class Enemy : MonoBehaviour {
    //이동애니메이션표현
    enum ANI_STATE {
        E_IDLE,
        E_MU,
        E_MD,
        E_ML
    }
    ANI_STATE aniState = ANI_STATE.E_IDLE;
    //애니메이터
    Animator EnemyAnimator;
    //캐릭터의 방향을 체크해줄 값
    bool checkRight = false;

    public float moveSpeed = 1f;
    public int HP = 100;
    //플레이어의 위치를 받을값
    public Transform target;
    //플레이어의방향계산
    Vector3 dir;
    //패턴시간을추가해야함
    //사용시간
    float currTime = 0f;
    //패턴종류
    public int moveState = 1;
    ////////////////패턴1 -> 플레이어의 위치를 지정시간마다 따라감
    public float maxTime1 = 2f;
    Vector3 tempDir;
    Vector3 tempPos;

    ////////////////패턴2 ->지정방향으로 순간이동,순간이동지정시에 플레이어 위치 주변반경 일정범위 내에 랜덤으로 나옴
    public float maxTime2 = 1.5f;
    //사라졌다가 다시 나오는대기시간
    public float respawnTime = 4.0f;


    // Use this for initialization
    void Start() {
        //할당
        EnemyAnimator = gameObject.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update() {
        //무브상태체크
        Move();
        //보스의이동
        switch (moveState) {
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
        //애니메이터
        PlayAnimator();
    }
    void Move() {
        //방향계산
        dir = target.position - this.transform.position;
        dir.Normalize();
        Debug.Log(dir);
        if (dir.x >= 0.9f || dir.x <= -0.9f) {
            Debug.Log("이런");
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
        //방향계산후 보스의 애니메이션 지정
    }
    void MovePattern1() {
        //임시방향

        //패턴1의 시간 계산
        currTime += Time.deltaTime;
        //시간이 0일때만 dir의 방향을 받음
        if (currTime <= 0.5) {
            tempPos = target.transform.position;
            tempDir = tempPos - this.transform.position;
        }
        if (currTime > maxTime1) {
            this.transform.position += tempDir.normalized * moveSpeed * Time.deltaTime;
            if ((this.transform.position.x - 1 < tempPos.x && this.transform.position.x + 1 > tempPos.x) && (this.transform.position.z - 1 < tempPos.z && this.transform.position.z + 1 > tempPos.z)) {
                currTime = 0;
                Debug.Log("위치재선정");
            }

        }
    }
    void MovePattern2() {
        //텔레포트
        //임시지정
        //애니메이션 필요(방향지정,슈웅사라짐)
        currTime += Time.deltaTime;
        if (currTime > maxTime2) {
            //해당위치에 나올지점 이펙트 찍어주기
            Debug.Log("순간이동시전");
            if (currTime > respawnTime) {
                Debug.Log("순간이동대기시간끝,완료");
                this.transform.position = new Vector3(target.position.x + Random.Range(-2, 2), -10, target.position.z + Random.Range(-2, 2));
                currTime = 0;
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

        }

    }
    private void OnTriggerEnter(Collider other) {
        if (other.tag.Equals("P_Bullet")) {
            Damage(other.GetComponent<P_Bullet>().power);
            Destroy(other.gameObject);
        }
    }

   
    //데미지받는것
    public void Damage(int power) {
        this.HP -= power;
    }
}