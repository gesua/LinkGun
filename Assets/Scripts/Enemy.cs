using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//보스의 이동경로지정
//기본으로 키입력을 받아서 이동하기
//캐릭터의 기본 스탯


public class Enemy : MonoBehaviour {
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
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {   
        //방향계산
        dir = target.position - this.transform.position;
        dir.Normalize();
        //보스의이동
        //this.transform.position += new Vector3(Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime,0 , Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime);
        switch(moveState) {
            case 0:
                //플레이어따라감
                this.transform.position += dir * moveSpeed * Time.deltaTime;
                break;
            case 1:
                //임시방향
       
                //패턴1의 시간 계산
                currTime += Time.deltaTime;
                //시간이 0일때만 dir의 방향을 받음
                if(currTime <= 0.5) {
                    tempPos = target.transform.position;
                    tempDir = tempPos - this.transform.position;
                }
                if(currTime > maxTime1) {
                    this.transform.position += tempDir.normalized * moveSpeed * Time.deltaTime;
                    if ((this.transform.position.x-1 < tempPos.x && this.transform.position.x + 1 >tempPos.x) && (this.transform.position.z - 1 < tempPos.z && this.transform.position.z + 1 > tempPos.z)) {
                        currTime = 0;
                        Debug.Log("위치재선정");
                    }

                }
                break;

            case 2:
                //텔레포트
                //임시지정
                //애니메이션 필요(방향지정,슈웅사라짐)
                currTime += Time.deltaTime;
                if(currTime > maxTime2) {
                    this.transform.position = new Vector3(target.position.x + Random.Range(-2, 2), -10, target.position.z + Random.Range(-2, 2));
                    currTime = 0;
                }
                
                break;

                
        }
        

    }
    private void OnTriggerEnter(Collider other) {
        if(other.tag.Equals("P_Bullet")) {
            Damage(other.GetComponent<P_Bullet>().power);
            Destroy(other.gameObject);
        }
    }

    //데미지받는것
    public void Damage(int power) {
        this.HP -= power;
    }
}
