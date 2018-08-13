﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : Weapon
{
    public Boomerang()
    {
        Number = 3;
        Name = "Boomerang";
        W_Type = WeaponType.Boomerang;
        CooldownTime = 0.2f;
        AmmoMax = 3;
        AmmoCount = AmmoMax;

        Power = 10;
        BulletSpeed = 10f;
        BulletTime = 1f;

        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/LinkImage");
        for (int i = 0; i < sprites.Length; i++)
        {
            if (sprites[i].name.Equals("Boomerang"))
            {
                WeaponSprite = sprites[i];
            }

            if (sprites[i].name.Equals("BoomerangBullet"))
            {
                BulletSprite = sprites[i];
            }
        }
        if (WeaponSprite == null)
        {
            Debug.LogError("Sword WeaponSprite 못 찾음");
        }
        if (BulletSprite == null)
        {
            Debug.LogError("Sword BulletSprite 못 찾음");
        }
    }
}
