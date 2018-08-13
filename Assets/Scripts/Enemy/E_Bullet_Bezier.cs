using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Bullet_Bezier : MonoBehaviour {
    
    //베지어곡선 기본발사위치받을값필요
    Vector3 p0;
    Vector3 p1;
    Vector3 p2;
    //베지어 이동 좌표 x값
    public float moveXValue = 5f;
    public float moveZValue = 5f;
    //Vector3 p3;
    float currTime = 0f;
    //오른쪽 왼쪽 체크 변수
    bool flagRight = false;
    
    private void Start() {
        p0 = new Vector3(0, 0, 0);
        NewPosBeziRight();
        flagRight = true;
    }

    // Update is called once per frame
    void Update () {
        currTime += Time.deltaTime;
        this.transform.position = BezierMove(p0,p1,p2,currTime);
        //시간체크해서 좌우변경
        if(currTime >= 1f) {
            p0 = p2;
            currTime = 0;
            if (flagRight) {
                NewPosBeziLeft();
                flagRight = false;
            } else {
                NewPosBeziRight();
                flagRight = true;
            }
                
        }
    }

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
        p1 = new Vector3(p0.x + moveXValue, p0.y, p0.z - moveZValue);
        p2 = new Vector3(p0.x , p0.y, p0.z - moveZValue*2);
    }
    void NewPosBeziLeft() {
        p1 = new Vector3(p0.x - moveXValue, p0.y, p0.z - moveZValue);
        p2 = new Vector3(p0.x, p0.y, p0.z - moveZValue * 2);
    }
}
