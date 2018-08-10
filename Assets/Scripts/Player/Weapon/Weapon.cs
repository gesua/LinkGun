using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 무기 타입
public enum WeaponType
{
    None,     // 없음
    Gun,      // 총
    Boomerang // 부메랑
}

public class Weapon
{
    protected WeaponType W_Type = WeaponType.None;

    // 속성
    protected int Number; // 번호
    protected string Name; // 이름
    protected float CooldownTime; // 쿨다운 시간
    protected float ReloadSpeed; // 장전속도
    protected int AmmoMax; // 최대 탄약
    protected int AmmoCount; // 남은 탄약
    protected Sprite WeaponSprite; // 무기 스프라이트
    protected Sprite BulletSprite; // 총알 스프라이트

    public Weapon()
    {
    }

    //Weapon(int _num, string _name, float _cooldown, float _reload, int _ammoMax, int _ammoCount, Sprite _weapon, Sprite _bullet)
    //{
    //    Number = _num;
    //    Name = _name;
    //    CooldownTime = _cooldown;
    //    ReloadSpeed = _reload;
    //    AmmoMax = _ammoMax;
    //    AmmoCount = _ammoCount;
    //    WeaponSprite = _weapon;
    //    BulletSprite = _bullet;
    //}

    public Sprite _BulletSprite
    {
        get
        {
            return BulletSprite;
        }
    }

    public Sprite _WeaponSprite
    {
        get
        {
            return WeaponSprite;
        }
    }

    public int _Number
    {
        get
        {
            return Number;
        }
    }

    public string _Name
    {
        get
        {
            return Name;
        }
    }

    public float _CooldownTime
    {
        get
        {
            return CooldownTime;
        }
    }

    public float _ReloadSpeed
    {
        get
        {
            return ReloadSpeed;
        }
    }

    public int _AmmoMax
    {
        get
        {
            return AmmoMax;
        }
    }

    public WeaponType _W_Type
    {
        get
        {
            return W_Type;
        }
    }

    public int _AmmoCount
    {
        get
        {
            return AmmoCount;
        }

        set
        {
            AmmoCount = value;
        }
    }
}
