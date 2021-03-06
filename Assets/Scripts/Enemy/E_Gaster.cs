﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Gaster : MonoBehaviour {
    //정면쪽으로 레이저 발사 필요
    //레이저발사 스프라이트 이미지 변경
    //이펙트

    //스프라이트이미지
    SpriteRenderer gasterSprite;
    Sprite[] sprite;
    //레이저받기

    //GasterSpawner gasterSpawner;

    

    /*public void SetSpawner(GasterSpawner spawner) {
        gasterSpawner = spawner;
    }*/

    void Start() {
        //가스터이미지지정
        gasterSprite = gameObject.GetComponentInChildren<SpriteRenderer>();
        SetGasterSprite();
    }
    private void OnEnable() {
        StartCoroutine("GasterSpawn");
    }


    //가스터 스프라이트 이미지 지정
    void SetGasterSprite() {
        Sprite[] tempSprite = Resources.LoadAll<Sprite>("Sprites/gaster");
        int gasterCount = 0;
        sprite = new Sprite[4];
        for (int i = 0; i < tempSprite.Length; i++) {
            if (tempSprite[i].name.Contains("Bla")) {
                sprite[gasterCount++] = tempSprite[i];
            }
        }
    }

    //가스터 스폰시 이미지 작게했다가 늘려주기
    IEnumerator GasterSpawn() {
        for (int i = 0; i < 5; i++) {
            yield return new WaitForSeconds(0.05f);
            this.transform.localScale = new Vector3((i + 1) * 0.2f, this.transform.localScale.y, this.transform.localScale.z);
        }
        //나타난후 공격대기시간
        yield return new WaitForSeconds(0.3f);
        StartCoroutine("GasterAttack");
    }
    IEnumerator GasterAttack() {
        gasterSprite.sprite = sprite[0];
        //레이저생성
        GameObject tempLaser = LaserSpawner.Instance.ActiveLaser();
        tempLaser.transform.position = this.transform.position;
        tempLaser.transform.forward = this.transform.forward;
        tempLaser.GetComponentInChildren<SpriteRenderer>().enabled = false;
        tempLaser.GetComponent<BoxCollider>().enabled = false;
        tempLaser.SetActive(true);
        for (int i = 1; i < sprite.Length; i++) {
            yield return new WaitForSeconds(0.05f);
            gasterSprite.sprite = sprite[i];
            tempLaser.GetComponentInChildren<SpriteRenderer>().enabled = true;
            tempLaser.transform.localScale = new Vector3((i + 1) * 0.25f, this.transform.localScale.y, this.transform.localScale.z * 10);
        }
      
        tempLaser.GetComponent<BoxCollider>().enabled = true;
        tempLaser.GetComponent<BoxCollider>().size = new Vector3(tempLaser.GetComponent<BoxCollider>().size.x, tempLaser.GetComponent<BoxCollider>().size.y, 2.5f);
        tempLaser.GetComponent<BoxCollider>().center = new Vector3(tempLaser.GetComponent<BoxCollider>().center.x, tempLaser.GetComponent<BoxCollider>().center.y, 1.2f);
        yield return new WaitForSeconds(0.15f);
        LaserSpawner.Instance.AddLaserPool(tempLaser);
        for (int i = 3; i > 0; i--) {
            this.transform.localScale = new Vector3((i + 1) * 0.25f, this.transform.localScale.y, this.transform.localScale.z);
            yield return new WaitForSeconds(0.05f);
        }
        //가스터를 풀에 집어넣음
        GasterSpawner.Instance.AddGasterPool(gameObject);
        //레이저를 풀에 집어넣음

    }
    public void GameSetCoroutine() {
        StopAllCoroutines();
    }
}
