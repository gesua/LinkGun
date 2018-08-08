using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletType : MonoBehaviour
{
    public enum B_Type
    {
        Basic,
        Knife,
        Nabi
    };
    public B_Type BType = B_Type.Basic;

    // 딱 1번만 스프라이트 찾음
    static bool IsInitialize = false;
    static Sprite BasicSprite;
    static Sprite KnifeSprite;
    static Sprite[] NabiSprite;

    // 랜더러
    SpriteRenderer BulletSR;

    // 콜라이더
    BoxCollider BulletCollider;

    bool IsAnimation = false; // 움직이는 총알
    float CurrentTime = 0; // 누적 시간
    int AniCount = 0; // 스프라이트 몇 번째 보여줄지
    int AniAddValue = 1; // 더해질 값

    void Start()
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

        switch (BType)
        {
            case B_Type.Basic: // 기본
                BulletSR.sprite = BasicSprite;
                BulletCollider.center = Vector3.zero;
                BulletCollider.size = new Vector3(0.25f, 1, 0.3f);
                BulletSR.transform.localScale = new Vector3(3, 3, 1);
                break;
            case B_Type.Knife: // 나이프
                BulletSR.sprite = KnifeSprite;
                BulletCollider.center = Vector3.zero;
                BulletCollider.size = new Vector3(0.15f, 1, 0.16f);
                BulletSR.transform.localScale = new Vector3(3, 3, 1);
                break;
            case B_Type.Nabi: // 나비
                IsAnimation = true;
                BulletSR.sprite = NabiSprite[0];
                BulletCollider.center = new Vector3(0.02f, 0, -0.03f);
                BulletCollider.size = new Vector3(0.15f, 1, 0.16f);
                BulletSR.transform.localScale = new Vector3(3, 3, 1);
                break;
        }
    }

    private void Update()
    {
        // 움직이는 총알
        if (IsAnimation)
        {
            CurrentTime += Time.deltaTime;

            // 애니메이션 재생
            if (CurrentTime >= 0.05f)
            {
                CurrentTime = 0; // 초기화
                AniCount += AniAddValue; // 다음 스프라이트

                switch (BType)
                {
                    case B_Type.Nabi:
                        if (AniCount == 0) AniAddValue *= -1; // 정순으로 보이기
                        if (AniCount == 3) AniAddValue *= -1; // 역순으로 보이기

                        BulletSR.sprite = NabiSprite[AniCount];
                        break;
                }
            }
        }
    }

    // 총알 타입 설정
    //public void SetBType(B_Type type)
    //{
    //    BType = type;
    //}
}
