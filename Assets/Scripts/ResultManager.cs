using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoSingleton<ResultManager>
{
    // 플레이어
    Player PlayerScript;
    Gun GunScript;
    MousePoint MousePointScript;
    SpriteRenderer PlayerSprite;
    Animator PlayerAnimator;
    Transform Shadow;
    HeartDestroy HeartDestroyScript;

    // 패배
    Sprite[] LoseSprites; // 스프라이트
    Image LoseImage; // 이미지
    Text LoseText; // 텍스트

    // 승리
    Sprite[] VictorySprites; // 스프라이트
    Image VictoryImage; // 이미지

    // 적
    Enemy EnemyScript;
    BulletSpawner BulletSpawnerScript;

    GasterSpawner GasterSpawnerScript;

    LaserSpawner LaserSpawnerScript;

    private void Awake()
    {
        SetInstance(this);
    }

    void Start()
    {
        // 플레이어 관련
        GameObject temp = GameObject.Find("Player");
        PlayerScript = temp.GetComponent<Player>();
        if (PlayerScript == null)
        {
            Debug.LogError("PlayerScript 못 찾음");
            return;
        }

        GunScript = temp.transform.Find("Gun").GetComponent<Gun>();
        if (GunScript == null)
        {
            Debug.LogError("GunScript 못 찾음");
            return;
        }

        MousePointScript = GameObject.Find("MousePoint").GetComponent<MousePoint>();
        if (MousePointScript == null)
        {
            Debug.LogError("MousePointScript 못 찾음");
            return;
        }

        PlayerSprite = temp.transform.Find("Image").GetComponent<SpriteRenderer>();
        if (PlayerSprite == null)
        {
            Debug.LogError("PlayerSprite 못 찾음");
            return;
        }

        PlayerAnimator = temp.GetComponentInChildren<Animator>();
        if (PlayerAnimator == null)
        {
            Debug.LogError("PlayerAnimator 못 찾음");
            return;
        }

        Shadow = PlayerSprite.transform.GetChild(0);
        if (Shadow == null)
        {
            Debug.LogError("Shadow 못 찾음");
            return;
        }

        HeartDestroyScript = temp.transform.Find("HitPoint").GetComponent<HeartDestroy>();
        if (HeartDestroyScript == null)
        {
            Debug.LogError("HeartDestroyScript 못 찾음");
            return;
        }



        // 스프라이트
        Sprite[] tempSprite = Resources.LoadAll<Sprite>("Sprites/LinkImage");
        LoseSprites = new Sprite[3];
        VictorySprites = new Sprite[5];
        int LoseCount = 0;
        int VictoryCount = 0;
        for (int i = 0; i < tempSprite.Length; i++)
        {
            // 패배
            if (LoseCount < 3)
            {
                if (tempSprite[i].name.Contains("Lose"))
                {
                    LoseSprites[LoseCount] = tempSprite[i];
                    LoseCount++;
                }
            }

            // 승리
            if (VictoryCount < 5)
            {
                if (tempSprite[i].name.Contains("Victory"))
                {
                    VictorySprites[VictoryCount] = tempSprite[i];
                    VictoryCount++;
                }
            }
        }
        for (int i = 0; i < 3; i++)
        {
            if (LoseSprites[i] == null)
            {
                Debug.LogError("LoseSprites[" + i + "] 못 찾음");
                return;
            }
        }
        for (int i = 0; i < 5; i++)
        {
            if (VictorySprites[i] == null)
            {
                Debug.LogError("VictorySprites[" + i + "] 못 찾음");
                return;
            }
        }

        // 패배 이미지
        temp = GameObject.Find("Result");
        LoseImage = temp.transform.Find("Lose").GetComponent<Image>();
        if (LoseImage == null)
        {
            Debug.LogError("LoseImage 못 찾음");
            return;
        }

        // 패배 텍스트
        LoseText = temp.transform.Find("LoseText").GetComponent<Text>();
        if (LoseText == null)
        {
            Debug.LogError("LoseText 못 찾음");
            return;
        }

        // 승리 이미지
        VictoryImage = temp.transform.Find("Victory").GetComponent<Image>();
        if (VictoryImage == null)
        {
            Debug.LogError("VictoryImage 못 찾음");
            return;
        }

        // 적 관련
        EnemyScript = GameObject.Find("Enemy").GetComponent<Enemy>();
        if (EnemyScript == null)
        {
            Debug.LogError("EnemyScript 못 찾음");
            return;
        }

        BulletSpawnerScript = GameObject.Find("E_BulletSpawner").GetComponent<BulletSpawner>();
        if (BulletSpawnerScript == null)
        {
            Debug.LogError("BulletSpawnerScript 못 찾음");
            return;
        }

        GasterSpawnerScript = GameObject.Find("E_GasterSpawner").GetComponent<GasterSpawner>();
        if (BulletSpawnerScript == null) {
            Debug.LogError("GasterSpawnerScript 못 찾음");
            return;
        }


        LaserSpawnerScript = GameObject.Find("E_LaserSpawner").GetComponent<LaserSpawner>();
        if (BulletSpawnerScript == null) {
            Debug.LogError("LaserSpawnerScript 못 찾음");
            return;
        }

    }

    // 게임 끝
    public void GameSet(bool victory)
    {
        //플레이어
        PlayerScript.enabled = false; // 플레이어 멈춤
        GunScript.AllBulletOff(); // 총알 다 멈춤
        GunScript.gameObject.SetActive(false); // 총 사라짐
        MousePointScript.enabled = false; // 마우스 포인터 가만히
        PlayerAnimator.enabled = false; // 애니메이터 끔
        //에너미
        //샌즈
        EnemyScript.enabled = false;
        BulletSpawnerScript.enabled = false;
        //가스터
        GasterSpawnerScript.enabled = false;
        //레이저
        LaserSpawnerScript.enabled = false;
        EnemyScript.AllStop(); // 총알, 애니메이터 다 멈춤

        // 승리
        if (victory)
        {
            StartCoroutine("VictorySpriteChange"); // 승리 이미지 변경 코루틴 실행
        }
        else // 패배
        {
            StartCoroutine("LoseSpriteChange"); // 패배 이미지 변경 코루틴 실행
        }
    }

    // 패배 이미지 변경
    IEnumerator LoseSpriteChange()
    {
        PlayerSprite.sprite = LoseSprites[0];
        HeartDestroyScript.enabled = true;

        for (int i = 1; i < 3; i++)
        {
            yield return new WaitForSeconds(1f);
            PlayerSprite.sprite = LoseSprites[i];
        }
        Shadow.gameObject.SetActive(false);

        // 패배 화면 띄우기
        for (int i = 0; i < 256; i++)
        {
            LoseImage.color = new Color(1, 1, 1, (float)i / 256);
            LoseText.color = new Color(1, 0, 0, (float)i / 256);
            yield return new WaitForSeconds(0.01f);
        }
    }

    // 승리 이미지 변경
    IEnumerator VictorySpriteChange()
    {
        HeartDestroyScript.gameObject.SetActive(false);

        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(0.5f);
            PlayerSprite.sprite = VictorySprites[i];
        }
        //Shadow.gameObject.SetActive(false);

        // 승리 화면 띄우기
        for (int i = 0; i < 256; i++)
        {
            VictoryImage.color = new Color(1, 1, 1, (float)i / 256);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
