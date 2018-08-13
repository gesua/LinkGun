using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    GameObject BulletPrefab;
    Transform Target; // 마우스 위치

    SpriteRenderer GunImage;

    // 쿨다운
    bool IsCooldown = false; // 쿨다운 중인지
    float CooldownTime; // 연사속도
    float CooldownCount = 0f; // 연사속도 세는거

    // 총알 Pool
    GameObject[] AllBullet; // 모든 총알
    List<GameObject> BulletPool; // Pool 리스트
    int BulletPoolSize = 500; // Pool 최대 갯수

    // 무기
    List<Weapon> WeaponsList = new List<Weapon>(); // 현재 갖고 있는 무기 리스트
    int WeaponSelectNumber = 0; // 현재 사용중인 무기 위치
    Weapon NowWeapon; // 지금 사용중인 무기

    // 탄약
    int AmmoMax; // 최대 탄약
    Text AmmoText; // UI

    // 재장전
    bool IsReload = false; // 장전 중인지
    float ReloadSpeed; // 장전속도
    float ReloadCount = 0f; // 장전시간 세는거
    Slider ReloadSlider; // 장전 보여줄 UI

    // 무기 스프라이트
    Image UIGunImage; // UI상 이미지 위치

    Camera MainCam;

    private void Awake()
    {
        GunImage = GetComponentInChildren<SpriteRenderer>();
        if (GunImage == null)
        {
            Debug.LogError("GunImage 못 찾음");
            return;
        }

        // 기본 무기 추가하고 값 복사하고 정보 받아옴
        WeaponsList.Add(new BlueWand());
        CooldownTime = WeaponsList[0]._CooldownTime;
        AmmoMax = WeaponsList[0]._AmmoMax;
        ReloadSpeed = WeaponsList[0]._ReloadSpeed;
        NowWeapon = WeaponsList[0];
    }

    private void Start()
    {
        MainCam = Camera.main;

        BulletPrefab = Resources.Load<GameObject>("Prefabs/P_Bullet");
        if (BulletPrefab == null)
        {
            Debug.LogError("BulletPrefab 못 찾음");
            return;
        }

        Target = GameObject.Find("MousePoint").transform;
        if (Target == null)
        {
            Debug.LogError("Target 못 찾음");
            return;
        }

        // Pool 생성
        BulletPool = new List<GameObject>();
        AllBullet = new GameObject[BulletPoolSize];
        Transform BulletBox = GameObject.Find("P_Bullet").transform;
        for (int i = 0; i < BulletPoolSize; i++)
        {
            GameObject tempBullet = Instantiate(BulletPrefab);
            tempBullet.GetComponent<P_Bullet>().SetGun(this);
            tempBullet.transform.parent = BulletBox;
            tempBullet.SetActive(false);

            BulletPool.Add(tempBullet);
            AllBullet[i] = tempBullet;
        }

        // 무기 추가
        WeaponsList.Add(new Sword());
        WeaponsList.Add(new Boomerang());

        // AmmoText
        GameObject tempUI = GameObject.Find("UI");
        Transform tempGun = tempUI.transform.Find("Gun");
        AmmoText = tempGun.GetComponentInChildren<Text>();
        if (AmmoText == null)
        {
            Debug.LogError("AmmoText 못 찾음");
            return;
        }

        // UIGunImage
        UIGunImage = tempGun.Find("Image").GetComponent<Image>();
        if (UIGunImage == null)
        {
            Debug.LogError("UIGunImage 못 찾음");
            return;
        }

        // ReloadSlider
        Transform tempReload = tempUI.transform.Find("Reload");
        ReloadSlider = tempReload.GetChild(0).GetComponent<Slider>();
        if (ReloadSlider == null)
        {
            Debug.LogError("ReloadSlider 못 찾음");
            return;
        }

        // TextUI 세팅
        UpdateAmmoTextUI();
    }

    void Update()
    {
        // 마우스 포인터 바라봄
        LookTarget();

        // 무기 종류 따라 다르게 작동
        switch (NowWeapon._W_Type)
        {
            // 일반 총
            case WeaponType.Gun:
                NomalGun();
                break;

            // 부메랑
            case WeaponType.Boomerang:
                Boomerang();
                break;
        }

        // 마우스 휠
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            if (IsReload == false)
            {
                // 무기 변경
                WeaponChange();
            }
        }
    }

    // 일반 총
    void NomalGun()
    {
        // 쿨다운
        if (IsCooldown)
        {
            Cooldown();
        }

        // 좌클릭
        if (Input.GetMouseButton(0))
        {
            // 총알 발사
            if (NowWeapon._AmmoCount > 0)
            {
                if (IsCooldown == false && IsReload == false)
                {
                    Shoot();
                }
            }
            else if (IsReload == false) // 재장전 켜기
            {
                IsReload = true;
            }
        }

        // R키
        if (Input.GetKeyDown(KeyCode.R))
        {
            // 재장전
            if (IsReload == false && NowWeapon._AmmoCount != AmmoMax)
            {
                IsReload = true;
            }
        }

        // 재장전
        if (IsReload)
        {
            Reload();
        }
    }

    // 부메랑
    void Boomerang()
    {
        // 쿨다운
        if (IsCooldown)
        {
            Cooldown();
        }

        // 좌클릭
        if (Input.GetMouseButton(0))
        {
            // 부메랑 날림
            if (NowWeapon._AmmoCount > 0)
            {
                if (IsCooldown == false)
                {
                    Shoot();
                    IsReload = true; // 부메랑 회수 전까진 무기변경 불가
                }
            }
        }
    }

    // 총알 발사
    void Shoot()
    {
        // Pool에서 켜기
        if (BulletPool.Count > 0)
        {
            NowWeapon._AmmoCount--; // 총알 1개 소모

            // 부메랑 다 날리면 손에서 사라짐(보여지는 건 회수하는 부분에 있음)
            if (NowWeapon._W_Type == WeaponType.Boomerang)
            {
                if (NowWeapon._AmmoCount <= 0)
                {
                    if (GunImage.enabled) GunImage.enabled = false;
                }
            }

            // TextUI 세팅
            UpdateAmmoTextUI();

            // Pool에서 뺌
            GameObject tempBullet = BulletPool[0];
            BulletPool.RemoveAt(0);

            // 총알 상세 설정
            P_Bullet tempScript = tempBullet.GetComponent<P_Bullet>();
            tempScript.SetAttribute(NowWeapon._W_Type, NowWeapon._BulletSpeed, NowWeapon._Power, NowWeapon._BulletTime);

            // 생김새 바꿔줌
            tempBullet.GetComponentInChildren<SpriteRenderer>().sprite = NowWeapon._BulletSprite;

            // 콜라이더 잡아줌
            tempBullet.GetComponent<BoxCollider>().size = NowWeapon._BulletCollider;

            // 위치 잡아줌
            tempBullet.transform.position = transform.position + transform.forward * 0.5f; // 약간 앞에서 발사
            tempBullet.transform.LookAt(Target);

            // x축 회전 없앰
            Vector3 tempAngle = tempBullet.transform.eulerAngles;
            tempAngle.x = 0;
            tempBullet.transform.eulerAngles = tempAngle;

            // 쿨다운 시작
            IsCooldown = true;

            // 총알 켬
            tempBullet.SetActive(true);
        }
        else
        {
            Debug.Log("Player BulletPool 초과");
        }
    }

    // 재장전
    void Reload()
    {
        // 슬라이더 켜기
        if (ReloadSlider.gameObject.activeSelf == false) ReloadSlider.gameObject.SetActive(true);

        ReloadCount += Time.deltaTime;

        // 슬라이더 위치
        Vector3 temp = MainCam.WorldToScreenPoint(transform.parent.position);
        temp.y += 70;
        temp.z = 0;
        ReloadSlider.transform.position = temp;

        ReloadSlider.value = ReloadCount / ReloadSpeed;

        if (ReloadCount >= ReloadSpeed)
        {
            // 초기화
            ReloadCount = 0;

            // 재장전
            NowWeapon._AmmoCount = AmmoMax;

            // TextUI 세팅
            UpdateAmmoTextUI();

            // 슬라이더 끔
            ReloadSlider.gameObject.SetActive(false);

            // 재장전 끝
            IsReload = false;
        }
    }

    // 마우스 포인터 바라봄
    void LookTarget()
    {
        transform.LookAt(Target);

        // x축 회전 없앰
        Vector3 tempAngle = transform.eulerAngles;
        tempAngle.x = 0;
        transform.eulerAngles = tempAngle;
    }

    // 총 위치 설정
    public void SetPosition(int way)
    {
        switch (way)
        {
            case 1: // ↙
                transform.localPosition = new Vector3(-0.195f, 0, -0.02f);
                GunImage.sortingOrder = 3;
                break;
            case 2: // ↓
                transform.localPosition = new Vector3(-0.18f, 0, -0.05f);
                GunImage.sortingOrder = 3;
                break;
            case 3: // ↘
                transform.localPosition = new Vector3(-0.09f, 0, -0.1f);
                GunImage.sortingOrder = 3;
                break;
            case 4: // ←
                transform.localPosition = new Vector3(-0.2f, 0, 0.06f);
                GunImage.sortingOrder = 1;
                break;
            case 6: // →
                transform.localPosition = new Vector3(0.03f, 0, -0.1f);
                GunImage.sortingOrder = 3;
                break;
            case 7: // ↖
                transform.localPosition = new Vector3(0, 0, 0.1f);
                GunImage.sortingOrder = 1;
                break;
            case 8: // ↑
                transform.localPosition = new Vector3(0.2f, 0, 0.062f);
                GunImage.sortingOrder = 1;
                break;
            case 9: // ↗
                transform.localPosition = new Vector3(0.1f, 0, -0.1f);
                GunImage.sortingOrder = 1;
                break;
        }
    }

    // 쿨다운 증가
    void Cooldown()
    {
        CooldownCount += Time.deltaTime;

        // 다 셌음
        if (CooldownCount >= CooldownTime)
        {
            CooldownCount = 0;
            IsCooldown = false;
        }
    }

    // 총알 회수
    public void BulletCollect(GameObject bullet, WeaponType w_type)
    {
        BulletPool.Add(bullet);

        // 부메랑 회수
        if (w_type == WeaponType.Boomerang)
        {
            NowWeapon._AmmoCount++;

            // 전부 회수
            if(NowWeapon._AmmoCount == AmmoMax)
            {
                IsReload = false;
            }

            // 부메랑 들고 있기(없애는 건 총알 발사에 있음)
            if (GunImage.enabled == false) GunImage.enabled = true;

            // AmmoTextUI 세팅
            UpdateAmmoTextUI();
        }
    }

    // 모든 총알 멈추기
    public void AllBulletOff()
    {
        for (int i = 0; i < BulletPoolSize; i++)
        {
            P_Bullet temp = AllBullet[i].GetComponent<P_Bullet>();
            temp.enabled = false;
        }
    }

    // 무기 변경
    void WeaponChange()
    {
        // 1개씩 교체
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            WeaponSelectNumber++;

            if (WeaponSelectNumber >= WeaponsList.Count)
            {
                WeaponSelectNumber = 0;
            }
        }
        else
        {
            WeaponSelectNumber--;
            if (WeaponSelectNumber < 0)
            {
                WeaponSelectNumber = WeaponsList.Count - 1;
            }
        }

        // 무기 정보 받음
        NowWeapon = WeaponsList[WeaponSelectNumber];

        // 이미지 교체
        GunImage.sprite = NowWeapon._WeaponSprite;
        UIGunImage.sprite = NowWeapon._WeaponSprite;

        // 타입에 따라 다름
        switch (NowWeapon._W_Type)
        {
            // 일반 총
            case WeaponType.Gun:
                // 필요한 값 복사
                CooldownTime = NowWeapon._CooldownTime;
                AmmoMax = NowWeapon._AmmoMax;
                ReloadSpeed = NowWeapon._ReloadSpeed;
                break;

            // 부메랑
            case WeaponType.Boomerang:
                // 필요한 값 복사
                CooldownTime = NowWeapon._CooldownTime;
                AmmoMax = NowWeapon._AmmoMax;
                break;
        }

        // AmmoTextUI 세팅
        UpdateAmmoTextUI();
    }

    // AmmoTextUI 세팅
    void UpdateAmmoTextUI()
    {
        AmmoText.text = NowWeapon._AmmoCount.ToString() + " / " + AmmoMax.ToString();
    }
}
