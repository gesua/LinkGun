using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Bullet : MonoBehaviour {
    //총알스피드
    public float bulletSpeed = 1f;
    //타겟위치 ->동적으로변경(프리팹)
    //Transform target;
    //위치계산
    Vector3 dir;
    //총알공격력
    public int power = 1;
    //거리 계산해서 삭제
    //시간 계산해서 삭제
    public float surviveTime = 5f;
    float currTime = 0f;
    //총알은 앞으로 날아감
	// Use this for initialization
	void Start () {
        //동적할당

        //방향계산
        dir = this.transform.forward;
        //dir.Normalize();
        //총알방향전환 //전환필요없어짐(스폰에넘김)
        //this.transform.LookAt(target);
	}
	
	// Update is called once per frame
	void Update () {
        currTime += Time.deltaTime;
        
        //총알 앞으로 날리기
        this.transform.position += dir * bulletSpeed * Time.deltaTime;

      

	}
}
