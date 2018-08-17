using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MobSpawner : MonoSingleton<MobSpawner> {
    //스포너위치를받을배열
    Transform[] spawnPos;
    //스폰시간
    public float mobSpawnTime = 1f;
    float spawnCurrTime = 0f;

    //잡몹 오브젝트풀
    public GameObject mobFactory;
    GameObject[] mobPool;
    List<GameObject> deactiveList = new List<GameObject>();
    public int poolSize = 100;
    public int ableCount = 15;

    private void Awake() {
        //싱글톤
        SetInstance(this);
    }
    // Use this for initialization
    void Start() {
        SetPool(); //풀지정
        spawnPos = GetComponentsInChildren<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        spawnCurrTime += Time.deltaTime;
        if(spawnCurrTime > mobSpawnTime) {
            if (deactiveList.Count > 0 && deactiveList.Count > poolSize - ableCount) {
                spawnCurrTime = 0f;
                //위치지정
                GameObject tempMob = MobPoolActive();
                if (tempMob.activeSelf == false)
                {
                    tempMob.transform.position = spawnPos[Random.Range(1, spawnPos.Length)].position;
                    tempMob.SetActive(true);
                }
            }
        }
	}
    //몹풀
    void SetPool() {
        mobPool = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++) {
            mobPool[i] = Instantiate(mobFactory);
            mobPool[i].SetActive(false);
            mobPool[i].transform.parent = GameObject.Find("E_Mob").transform;
            deactiveList.Add(mobPool[i]);
        }
    }
    
    //풀액티브
    GameObject MobPoolActive() {
       
        GameObject tempMob = deactiveList[0];
        deactiveList.RemoveAt(0);
        //꺼졌던 액티브 다시온
        tempMob.GetComponentInChildren<Animator>().enabled = true;
        tempMob.GetComponent<BoxCollider>().enabled = true;
        tempMob.GetComponent<SphereCollider>().enabled = true;
        //tempMob.SetActive(true);
        return tempMob;
    }

    public void AddMobPool(GameObject mob) {
        mob.SetActive(false);
        deactiveList.Add(mob);
    }
    public void AllMobOff() {
        for (int i = 0; i < poolSize; i++) {
            mobPool[i].GetComponent<EnemyMob>().enabled = false;
            mobPool[i].GetComponent<NavMeshAgent>().enabled = false;
            mobPool[i].GetComponentInChildren<Animator>().enabled = false;
        }
    }
    public void AllMobDisable() {
        for (int i = 0; i < poolSize; i++) {
            if(mobPool[i].activeSelf) {
                mobPool[i].GetComponent<EnemyMob>().Dead();
                
            }
        }
    }
}
