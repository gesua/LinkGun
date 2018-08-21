using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawnPointCheck : MonoBehaviour {
    public bool checkPlayer = false;
    private void OnTriggerEnter(Collider other) {
        if(other.tag.Equals("Player")) {
            checkPlayer = true;
        }
    }
    private void OnTriggerExit(Collider other) {
        Debug.Log(other.name);
        if (other.tag.Equals("Player")) {
            checkPlayer = false;
        }
    }
}
