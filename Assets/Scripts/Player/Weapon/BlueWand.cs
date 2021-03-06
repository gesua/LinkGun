﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueWand : Weapon {

    public BlueWand()
    {
        Number = 2;
        Name = "BlueWand";
        W_Type = WeaponType.Gun;
        CooldownTime = 0.2f;
        ReloadSpeed = 2f;
        AmmoMax = 30;
        AmmoCount = AmmoMax;

        Power = 4;
        BulletSpeed = 20f;
        BulletTime = 3f;

        BulletCollider = new Vector3(0.3f, 1, 0.5f);
        BulletSize = new Vector3(5, 5, 1);

        WeaponUISize = new Vector3(8, 16, 1);

        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/LinkImage");
        for (int i = 0; i < sprites.Length; i++) {
            // 마법봉
            if (sprites[i].name.Equals("BlueWand"))
            {
                WeaponSprite = sprites[i];
                break; // 1개만 찾으면 되니 바로 나옴
            }
        }
        if(WeaponSprite == null)
        {
            Debug.LogError("BlueWand WeaponSprite 못 찾음");
        }

        sprites = Resources.LoadAll<Sprite>("Sprites/BulletImage1");
        for (int i = 0; i < sprites.Length; i++)
        {
            if (sprites[i].name.Equals("BlueBullet"))
            {
                BulletSprite = sprites[i];
                break; // 1개만 찾으면 되니 바로 나옴
            }
        }
        if (BulletSprite == null)
        {
            Debug.LogError("BlueWand BulletSprite 못 찾음");
        }
    }
}
