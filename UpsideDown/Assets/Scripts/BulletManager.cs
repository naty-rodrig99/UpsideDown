using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollect : MonoBehaviour
{
    public int bulletCount = 10;
    private Vector3 startPosition;
    private float noiseOffsetX;
    private float noiseOffsetY;
    public float moveRange = 0.5f; // The range of movement from the start position
    public float noiseScale = 1.0f; // Scale of the noise
    public float moveSpeed = 1.0f; // Speed of movement

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        noiseOffsetX = Random.Range(0f, 100f);
        noiseOffsetY = Random.Range(0f, 100f);
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate new position using Perlin noise
        float newX = startPosition.x + (Mathf.PerlinNoise(Time.time * moveSpeed + noiseOffsetX, 0) - 0.5f) * moveRange * 2;
        float newY = startPosition.y + (Mathf.PerlinNoise(0, Time.time * moveSpeed + noiseOffsetY) - 0.5f) * moveRange * 2;

        transform.position = new Vector3(newX, newY, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Increase player's bullet count
            // player_script playerController = other.GetComponent<player_script>();
            // SoundManager soundManager = playerController.GetComponent<SoundManager>();  
            // if (playerController != null)
            // {
            //     playerController.ManageBullets(bulletCount);
            // }

            // // Destroy the collectible bullet GameObject
            // soundManager.PlayCollectPointSound();
            Destroy(gameObject);
        }
    }
}
