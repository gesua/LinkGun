using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSpawner : MonoSingleton<LaserSpawner> {

    //가스터풀만듬
    //레이저받을팩토리
    public GameObject laserFactory;

    GameObject[] laserPool;
    List<GameObject> deactiveList = new List<GameObject>();
    public int poolSize = 50;


    private void Awake() {

        SetInstance(this);

    }

    void Start() {
        laserPool = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++) {
            laserPool[i] = Instantiate(laserFactory);
            laserPool[i].GetComponent<E_Laser>().SetSpawner(this);
            laserPool[i].SetActive(false);
            //Spawner의자식
            laserPool[i].transform.parent = GameObject.Find("E_Laser").transform;
            deactiveList.Add(laserPool[i]);
        }
    }
    //레이저발사
    public GameObject ActiveLaser() {
        GameObject tempLaser = deactiveList[0];
        deactiveList.RemoveAt(0);
        return tempLaser;
    }


    public void AddLaserPool(GameObject laser) {
        deactiveList.Add(laser);
    }

    public void AllLaserOff() {
        for (int i = 0; i < poolSize; i++) {
            laserPool[i].GetComponent<E_Laser>().enabled = false;
        }
    }
    public void AllLaserDisable() {
        //Bomb
        for (int i = 0; i < poolSize; i++) {
            laserPool[i].SetActive(false);
        }
    }
}
