using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public int maxHearts = 5;
    private int currentHearts = 3;

    public Image[] heartIcons;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    private player_script playerScript;

    // Start is called before the first frame update
    void Start()
    {
        UpdateHeartIcons();
        GameObject player = GameObject.Find("Main_Character");
        playerScript = player.GetComponent<player_script>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdateHeartIcons()
    {
        for (int i = 0; i < heartIcons.Length; i++)
        {

            if (i < currentHearts)
            {
                heartIcons[i].sprite = fullHeart;
            }
            else
            {
                heartIcons[i].sprite = emptyHeart;
            }

            if (i < maxHearts)
            {
                heartIcons[i].enabled = true;
            }
            else
            {
                heartIcons[i].enabled = false;
            }
        }
    }

    public void ModifyHealth(int amount)
    {
        currentHearts = amount;
        currentHearts = Mathf.Clamp(currentHearts, 0, maxHearts);
        UpdateHeartIcons();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Increase player's heart count
            //SoundManager soundManager = playerController.GetComponent<SoundManager>();
            
            playerScript.Health += 1;
            //Destroy the collectible bullet GameObject
            //soundManager.PlayCollectPointSound();
            Destroy(gameObject);
        }
    }

}
