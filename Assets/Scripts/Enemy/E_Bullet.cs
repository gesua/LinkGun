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

    //총알 넣어줄 Spawner객체
    BulletSpawner bulletSpawner;
    // Use this for initialization

    private void OnEnable() {
        // 일단 전부 끔
        CancelInvoke();

        // 3초 뒤 사라짐
        Invoke("Off", 7f);
    }
    void Start () {
        //동적할당
      
        //방향계산
        //dir.Normalize();
        //총알방향전환 //전환필요없어짐(스폰에넘김)
        //this.transform.LookAt(target);
	}
	
	// Update is called once per frame
	void Update () {
        currTime += Time.deltaTime;
        
        dir = this.transform.forward;
        //총알 앞으로 날리기
        this.transform.position += dir * bulletSpeed * Time.deltaTime;

	}
    
    public void setSpawner(BulletSpawner spawner) {
        bulletSpawner = spawner;
    }

    // 끄고 Pool에 넣음
    public void Off() {
        gameObject.SetActive(false);
        bulletSpawner.AddBulletPool(gameObject);
    }
}
