using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Laser : E_Bullet {
    LaserSpawner laserSpawner;

    public void SetSpawner(LaserSpawner spawner) {
        laserSpawner = spawner;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag.Equals("Player")) {
            Debug.Log("플레이어에닿음");
        }
    }
}
