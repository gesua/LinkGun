using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBomb : Weapon {
    public TimeBomb()
    {
        Number = 5;
        Name = "TimeBomb";
        W_Type = WeaponType.TimeBomb;
        CooldownTime = 0.2f;
        AmmoMax = 3;
        AmmoCount = AmmoMax;

        Power = 30;
        BulletSpeed = 0;
        BulletTime = 3f;

        BulletCollider = new Vector3(0.3f, 1, 0.3f);
        BulletSize = new Vector3(3, 3, 1);

        WeaponUISize = new Vector3(13, 14, 1);

        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/LinkImage");
        for (int i = 0; i < sprites.Length; i++)
        {
            if (sprites[i].name.Equals("TimeBomb"))
            {
                WeaponSprite = sprites[i];
                break;
            }
        }
        if (WeaponSprite == null)
        {
            Debug.LogError("TimeBombBullet WeaponSprite 못 찾음");
        }

        sprites = Resources.LoadAll<Sprite>("Sprites/BulletImage1");
        for (int i = 0; i < sprites.Length; i++)
        {
            if (sprites[i].name.Equals("TimeBombBullet"))
            {
                BulletSprite = sprites[i];
                break; // 1개만 찾으면 되니 바로 나옴
            }
        }
        if (BulletSprite == null)
        {
            Debug.LogError("TimeBombBullet BulletSprite 못 찾음");
        }
    }
}
