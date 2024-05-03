using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBarUIBar : MonoBehaviour
{
    public GameObject dynamicBar;
    public GameObject backgroundBar;
    public GameObject thresholdLine;

    public GameObject Player;

    double switchCharge;
    public double SwitchCost = 40;
    public double chargeSpeed = 1;
    private double dynamicBar_full_height;

    public bool ready;
    private Renderer renderer;


    void Start()
    {
        player playerScript = Player.GetComponent<player>();

        renderer = dynamicBar.GetComponent<Renderer>();
        renderer.material.color = Color.blue; 

        dynamicBar_full_height = dynamicBar.transform.localScale.y;
        switchCharge = 20;
        
    }

    void Update()
    {
        
    }

    public double getCharge(){
        return switchCharge;
    }

    void FixedUpdate(){
        if(switchCharge < 100){
            update_switch_ui_bar(chargeSpeed * 0.01);
        }
    }

    public void update_switch_ui_bar(double value){
        switchCharge += value;
        Vector3 scale = dynamicBar.transform.localScale;
        scale.y = (float)dynamicBar_full_height * (float)(switchCharge / 100.0f);
        dynamicBar.transform.localScale = scale;

        if(!ready && switchCharge >= SwitchCost){
            ready = true;
            renderer.material.color = Color.white; 
        }
        if(ready && switchCharge < SwitchCost){
            ready = false;
            renderer.material.color = Color.blue; 
        }
    }
}
