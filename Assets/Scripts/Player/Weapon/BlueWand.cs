using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueWand : Weapon {

    public BlueWand()
    {
        Number = 1;
        Name = "BlueWand";
        W_Type = WeaponType.Gun;
        CooldownTime = 0.2f;
        ReloadSpeed = 2f;
        AmmoMax = 30;
        AmmoCount = AmmoMax;

        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/LinkImage");
        for (int i = 0; i < sprites.Length; i++) {
            // 마법봉
            if (sprites[i].name.Equals("BlueWand"))
            {
                WeaponSprite = sprites[i];
                break; // 1개만 찾으면 되니 바로 나옴
            }
        }

        sprites = Resources.LoadAll<Sprite>("Sprites/BulletImage1");
        for (int i = 0; i < sprites.Length; i++)
        {
            // 마법봉
            if (sprites[i].name.Equals("BlueBullet"))
            {
                BulletSprite = sprites[i];
                break; // 1개만 찾으면 되니 바로 나옴
            }
        }
    }
}
