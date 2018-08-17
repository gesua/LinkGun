using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBoomerang : Weapon
{
    public BigBoomerang()
    {
        Number = 4;
        Name = "BigBoomerang";
        W_Type = WeaponType.Boomerang;
        CooldownTime = 0.2f;
        AmmoMax = 1;
        AmmoCount = AmmoMax;

        Power = 50;
        BulletSpeed = 5f;
        BulletTime = 100f;

        BulletCollider = new Vector3(0.5f, 1, 0.5f);
        BulletSize = new Vector3(5, 5, 1);

        WeaponUISize = new Vector3(8, 12, 1);

        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/LinkImage");
        for (int i = 0; i < sprites.Length; i++)
        {
            if (sprites[i].name.Equals("BigBoomerang"))
            {
                WeaponSprite = sprites[i];
            }

            if (sprites[i].name.Equals("BigBoomerangBullet"))
            {
                BulletSprite = sprites[i];
            }
        }
        if (WeaponSprite == null)
        {
            Debug.LogError("BigBoomerang WeaponSprite 못 찾음");
        }
        if (BulletSprite == null)
        {
            Debug.LogError("BigBoomerang BulletSprite 못 찾음");
        }
    }
}
