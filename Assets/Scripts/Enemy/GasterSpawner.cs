using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasterSpawner : MonoBehaviour {
    //플레이어의 위치를 받음
    public Transform player;
    //가스터블래스터 스폰타임
    public float spawnTime = 1f;
    float currTime = 0f;
    public GameObject gasterFactory;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        currTime += Time.deltaTime;
        if(currTime > spawnTime) {
            GameObject gaster = Instantiate(gasterFactory);
            gaster.transform.position = new Vector3(player.transform.position.x + Random.Range(-2, 2), -10, player.transform.position.z + Random.Range(-2, 2));
            Vector3 dir = player.transform.position - gaster.transform.position;
            Debug.Log(dir);
            
            float degree = Mathf.Acos(dir.x);
            Debug.Log(degree);
            degree = Mathf.Rad2Deg * degree;
            gaster.transform.rotation = Quaternion.Euler(0, degree, 0);
            currTime = 0;
        }
	}
}
