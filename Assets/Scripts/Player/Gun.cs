using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    GameObject BulletPrefab;
    Transform Target; // 마우스 위치

    SpriteRenderer GunImage;

    // 무기 타입
    enum WeaponType
    {
        MIN,       // 최소
        BlueWand,  // 마법봉
        Sword,     // 칼
        Boomerang, // 부메랑
        MAX        // 최대
    }
    WeaponType W_Type = WeaponType.BlueWand;

    // 쿨다운
    bool IsCooldown = false; // 쿨다운 중인지
    float CooldownTime = 0.2f; // 연사속도
    float CooldownCount = 0f; // 연사속도 세는거

    // 총알 Pool
    GameObject[] AllBullet; // 모든 총알
    List<GameObject> BulletPool; // Pool 리스트
    public int BulletPoolSize = 20; // Pool 최대 갯수

    // 탄약
    int AmmoMax = 30; // 최대 탄약
    int AmmoCount = 0; // 남은 탄약
    Text AmmoText; // UI
    int[] TempAmmo = new int[2]; // 무기에 따라 탄약 담아둘 거

    // 재장전
    bool IsReload = false; // 장전 중인지
    float ReloadSpeed = 2f; // 장전속도
    float ReloadCount = 0f; // 장전시간 세는거
    Slider ReloadSlider; // 장전 보여줄 UI

    // 무기 스프라이트
    Image UIGunImage; // UI상 이미지 위치
    Sprite BlueWandSprite; // 마법봉
    Sprite SwordSprite; // 칼
    Sprite BoomerangSprite; // 부메랑

    Camera MainCam;

    private void Awake()
    {
        GunImage = GetComponentInChildren<SpriteRenderer>();
        if (GunImage == null)
        {
            Debug.LogError("GunImage 못 찾음");
            return;
        }
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

        // AmmoText
        GameObject tempUI = GameObject.Find("UI");
        Transform tempGun = tempUI.transform.Find("Gun");
        AmmoText = tempGun.GetComponentInChildren<Text>();
        if (AmmoText == null)
        {
            Debug.LogError("AmmoText 못 찾음");
            return;
        }

        // 초기 총알
        TempAmmo[0] = 30; // 마법봉
        TempAmmo[1] = 100; // 칼

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

        // 무기 스프라이트
        Sprite[] temp = Resources.LoadAll<Sprite>("Sprites/LinkImage");
        for (int i = 0; i < temp.Length; i++)
        {
            // 마법봉
            if (temp[i].name.Equals("BlueWand"))
            {
                BlueWandSprite = temp[i];
            }

            // 칼
            if (temp[i].name.Equals("Sword"))
            {
                SwordSprite = temp[i];
            }

            // 부메랑
            if (temp[i].name.Equals("Boomerang"))
            {
                BoomerangSprite = temp[i];
            }
        }
        if (BlueWandSprite == null)
        {
            Debug.LogError("BlueWandSprite 못 찾음");
            return;
        }
        if (SwordSprite == null)
        {
            Debug.LogError("SwordSprite 못 찾음");
            return;
        }
        if (BoomerangSprite == null)
        {
            Debug.LogError("BoomerangSprite 못 찾음");
            return;
        }

        // 총알 최대로 장전
        AmmoCount = AmmoMax;

        // TextUI 세팅
        AmmoText.text = AmmoCount.ToString() + " / " + AmmoMax.ToString();
    }

    void Update()
    {
        // 마우스 포인터 바라봄
        LookTarget();

        // 무기 종류 따라 다르게 작동
        switch (W_Type)
        {
            // 마법봉, 칼
            case WeaponType.BlueWand:
            case WeaponType.Sword:
                // 쿨다운
                if (IsCooldown)
                {
                    Cooldown();
                }

                // 좌클릭
                if (Input.GetMouseButton(0))
                {
                    // 총알 발사
                    if (AmmoCount > 0)
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
                    if (IsReload == false && AmmoCount != AmmoMax)
                    {
                        IsReload = true;
                    }
                }

                // 재장전
                if (IsReload)
                {
                    Reload();
                }
                break;

            // 부메랑
            case WeaponType.Boomerang:

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

    // 총알 발사
    void Shoot()
    {
        // Pool에서 켜기
        if (BulletPool.Count > 0)
        {
            AmmoCount--; // 총알 1개 소모

            // TextUI 세팅
            AmmoText.text = AmmoCount.ToString() + " / " + AmmoMax.ToString();

            // Pool에서 뺌
            GameObject tempBullet = BulletPool[0];
            BulletPool.RemoveAt(0);

            // 켬
            tempBullet.SetActive(true);

            // 위치 잡아줌
            tempBullet.transform.position = transform.position + transform.forward * 0.5f; // 약간 앞에서 발사
            tempBullet.transform.LookAt(Target);

            // x축 회전 없앰
            Vector3 tempAngle = tempBullet.transform.eulerAngles;
            tempAngle.x = 0;
            tempBullet.transform.eulerAngles = tempAngle;

            // 쿨다운 시작
            IsCooldown = true;
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
            AmmoCount = AmmoMax;

            // TextUI 세팅
            AmmoText.text = AmmoCount.ToString() + " / " + AmmoMax.ToString();

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

    // 꺼진거 Pool에 넣기
    public void AddBulletPool(GameObject bullet)
    {
        BulletPool.Add(bullet);
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
        // 현재 총알 기억해놓기
        switch (W_Type)
        {
            case WeaponType.BlueWand:
                TempAmmo[0] = AmmoCount;
                break;
            case WeaponType.Sword:
                TempAmmo[1] = AmmoCount;
                break;
        }

        // 1개씩 교체
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            W_Type++;
            if (W_Type == WeaponType.MAX)
            {
                W_Type = WeaponType.MIN + 1;
            }
        }
        else
        {
            W_Type--;
            if (W_Type == WeaponType.MIN)
            {
                W_Type = WeaponType.MAX - 1;
            }
        }

        // 무기마다 이것저것 설정
        switch (W_Type)
        {
            case WeaponType.BlueWand: // 마법봉
                // 이미지 교체
                GunImage.sprite = BlueWandSprite;
                UIGunImage.sprite = BlueWandSprite;

                // 총알 변경
                AmmoCount = TempAmmo[0];

                // 

                // 총알 갯수 보이게
                AmmoText.enabled = true;
                AmmoText.text = AmmoCount.ToString() + " / " + AmmoMax.ToString();
                break;
            case WeaponType.Sword: // 칼
                // 이미지 교체
                GunImage.sprite = SwordSprite;
                UIGunImage.sprite = SwordSprite;

                // 총알 변경
                AmmoCount = TempAmmo[1];

                // 총알 갯수 보이게
                AmmoText.enabled = true;
                AmmoText.text = AmmoCount.ToString() + " / " + AmmoMax.ToString();
                break;
            case WeaponType.Boomerang: // 부메랑
                // 이미지 교체
                GunImage.sprite = BoomerangSprite;
                UIGunImage.sprite = BoomerangSprite;

                // 총알 갯수 안 보이게
                AmmoText.enabled = false;
                break;
        }
    }
}
