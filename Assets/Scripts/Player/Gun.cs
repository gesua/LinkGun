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
        MagicStick, // 마법봉
        Boomerang // 부메랑
    }
    WeaponType W_Type = WeaponType.MagicStick;

    // 쿨다운
    bool IsCooldown = false; // 쿨다운 중인지
    public float CooldownTime = 0.2f; // 연사속도
    float CooldownCount = 0f; // 연사속도 세는거

    // 총알 Pool
    GameObject[] AllBullet; // 모든 총알
    List<GameObject> BulletPool; // Pool 리스트
    public int BulletPoolSize = 20; // Pool 최대 갯수

    // 탄약
    public int AmmoMax = 30; // 최대 탄약
    int AmmoCount = 0; // 남은 탄약
    Text AmmoText; // UI

    // 재장전
    bool IsReload = false; // 장전 중인지
    public float ReloadSpeed = 2f; // 장전속도
    float ReloadCount = 0f; // 장전시간 세는거
    Slider ReloadSlider; // 장전 보여줄 UI

    // 무기 스프라이트
    Sprite BlueWandSprite; // 마법봉
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
                if (IsCooldown == false)
                {
                    Shoot();
                }
            }
            else if (IsReload == false) // 재장전 켜기
            {
                IsReload = true;
            }
        }

        //if(Input.getmou)

        // 재장전
        if (IsReload)
        {
            Reload();
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
                transform.localPosition = new Vector3(-0.195f, 0, -0.12f);
                GunImage.sortingOrder = 3;
                break;
            case 2: // ↓
                transform.localPosition = new Vector3(-0.18f, 0, -0.15f);
                GunImage.sortingOrder = 3;
                break;
            case 3: // ↘
                transform.localPosition = new Vector3(-0.09f, 0, -0.2f);
                GunImage.sortingOrder = 3;
                break;
            case 4: // ←
                transform.localPosition = new Vector3(-0.2f, 0, -0.04f);
                GunImage.sortingOrder = 1;
                break;
            case 6: // →
                transform.localPosition = new Vector3(0.03f, 0, -0.2f);
                GunImage.sortingOrder = 3;
                break;
            case 7: // ↖
                transform.localPosition = new Vector3(0, 0, 0);
                GunImage.sortingOrder = 1;
                break;
            case 8: // ↑
                transform.localPosition = new Vector3(0.2f, 0, -0.038f);
                GunImage.sortingOrder = 1;
                break;
            case 9: // ↗
                transform.localPosition = new Vector3(0.1f, 0, -0.2f);
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
            //temp.InvokeOff();
            temp.enabled = false;
        }
    }

    // 무기 변경
    void WeaponChange()
    {
        GunImage.sprite = BoomerangSprite;
    }
}
