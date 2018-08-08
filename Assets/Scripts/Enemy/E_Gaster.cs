using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Gaster : MonoBehaviour {
    //정면쪽으로 레이저 발사 필요
    //레이저발사 스프라이트 이미지 변경
    //이펙트
 
    //스프라이트이미지
    SpriteRenderer gasterSprite;
    Sprite[] sprite;

    void Start() {
        //가스터이미지지정
        gasterSprite = gameObject.GetComponentInChildren<SpriteRenderer>();
        SetGasterSprite();
        StartCoroutine("GasterSpawn");
    }

    

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

  
    IEnumerator GasterSpawn() {
        gasterSprite.sprite = sprite[0];
        for (int i = 1; i < sprite.Length; i++) {
            yield return new WaitForSeconds(0.05f);
            gasterSprite.sprite = sprite[i];
        }
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
}
