using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Laser : E_Bullet {
    LaserSpawner laserSpawner;

    new private void OnEnable() {
        bulletSpeed = 0f;
    }
    public void SetSpawner(LaserSpawner spawner) {
        laserSpawner = spawner;
    }
   
    private void Update() {
        
    }


    new private void OnTriggerEnter(Collider other) {
        if (other.tag.Equals("Player")) {
          
        }
    }
}
