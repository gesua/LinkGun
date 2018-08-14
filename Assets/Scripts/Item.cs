﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int ItemNumber = -1;

    Gun PlayerGun;

    // 이펙트 반짝반짝
    SpriteRenderer EffectSR;
    int AddValue = 1;
    float ChangeValue = 1;

    private void Start()
    {
        if (ItemNumber == -1)
        {
            Debug.LogError(gameObject.name + " 아이템 초기화 안 됨");
        }

        GameObject temp = GameObject.Find("Player");
        PlayerGun = temp.transform.Find("Gun").GetComponent<Gun>();
        if (PlayerGun == null)
        {
            Debug.LogError("PlayerGun 못 찾음");
            return;
        }

        EffectSR = transform.GetChild(1).GetComponent<SpriteRenderer>();
        if (EffectSR == null)
        {
            Debug.LogError("EffectSR 못 찾음");
            return;
        }
    }

    private void Update()
    {
        // 반짝반짝
        ChangeValue += AddValue * Time.deltaTime;

        if (ChangeValue <= 0.5f)
        {
            AddValue = 1;
        }
        else if (ChangeValue >= 1f)
        {
            AddValue = -1;
        }

        EffectSR.color = new Color(1, 1, 1, ChangeValue);
    }

    // 아이템 획득
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            switch (ItemNumber)
            {
                case 2: // 마법봉
                    PlayerGun.TakeWeapon(new BlueWand());
                    break;
                case 3: // 부메랑
                    PlayerGun.TakeWeapon(new Boomerang());
                    break;
                case 4: // 큰 부메랑
                    PlayerGun.TakeWeapon(new BigBoomerang());
                    break;
            }

            Destroy(gameObject);
        }
    }
}
