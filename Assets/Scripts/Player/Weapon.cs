using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    // 무기 타입
    enum WeaponType
    {
        None,     // 없음
        Gun,      // 총
        Boomerang // 부메랑
    }
    WeaponType W_Type = WeaponType.None;

    // 속성
    private int Number; // 번호
    private string Name; // 이름
    private float CooldownTime; // 쿨다운 시간
    private float ReloadSpeed; // 장전속도
    private int AmmoMax; // 최대 탄약
    private int AmmoCount; // 남은 탄약
    private Sprite WeaponSprite; // 무기 스프라이트
    private Sprite BulletSprite; // 총알 스프라이트

    Weapon(int _num, string _name, float _cooldown, float _reload, int _ammoMax, int _ammoCount, Sprite _weapon, Sprite _bullet)
    {
        Number = _num;
        Name = _name;
        CooldownTime = _cooldown;
        ReloadSpeed = _reload;
        AmmoMax = _ammoMax;
        AmmoCount = _ammoCount;

    }

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

    public int _AmmoCount
    {
        get
        {
            return AmmoCount;
        }
    }
}
