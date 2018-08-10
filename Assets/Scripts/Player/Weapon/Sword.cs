using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    public Sword()
    {
        Number = 2;
        Name = "Sword";
        W_Type = WeaponType.Gun;
        CooldownTime = 0.1f;
        ReloadSpeed = 5f;
        AmmoMax = 100;
        AmmoCount = AmmoMax;

        Power = 1;

        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/LinkImage");
        for (int i = 0; i < sprites.Length; i++)
        {
            if (sprites[i].name.Equals("Sword"))
            {
                WeaponSprite = sprites[i];
            }
            
            if (sprites[i].name.Equals("SwordBullet"))
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

        BulletCollider = new Vector3(0.15f, 1, 0.65f);
    }
}
