using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBoids : MonoBehaviour
{
    [SerializeField] private GameObject boidPrefab;
    [SerializeField] private float spawnRadius = 10;
    [SerializeField] private float numBoids = 50;

    // Start is called before the first frame update
    void Start()
    {
        // Spawn x number of boids within a radius with some random direction
        for (int i = 0; i < numBoids; i++) {
            GameObject boid = Instantiate(boidPrefab);
            boid.transform.position = Random.insideUnitSphere * spawnRadius;
            boid.transform.rotation = Random.rotation;
            if (i == 0) BirdCam.target = boid.transform;
        }
    }

}
