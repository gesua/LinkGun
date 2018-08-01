using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//보스의 이동경로지정
//기본으로 키입력을 받아서 이동하기


public class Enemy : MonoBehaviour {
    public float moveSpeed = 1f;
  
	// Use this for initialization
	void Start () {

		
	}
	
	// Update is called once per frame
	void Update () {
        //보스의이동
        this.transform.position += new Vector3(Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime,0 , Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime);
        

	}
}
