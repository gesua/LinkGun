using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoSingleton<BulletSpawner> {
    //불렛 프리팹 받아서 사용
    public GameObject bulletFactory;
    //불렛 오브젝트풀을 만들어 줘야함
    public int poolSize = 500;
    GameObject[] bulletPool;
    List<GameObject> deactiveList = new List<GameObject>();
    //총알의 생성 시간
    public float b_spawnTime = 0.3f;
    //현재시간
    float currTime = 0f;
    //타겟의위치
    public GameObject target;

    //(임시) 총알모드지정
    public int bulletState = 0;

    //공격상태판단
    //public bool attackState = false;

    //////////////////탄막 0번 직선


    //////////////////탄막 1번 부채꼴(갯수지정필요)
    public int bulletCount1 = 3;
    //탄막간 각도
    public float bulletDegree1 = 10f;

    //////////////////탄막 2번 전체(기본)->플레이어위치 기본적으로 따라감0
    public int bulletCount2 = 36;
    public float bulletDegree2 = 10f;

    //////////////////탄막 3번 전체(유도아님,각도 변하면서 흩뿌려줌)
    //스타트각
    public float startDegree3 = 0f;
    //간격차
    public float bulletDegree3 = 0.5f;

    //////////////////탄막 4번 베지어
    //베지어한발씩
    //////////////////탄막 5번 베지어
    //베지어두발씩

    public int bulletCount4 = 18;
    public float bulletDegree4 = 20f;

    

    private void Awake() {
        //싱글톤
        SetInstance(this);
    }

    // Use this for initialization
    void Start() {

        bulletPool = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++) {
            bulletPool[i] = Instantiate(bulletFactory);
            bulletPool[i].AddComponent<E_Bullet>();
            bulletPool[i].GetComponent<E_Bullet>().SetSpawner(this);
            bulletPool[i].SetActive(false);
            //Spawner의자식
            bulletPool[i].transform.parent = GameObject.Find("E_Bullet").transform;
            Destroy(bulletPool[i].GetComponent<E_Bullet>());
            deactiveList.Add(bulletPool[i]);
        }
    }

    // Update is called once per frame
    void Update() {
        this.transform.position = GameObject.Find("Enemy").transform.position;
        //시간 증가

        currTime += Time.deltaTime;
        //시간이 증가할때마다 총알을생성
        //1.시간체크
        if (currTime > b_spawnTime) {

            //2.총알을생성
            //조건분기
            switch (bulletState) {
                case 0:
                    //직선일때

                    ShootMode0();
                    break;
                case 1:
                    //부채꼴일때
                    ShootMode1();
                    break;
                case 2:
                    //전방향발사(기본)
                    ShootMode2();
                    break;
                case 3:
                    //회전샷(방향forWard)
                    //자연스러운 발사시간전환
                    ShootMode3();
                    break;
                case 4:
                    ShootMode4();
                    break;
                case 5:
                    ShootMode5();
                    break;
                default:
                    break;

            }

            //3.시간을초기화
            currTime = 0f;
        }


    }
    void ShootMode0() {
        b_spawnTime = 0.3f;
        if (deactiveList.Count > 0) {
            GameObject bullet = deactiveList[0];
            deactiveList.RemoveAt(0);
            bullet.GetComponent<BulletType>().BType = BulletType.B_Type.Basic;
            bullet.AddComponent<E_Bullet>();
            bullet.SetActive(true);
            bullet.transform.position = this.transform.position;
            bullet.transform.LookAt(target.transform);
        }

    }
    void ShootMode1() {
        b_spawnTime = 0.3f;
        for (int i = 0; i < bulletCount1; i++) {
            //방향백터값 계산필요
            Vector3 dir = target.transform.position - this.transform.position;
            //방향백터 정규화
            dir.Normalize();
            //코사인값의 세타값을 구해줘야 하므로 x값(x/R)을 넣어줘야함
            float inner = Mathf.Acos(dir.x);
            //해당라디안 결과값을 디그리로 바꿔주고 정면이 아니므로 180도를 더해 꺽어줌
            inner = Mathf.Rad2Deg * inner + 180;
            //기본 각도 계산
            float startDegree = -(bulletDegree1 / 2) * (bulletCount1 - 1) + inner;
            float fireDegree = startDegree + (bulletDegree1 * i);
            float radDegree = Mathf.Deg2Rad * fireDegree;
            //X의 각도는 구할수 있으나 Z의값은 양수 음수만 판단해주면 알아서 체크가 가능
            if (target.transform.position.z > this.transform.position.z) {
                radDegree *= -1;
            }
            //해당 각도 (X,Z축)
            Vector3 fireVector = new Vector3(-Mathf.Cos(radDegree), 0, Mathf.Sin(radDegree));
            BulletPoolActive(fireVector,BulletType.B_Type.Nabi);
        }

    }
    void ShootMode2() {
        b_spawnTime = 0.3f;
        for (int i = 0; i < bulletCount2; i++) {
            //방향백터값 계산필요
            Vector3 dir = target.transform.position - this.transform.position;
            //방향백터 정규화
            dir.Normalize();
            //코사인값의 세타값을 구해줘야 하므로 x값(x/R)을 넣어줘야함
            float inner = Mathf.Acos(dir.x);
            //해당라디안 결과값을 디그리로 바꿔주고 정면이 아니므로 180도를 더해 꺽어줌
            inner = Mathf.Rad2Deg * inner + 180;
            //기본 각도 계산
            float startDegree = -(bulletDegree2 / 2) * (bulletCount2 - 1) + inner;
            float fireDegree = startDegree + (bulletDegree2 * i);
            float radDegree = Mathf.Deg2Rad * fireDegree;
            //X의 각도는 구할수 있으나 Z의값은 양수 음수만 판단해주면 알아서 체크가 가능
            if (target.transform.position.z > this.transform.position.z) {
                radDegree *= -1;
            }
            //해당 각도 (X,Z축)
            Vector3 fireVector = new Vector3(-Mathf.Cos(radDegree), 0, Mathf.Sin(radDegree));
            BulletPoolActive(fireVector,BulletType.B_Type.Knife);
        }
    }
    void ShootMode3() {
        b_spawnTime = 0.2f;
        for (int i = 0; i < 12; i++) {
            //각도가 계속 틀어지는 inner값 필요
            //기본 각도 계산
            startDegree3 += bulletDegree3;
            float fireDegree = startDegree3 + (30 * i);
            float radDegree = Mathf.Deg2Rad * fireDegree;
            //해당 각도 (X,Z축)
            Vector3 fireVector = new Vector3(Mathf.Cos(radDegree), 0, Mathf.Sin(radDegree));
            BulletPoolActive(fireVector, BulletType.B_Type.Shuriken);
            if (startDegree3 > 360) {
                startDegree3 = 0;
            }
        }
    }

    void ShootMode4() {
        b_spawnTime = 0.3f;
        for (int i = 0; i < bulletCount4; i++) {
            //방향백터값 계산필요
            Vector3 dir = target.transform.position - this.transform.position;
            //방향백터 정규화
            dir.Normalize();
            //코사인값의 세타값을 구해줘야 하므로 x값(x/R)을 넣어줘야함
            float inner = Mathf.Acos(dir.x);
            //해당라디안 결과값을 디그리로 바꿔주고 정면이 아니므로 180도를 더해 꺽어줌
            inner = Mathf.Rad2Deg * inner + 180;
            //기본 각도 계산
            float startDegree = -(bulletDegree4 / 2) * (bulletCount4 - 1) + inner;
            float fireDegree = startDegree + (bulletDegree4 * i);
            if (target.transform.position.z > this.transform.position.z) {
                fireDegree *= -1;
            }
            //해당 각도 (X,Z축)
            BulletPoolActiveBezier(fireDegree, BulletType.B_Type.Basic, true);
        }
    }

    void ShootMode5() {
        b_spawnTime = 0.3f;
        for (int i = 0; i < bulletCount4; i++) {
            //방향백터값 계산필요
            Vector3 dir = target.transform.position - this.transform.position;
            //방향백터 정규화
            dir.Normalize();
            //코사인값의 세타값을 구해줘야 하므로 x값(x/R)을 넣어줘야함
            float inner = Mathf.Acos(dir.x);
            //해당라디안 결과값을 디그리로 바꿔주고 정면이 아니므로 180도를 더해 꺽어줌
            inner = Mathf.Rad2Deg * inner + 180;
            //기본 각도 계산
            float startDegree = -(bulletDegree4 / 2) * (bulletCount4 - 1) + inner;
            float fireDegree = startDegree + (bulletDegree4 * i);
            if (target.transform.position.z > this.transform.position.z) {
                fireDegree *= -1;
            }
            for(int j = 0; j < 2; j++) {
                //해당 각도 (X,Z축)
                if (j == 0) {
                    BulletPoolActiveBezier(fireDegree, BulletType.B_Type.Knife, true);
                } else {
                    BulletPoolActiveBezier(fireDegree, BulletType.B_Type.Knife, false);
                }
            }
        }
    }


    void BulletPoolActive(Vector3 fireVector, BulletType.B_Type type) {
        GameObject tempBullet = deactiveList[0];
        deactiveList.RemoveAt(0);
        //타입지정
        tempBullet.GetComponent<BulletType>().BType = type;
        tempBullet.AddComponent<E_Bullet>();
        tempBullet.SetActive(true);
        tempBullet.transform.position = this.transform.position;
        tempBullet.transform.rotation = Quaternion.LookRotation(fireVector);
    }

    //베지어곡선생성
    void BulletPoolActiveBezier(float fireDeg, BulletType.B_Type type,bool dirRight) {
        GameObject tempBullet = deactiveList[0];
        deactiveList.RemoveAt(0);
        tempBullet.GetComponent<BulletType>().BType = type;
        tempBullet.AddComponent<E_Bullet_Bezier>();
        tempBullet.GetComponent<E_Bullet_Bezier>().dirRight = dirRight;
        tempBullet.SetActive(true);
        tempBullet.transform.position = this.transform.position;
        tempBullet.GetComponent<E_Bullet_Bezier>().spawnerPos = this.transform;
        tempBullet.GetComponent<E_Bullet_Bezier>().fireDegree = fireDeg;

    }

    public void AddBulletPool(GameObject bullet) {
        deactiveList.Add(bullet);
        Destroy(bullet.GetComponent<E_Bullet>());
    }

    public void AllBulletOff() {
        for (int i = 0; i < poolSize; i++) {
            bulletPool[i].GetComponent<BulletType>().StopAnimation();
            Destroy(bulletPool[i].GetComponent<E_Bullet>());
        }
    }
    public void AllBulletDisable() {
        //Bomb
        for (int i = 0; i < poolSize; i++) {
            bulletPool[i].SetActive(false);
            AddBulletPool(bulletPool[i]);
            Destroy(bulletPool[i].GetComponent<E_Bullet>());
        }
    }

}
