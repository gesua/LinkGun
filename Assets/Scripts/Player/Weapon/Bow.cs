using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon
{
    public Bow()
    {
        Number = 5;
        Name = "Bow";
        W_Type = WeaponType.Bow;

        Power = 30;
        BulletSpeed = 20f;
        BulletTime = 100f;

        BulletCollider = new Vector3(0.15f, 1, 0.65f);
        BulletSize = new Vector3(5, 5, 1);

        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/LinkImage");
        for (int i = 0; i < sprites.Length; i++)
        {
            if (sprites[i].name.Equals("Bow"))
            {
                WeaponSprite = sprites[i];
            }

            if (sprites[i].name.Equals("BowBullet"))
            {
                BulletSprite = sprites[i];
            }
        }
        if (WeaponSprite == null)
        {
            Debug.LogError("Bow WeaponSprite 못 찾음");
        }
        if (BulletSprite == null)
        {
            Debug.LogError("Bow BulletSprite 못 찾음");
        }
    }
}
