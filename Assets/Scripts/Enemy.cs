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
    public int moveState = 1;
    ////////////////패턴1 -> 플레이어의 위치를 지정시간마다 따라감
    public float maxTime = 2f;
    float currTime = 0f;
    Vector3 tempDir;
    Vector3 tempPos;

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
                if(currTime > maxTime) {
                    this.transform.position += tempDir.normalized * moveSpeed * Time.deltaTime;
                    if ((this.transform.position.x-1 < tempPos.x && this.transform.position.x + 1 >tempPos.x) && (this.transform.position.z - 1 < tempPos.z && this.transform.position.z + 1 > tempPos.z)) {
                        currTime = 0;
                        Debug.Log("위치재선정");
                    }

                }
                break;

                
        }
        

    }

    //데미지받는것
    public void Damage(int power) {
        this.HP -= power;
    }
}
