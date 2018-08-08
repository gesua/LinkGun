using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasterSpawner : MonoBehaviour {
    //플레이어의 위치를 받음
    public Transform player;
    //가스터 블래스터 모드 지정
    public int patState = 1;
    //가스터블래스터 스폰타임
    public float spawnTimePat1 = 1f;
    public float spawnTimePat2 = 0.5f;
    float currTimePat1 = 0f;
    float currTimePat2 = 0f;
    //가스터를받을 팩토리
    public GameObject gasterFactory;
    //범위지정필요
    //플레이어 위치 기준으로 radius로 원형반경 계산하여 그밖으로 띄워줌
    public float spawnRadius = 5f;
    float radius = 0f;
    //Vector3 기준위치
    Vector3 gasterPivot;
    // Use this for initialization
    



    // Update is called once per frame
    void Update() {
        //위치지정
        gasterPivot = new Vector3(player.position.x + (spawnRadius*Mathf.Cos(radius)), player.position.y, player.position.z + (spawnRadius * Mathf.Sin(radius)));
        //스폰타임 넘으면 생성

        switch (patState) {
            case 1:
                //랜덤출현
                SpawnPat1();
                break;
            case 2:
                //빙글빙글
                SpawnPat2();
                break;
            case 3:
                break;

        }
        
    }

    void SpawnPat1() {
        
        currTimePat1 += Time.deltaTime;
        if (currTimePat1 > spawnTimePat1) {
            GameObject tempGaster = Instantiate(gasterFactory);
            tempGaster.transform.position = gasterPivot;
            tempGaster.transform.LookAt(player.transform);
            radius += Random.Range(0, 360);
            currTimePat1 = 0f;
        }
    }

    void SpawnPat2() {
        currTimePat2 += Time.deltaTime;
        if (currTimePat2 > spawnTimePat2) {
            GameObject tempGaster = Instantiate(gasterFactory);
            tempGaster.transform.position = gasterPivot;
            tempGaster.transform.LookAt(player.transform);
            radius += 0.3f;
            currTimePat2 = 0f;
        }
    }

}
