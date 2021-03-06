﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 무기 타입
public enum WeaponType
{
    None,      // 없음
    Gun,       // 총
    Boomerang, // 부메랑
    TimeBomb   // 시한폭탄
}

public class Weapon
{
    protected WeaponType W_Type = WeaponType.None;

    // 속성
    protected int Number; // 번호
    protected string Name; // 이름

    protected int Power; // 공격력
    protected float BulletSpeed; // 총알 속도
    protected float BulletTime; // 총알 살아있는 시간
    protected Vector3 BulletSize; // 총알 크기

    protected float CooldownTime; // 쿨다운 시간
    protected float ReloadSpeed; // 장전속도
    protected int AmmoMax; // 최대 탄약
    protected int AmmoCount; // 남은 탄약

    protected Sprite WeaponSprite; // 무기 스프라이트
    protected Sprite BulletSprite; // 총알 스프라이트
    protected Vector3 BulletCollider; // 총알 콜라이더 크기
    protected Vector3 WeaponUISize; // 무기 UI 크기

    public Weapon()
    {
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

    public Vector3 _BulletCollider
    {
        get
        {
            return BulletCollider;
        }
    }

    public int _Power
    {
        get
        {
            return Power;
        }
    }

    public float _BulletSpeed
    {
        get
        {
            return BulletSpeed;
        }
    }

    public float _BulletTime
    {
        get
        {
            return BulletTime;
        }
    }

    public Vector3 _BulletSize
    {
        get
        {
            return BulletSize;
        }
    }

    public Vector3 _WeaponUISize
    {
        get
        {
            return WeaponUISize;
        }
    }
}
