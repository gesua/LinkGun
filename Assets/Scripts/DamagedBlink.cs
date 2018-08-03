using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedBlink : MonoBehaviour
{
    bool IsBlinkStart = false;

    public float DamageTime = 0.2f; // 깜빡이는 시간
    public int BlinkNumber = 4; // 깜빡이는 횟수
    int BlinkCount = 0; // 횟수 세는거
    float CurrentTime = 0f; // 경과 시간

    SpriteRenderer sprite; // 깜빡거릴 이미지

    private void Awake()
    {
        sprite = transform.GetComponentInChildren<SpriteRenderer>();
        if (sprite == null)
        {
            Debug.LogError("sprite 못 찾음");
            return;
        }
    }

    private void Update()
    {
        if (IsBlinkStart == true)
        {
            CurrentTime += Time.deltaTime;
            float temp = CurrentTime / DamageTime;

            // 빨간색으로 깜빡임
            sprite.color = new Color(1, temp, temp);

            // 깜빡였는지 확인
            if (CurrentTime >= DamageTime)
            {
                CurrentTime = 0;

                // 깜빡이는 횟수 확인
                BlinkCount++;
                if (BlinkCount >= BlinkNumber)
                {
                    // 끝났으면 멈춤
                    IsBlinkStart = false;
                }
            }
        }
    }

    // 깜빡임 시작
    public void BlinkStart()
    {
        IsBlinkStart = true;
        CurrentTime = 0;
        BlinkCount = 0;
    }
}
