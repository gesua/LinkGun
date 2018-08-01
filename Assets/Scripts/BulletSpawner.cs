using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour {
    //불렛 프리팹 받아서 사용
    public GameObject bulletFactory;
    //총알의 생성 시간
    public float b_spawnTime = 0.3f;
    //현재시간
    float currTime = 0f;

    //(임시) 총알모드지정
    public int bulletState = 0;
    //////////////////탄막 0번 직선


    //////////////////탄막 1번 부채꼴(갯수지정필요)
    public int bulletCount = 3;
    //탄막간 각도
    public float bulletDegree = 10f;



    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
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
                    GameObject bullet = Instantiate(bulletFactory, this.transform.position, Quaternion.identity);
                    break;
                case 1:
                    //부채꼴일때
                    for (int i = 0;  i < bulletCount; i++) {
                        
                    }
                    break;

                default:
                    break;

            }

            //3.시간을초기화
            currTime = 0f;
        }





    }
}
