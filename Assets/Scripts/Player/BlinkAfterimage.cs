using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkAfterimage : MonoBehaviour
{
    SpriteRenderer[] BlinkSR; // 블링크 이미지 위치들(4개)
    Sprite[] BlinkSprite; // 블링크 잔상 스프라이트

    float DestroyTime = -1f; // 사라지는 시간
    float CurrentTime = 0; // 경과시간

    float ChangeTime = 0; // 이미지 교체 시간
    int ChangeCount = 0; // 이미지 교체된 횟수

    void Start()
    {
        // 블링크 잔상 이미지
        Sprite[] tempSprite = Resources.LoadAll<Sprite>("Sprites/BulletImage1");
        BlinkSprite = new Sprite[4];
        int tempCount = 0;
        for (int i = 0; i < tempSprite.Length; i++)
        {
            // 4개 얻기
            if (tempSprite[i].name.Contains("Blink"))
            {
                BlinkSprite[tempCount] = tempSprite[i];
                tempCount++;
            }

            // 다 얻었으면 나옴
            if (tempCount >= BlinkSprite.Length)
            {
                break;
            }
        }
        for (int i = 0; i < BlinkSprite.Length; i++)
        {
            if (BlinkSprite[i] == null)
            {
                Debug.LogError("BlinkSprite[" + i + "] 못 찾음");
                return;
            }
        }

        // 이미지 바꿀 위치
        BlinkSR = transform.GetComponentsInChildren<SpriteRenderer>();
        if (BlinkSR == null)
        {
            Debug.LogError("BlinkTF 못 찾음");
        }

        // 일정시간 뒤 사라짐
        Destroy(gameObject, DestroyTime);
    }

    void Update()
    {
        CurrentTime += Time.deltaTime;

        // 이미지 교체
        if (CurrentTime >= ChangeTime && ChangeCount < 4)
        {
            CurrentTime -= ChangeTime;

            // 이미지 작은 걸로 교체
            for (int i = 3; i > ChangeCount; i--)
            {
                BlinkSR[i].sprite = BlinkSprite[i - ChangeCount];
            }

            // 마지막 이미지는 끄기
            BlinkSR[ChangeCount].enabled = false;

            ChangeCount++;
        }
    }

    public void SetDestroyTime(float _DestroyTime)
    {
        DestroyTime = _DestroyTime;
        ChangeTime = DestroyTime / 5;
    }
}
