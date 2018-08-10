using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierTest: E_Bullet {
    //베지어곡선 계산시간
    public Transform pStart;
    public Transform p1;
    public Transform p2;
    public Transform pEnd;
    float currTime = 0f;
    
    private void Start() {
        //p1 = pStart.position + new Vector3(2, -10, 0);
        //p2 = pStart.position + new Vector3(-10, -10, 0);
    }

    // Update is called once per frame
    void Update () {
        currTime += Time.deltaTime/5;
        this.transform.position = BezierMove(pStart.position,p1.position,p2.position,pEnd.position,currTime);


    }

    Vector3 BezierMove(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
        float u = 1f - t;
        float t2 = t * t;
        float u2 = u * u;
        float u3 = u2 * u;
        float t3 = t2 * t;

        Vector3 result =
            (u3) * p0 +
            (3f * u2 * t) * p1 +
            (3f * u * t2) * p2 +
            (t3) * p3;
        
        return result;
    }
}
