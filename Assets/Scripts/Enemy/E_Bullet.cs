using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Bullet : MonoBehaviour {
    //총알스피드
    public float bulletSpeed = 1f;
    //총알스피드임시저장
    float tempSpeed;
    //타겟위치 ->동적으로변경(프리팹)
    //Transform target;
    //위치계산
    Vector3 dir;
    //총알공격력
    public int power = 1;
    //총알은 앞으로 날아감
    //동적할당
    float currTime = 0f;
    //방향계산
    //dir.Normalize();
    //총알방향전환 //전환필요없어짐(스폰에넘김)
    //this.transform.LookAt(target);

    public float surviveTime = 7f;

    //총알 넣어줄 Spawner객체
    BulletSpawner bulletSpawner;
    // Use this for initialization

    // 스프라이트
    SpriteRenderer BulletSR; // 스프라이트 그리는거
    Sprite BulletSprite; // 총알 이미지
    Sprite[] BulletEffect; // 총알 이펙트


    private void Awake() {
        BulletSR = GetComponentInChildren<SpriteRenderer>();
        if (BulletSR == null) {
            Debug.LogError("BulletSR 못 찾음");
            return;
        }

        // 총알 스프라이트
        BulletSprite = BulletSR.sprite;
        if (BulletSprite == null) {
            Debug.LogError("BulletSprite 못 찾음");
            return;
        }

        // 총알 이펙트
        BulletEffect = Resources.LoadAll<Sprite>("Sprites/BulletEffect");
        if (BulletEffect == null) {
            Debug.LogError("BulletEffect 못 찾음");
            return;
        }

        tempSpeed = bulletSpeed;
    }


    private void OnEnable() {
        currTime = 0f;
    }

    // Update is called once per frame
    void Update() {
        Off();
        dir = this.transform.forward;
        //총알 앞으로 날리기
        this.transform.position += dir * bulletSpeed * Time.deltaTime;

    }

    public void SetSpawner(BulletSpawner spawner) {
        bulletSpawner = spawner;
    }

    // 끄고 Pool에 넣음
    public void Off() {
        currTime += Time.deltaTime;
        if (currTime > surviveTime) {
            currTime = 0f;
            gameObject.SetActive(false);
            bulletSpawner.AddBulletPool(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) {
 
        if (other.tag.Equals("Wall") || other.tag.Equals("Player")) {
            gameObject.transform.GetChild(0).localScale = new Vector3(5, 5, 1);
            gameObject.GetComponentInChildren<BulletType>().StopAnimation();
            StartCoroutine("SpriteChange");
            bulletSpeed = 0f;     
        }
        
    }

    // 이펙트로 변경
    IEnumerator SpriteChange() {
        BulletSR.sprite = BulletEffect[0];
        for (int i = 1; i < BulletEffect.Length; i++) {
            yield return new WaitForSeconds(0.05f);
            BulletSR.sprite = BulletEffect[i];
        }

        // 끄기
        gameObject.SetActive(false);

        // 풀에 넣음
        bulletSpawner.AddBulletPool(gameObject);
        //속도복구
        bulletSpeed = tempSpeed;
    }
}
