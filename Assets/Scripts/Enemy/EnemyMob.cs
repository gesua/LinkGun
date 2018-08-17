using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMob : MonoBehaviour {
    //이동속도및 체력
    public float moveSpeed = 1f; //이동속도
    public int MaxHP = 50; //최대체력
    public int CurrHP = 0; //현재체력
    Transform target; //플레이어의 위치를 받을값
    bool checkRight = false; //오른쪽체크
    NavMeshAgent agent; //네비
    Rigidbody rigid; //충돌체 밀림방지
    Vector3 dir;//플레이어와의방향

    //데미지깜빡임표현
    DamagedBlink blink;

    //공격상태구분
    bool attackState = false;
    public float attackContinuousTime = 3f;
    public float attackDelayTime = 2.5f;
    float currTime = 0f;
    public float b_spawnTime = 0.3f; //총알발사딜레이
    float b_currTime = 0f;

    //이펙트
    SpriteRenderer mobSprite;
    Sprite[] sprites;

    // Use this for initialization
    void Start() {
        CurrHP = MaxHP; //HP초기화
        //할당
        mobSprite = gameObject.GetComponentInChildren<SpriteRenderer>(); //스프라이트지정
        SetDeadSprite();
        target = GameObject.Find("Player").transform;//캐릭터위치
        agent = gameObject.GetComponent<NavMeshAgent>();
        blink = gameObject.GetComponent<DamagedBlink>();
        rigid = gameObject.GetComponent<Rigidbody>(); //충돌체부착
        agent.speed = moveSpeed; //속도초기화
        agent.SetDestination(target.transform.position);//위치지정
    }

    // Update is called once per frame
    void Update() {
        currTime += Time.deltaTime;//시간계산
        this.transform.rotation = Quaternion.Euler(0, 0, 0); //회전각도를주지않기
        //안튕겨나가게
        if (rigid.velocity != Vector3.zero) {
            rigid.velocity = Vector3.zero;
        }

        MoveDir();//방향전환
        switch (attackState) {
            case true:
                //공격상태일때
                Attack();
                break;
            case false:
                //이동상태일때
                Move();
                break;
        }
    }

    void Attack() {
        //공격 지속시간 넘기면 이동상태로전환
        if (currTime > attackContinuousTime) {
            attackState = false;
            currTime = 0f;
            agent.speed = moveSpeed;
        }
        else {
            //공격하기
            b_currTime += Time.deltaTime; //시간계산
            if (b_currTime > b_spawnTime) {
                GameObject bullet = BulletSpawner.Instance.ShootBullet();
                bullet.GetComponent<BulletType>().BType = BulletType.B_Type.Amulet;
                bullet.transform.position = this.transform.position;
                bullet.transform.LookAt(target.transform);
                bullet.SetActive(true);
                b_currTime = 0f;
            }

        }


    }
    void Move() {
        //이동 지속시간 넘기면 이동상태로전환
        if (currTime > attackDelayTime) {
            attackState = true;
            currTime = 0f;
            agent.speed = 0; //속도 0
            agent.velocity = Vector3.zero; //가속도0
        }
        else {
            agent.SetDestination(target.transform.position);//위치지정

        }

    }
    void MoveDir() {
        dir = target.position - this.transform.position;
        dir.Normalize();
        if (dir.x >= 0.9f || dir.x <= -0.9f) {
            //오른쪽인경우
            if (dir.x >= 0) {
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
        }
    }

    public void Damage(int power) {
        this.CurrHP -= power;
        blink.BlinkStart();

        if (CurrHP <= 0) {
            gameObject.GetComponentInChildren<Animator>().enabled = false;
            StartCoroutine("MobDeadEffect");
            agent.speed = 0f;//멈추고
            agent.velocity = Vector3.zero; //가속도0
            gameObject.GetComponent<BoxCollider>().enabled = false;
            gameObject.GetComponent<SphereCollider>().enabled = false;
        }
    }
    IEnumerator MobDeadEffect() {
        Sprite tempSprite = mobSprite.sprite;//현재스프라이트받기(생존중)
        mobSprite.sprite = sprites[0];
        for (int i = 1; i < sprites.Length; i++) {
            yield return new WaitForSeconds(0.15f);
            mobSprite.sprite = sprites[i];
        }
        yield return new WaitForSeconds(0.1f);
        MobSpawner.Instance.AddMobPool(gameObject);
        mobSprite.sprite = tempSprite;
    }

    void SetDeadSprite() {
        Sprite[] tempSprite = Resources.LoadAll<Sprite>("Sprites/YukDead");
        int DeadCount = 0;
        sprites = new Sprite[3];
        for (int i = 0; i < tempSprite.Length; i++) {
            if (tempSprite[i].name.Contains("Yuk")) {
                sprites[DeadCount++] = tempSprite[i];
            }
        }
    }
}
