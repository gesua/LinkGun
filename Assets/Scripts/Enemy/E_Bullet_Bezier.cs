using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Bullet_Bezier : E_Bullet {
    //플래그가 ture일때는 오른쪽->왼쪽 순서 false일때는 왼쪽->오른쪽 순서
    public bool dirRight = true;
    public float fireDegree;
    //베지어곡선 기본발사위치받을값필요
    public Transform spawnerPos;
    Vector3 p0; //p0은 발사위치 기본값의 초기화가 필요함
    Vector3 p1; 
    Vector3 p2;
    //베지어 이동 좌표 x값
    public float moveXValue = 3f;
    //간격간이동값
    public float moveZValue = 3f;
    //간격간 각도차이
    float diffDegree = 45f;
    float flagCurrTime = 0f;
    //오른쪽 왼쪽 체크 변수
    bool flagRight = false;
    
    private void Start() {
        //총알속도 다시지정
        bulletSpeed = 1f;
        //속도임의로다시받음
        tempSpeed = bulletSpeed;
        //위치초기화
        p0 = spawnerPos.position;
        if (dirRight) {
            NewPosBeziRight();
            flagRight = true;
        } else {
            NewPosBeziLeft();
            flagRight = false;
        }
        this.transform.forward = p2 - this.transform.position;
    }

    // Update is called once per frame
    private void Update () {
        //총알끄기
        //시간제어호출
        Off();
        flagCurrTime += Time.deltaTime*bulletSpeed;
        this.transform.position = BezierMove(p0, p1, p2, flagCurrTime);
        Vector3 dir = p2 - this.transform.position;
        this.transform.forward = Vector3.Lerp(this.transform.forward, dir, Time.deltaTime);
        //시간체크해서 좌우변경
        if (flagCurrTime >= 1f) {
            p0 = p2;
            flagCurrTime = 0;
            if (flagRight) {
                NewPosBeziLeft();
                flagRight = false;
            } else {
                NewPosBeziRight();
                flagRight = true;
            }
                
        }
    }

    ////////////////////////베지어곡선관련함수들(건들필요없음)
    Vector3 BezierMove(Vector3 p0, Vector3 p1, Vector3 p2, float t) {
        float u = 1f - t;
        float t2 = t * t;
        float u2 = u * u;
  

        //러프이동경로
        Vector3 result =
            (u2) * p0 +
            (2f * u * t) * p1 +
            (t2) * p2;
        return result;
    }
    void NewPosBeziRight() {
        p1 = new Vector3(p0.x + moveXValue * (Mathf.Cos((fireDegree + diffDegree) * Mathf.Deg2Rad)), p0.y, p0.z + moveXValue * (Mathf.Sin((fireDegree + diffDegree) * Mathf.Deg2Rad)));
        p2 = new Vector3(p0.x + moveZValue *Mathf.Cos(fireDegree*Mathf.Deg2Rad), p0.y, p0.z + moveZValue * Mathf.Sin(fireDegree * Mathf.Deg2Rad));

    }
    void NewPosBeziLeft() {
        p1 = new Vector3(p0.x + moveXValue * (Mathf.Cos((fireDegree - diffDegree) * Mathf.Deg2Rad)), p0.y, p0.z + moveXValue * (Mathf.Sin((fireDegree - diffDegree) * Mathf.Deg2Rad)));
        p2 = new Vector3(p0.x + moveZValue * Mathf.Cos(fireDegree * Mathf.Deg2Rad), p0.y, p0.z + moveZValue * Mathf.Sin(fireDegree * Mathf.Deg2Rad));
    }
}
