using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollect : MonoBehaviour
{
    public int bulletCount = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Increase player's bullet count
            player playerController = other.GetComponent<player>();
            SoundManager soundManager = playerController.GetComponent<SoundManager>();  
            if (playerController != null)
            {
                playerController.AddBullets(bulletCount);

                // Update UI element with new bullet count
                playerController.UpdateBulletUI();
            }

            // Destroy the collectible bullet GameObject
            soundManager.PlayCollectPointSound();
            Destroy(gameObject);
        }
    }
}
