using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasterSpawner : MonoBehaviour {
    //플레이어의 위치를 받음
    public Transform player;
    //가스터블래스터 스폰타임
    public float spawnTime = 1f;
    float currTime = 0f;
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
        gasterPivot = new Vector3(player.position.x + (spawnRadius*Mathf.Cos(radius)), player.position.y, player.position.z + (spawnRadius * Mathf.Sin(radius)));
        //스폰타임 넘으면 생성
        currTime += Time.deltaTime;
        if (currTime > spawnTime) {
            GameObject tempGaster = Instantiate(gasterFactory);
            tempGaster.transform.position = gasterPivot;
            tempGaster.transform.LookAt(player.transform);
            radius += 0.1f;
           
        }
    }


}
