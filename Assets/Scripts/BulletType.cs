using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletType : MonoBehaviour
{
    public enum B_Type
    {
        Basic,
        Knife,
        Nabi,
        Shuriken
    };
    public B_Type BType = B_Type.Basic;

    // 딱 1번만 스프라이트 찾음
    static bool IsInitialize = false;
    static Sprite BasicSprite;
    static Sprite KnifeSprite;
    static Sprite[] NabiSprite;
    static Sprite ShurikenSprite;

    // 랜더러
    SpriteRenderer BulletSR;

    // 콜라이더
    BoxCollider BulletCollider;

    bool IsAnimation = false; // 움직이는 총알
    float CurrentTime = 0; // 누적 시간

    // 나비
    int NabiCount = 0; // 스프라이트 몇 번째 보여줄지
    int NabiAddValue = 1; // 더해질 값

    private void Awake()
    {
        // 딱 1번만 스프라이트 찾음
        if (IsInitialize == false)
        {
            // 총알 찾기
            Sprite[] temp = Resources.LoadAll<Sprite>("Sprites/BulletImage2");
            NabiSprite = new Sprite[4];
            int nabiCount = 0;
            for (int i = 0; i < temp.Length; i++)
            {
                // 기본
                if (temp[i].name.Equals("Basic"))
                {
                    BasicSprite = temp[i];
                }
                // 나이프
                else if (temp[i].name.Equals("WhiteKnife"))
                {
                    KnifeSprite = temp[i];
                }
                // 나비
                else if (temp[i].name.Contains("Nabi"))
                {
                    NabiSprite[nabiCount] = temp[i];
                    nabiCount++;
                }
                // 수리검
                else if (temp[i].name.Equals("Shuriken"))
                {
                    ShurikenSprite = temp[i];
                }
            }
            if (KnifeSprite == null)
            {
                Debug.LogError("Knife 못 찾음");
                return;
            }
            if (NabiSprite[NabiSprite.Length - 1] == null)
            {
                Debug.LogError("Nabi 끝까지 못 찾음");
                return;
            }
            if (ShurikenSprite == null)
            {
                Debug.LogError("ShurikenSprite 못 찾음");
                return;
            }

            IsInitialize = true;
        }

        // 랜더러 가져옴
        BulletSR = GetComponentInChildren<SpriteRenderer>();
        if (BulletSR == null)
        {
            Debug.LogError("BulletSR 못 찾음");
            return;
        }

        // 콜라이더 가져옴
        BulletCollider = GetComponent<BoxCollider>();
        if (BulletCollider == null)
        {
            Debug.LogError("BulletCollider 못 찾음");
            return;
        }
    }

    void OnEnable()
    {
        switch (BType)
        {
            case B_Type.Basic: // 기본
                BulletSR.sprite = BasicSprite;
                BulletCollider.center = Vector3.zero;
                BulletCollider.size = new Vector3(0.25f, 1, 0.3f);
                break;
            case B_Type.Knife: // 나이프
                BulletSR.sprite = KnifeSprite;
                BulletCollider.center = Vector3.zero;
                BulletCollider.size = new Vector3(0.15f, 1, 0.16f);
                break;
            case B_Type.Nabi: // 나비
                IsAnimation = true;
                BulletSR.sprite = NabiSprite[0];
                BulletCollider.center = new Vector3(0.02f, 0, -0.03f);
                BulletCollider.size = new Vector3(0.15f, 1, 0.16f);
                break;
            case B_Type.Shuriken: // 수리검
                IsAnimation = true;
                BulletSR.sprite = ShurikenSprite;
                BulletCollider.size = new Vector3(0.15f, 1, 0.16f);
                break;
        }
        BulletSR.transform.localScale = new Vector3(3, 3, 1);
        BulletSR.transform.localEulerAngles = new Vector3(90f, 0, 0);
    }

    private void Update()
    {
        // 움직이는 총알
        if (IsAnimation)
        {
            switch (BType)
            {
                // 나비
                case B_Type.Nabi:
                    CurrentTime += Time.deltaTime;

                    // 애니메이션 재생
                    if (CurrentTime >= 0.05f)
                    {
                        CurrentTime = 0; // 초기화
                        NabiCount += NabiAddValue; // 다음 스프라이트

                        if (NabiCount == 0) NabiAddValue *= -1; // 정순으로 보이기
                        if (NabiCount == 3) NabiAddValue *= -1; // 역순으로 보이기

                        BulletSR.sprite = NabiSprite[NabiCount];
                    }
                    break;

                // 수리검
                case B_Type.Shuriken:
                    CurrentTime -= Time.deltaTime * 1000;

                    BulletSR.transform.eulerAngles = new Vector3(90, 0, CurrentTime);
                    break;
            }
        }
    }

    // 총알 타입 설정
    public void SetBType(B_Type type)
    {
        BType = type;
    }

    // 애니메이션 끄기
    public void StopAnimation()
    {
        IsAnimation = false;
    }
}
