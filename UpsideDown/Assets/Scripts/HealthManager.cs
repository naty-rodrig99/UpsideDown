using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Image[] heartIcons;
    public int maxHearts = 3;
    private int currentHearts;

    // Start is called before the first frame update
    void Start()
    {
        currentHearts = maxHearts;
        UpdateHeartIcons();
    }

    void UpdateHeartIcons()
    {
        for (int i = 0; i < heartIcons.Length; i++)
        {
            if (i < currentHearts)
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
        currentHearts += amount;
        currentHearts = Mathf.Clamp(currentHearts, 0, maxHearts);
        UpdateHeartIcons();
    }

    public void CollectHeart()
    {
        if (currentHearts < maxHearts)
        {
            currentHearts++;
            UpdateHeartIcons();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
