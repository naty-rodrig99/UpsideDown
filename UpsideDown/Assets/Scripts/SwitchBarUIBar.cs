using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using gamespace;

public class SwitchBarUIBar : MonoBehaviour
{
    public GameObject dynamicBar;
    public GameObject backgroundBar;
    public GameObject thresholdLine;


    double switchCharge;
    public double SwitchCost = 40;
    public double maxCharge = 100;
    public double chargeSpeed = 1;
    private double dynamicBar_full_height;

    private WorldType _current_world;

    public bool ready;
    private Renderer _renderer;

    public bool autoCharging;

    void OnEnable()
    {
        WorldController.OnWorldChanged += UpdateWorld;
    }
    void OnDisable()
    {
        WorldController.OnWorldChanged -= UpdateWorld;
    }
    
    void UpdateWorld(WorldType type){
        _current_world = type;
    }

    void Awake(){
        _renderer = dynamicBar.GetComponent<Renderer>();
        _renderer.material.color = Color.blue; 

        dynamicBar_full_height = dynamicBar.transform.localScale.y;
        switchCharge = 0;
        update_switch_ui_bar(0);
    }

    void Start()
    {
        autoCharging = false;
        
    }

    void Update()
    {
        
    }

    public double getCharge(){
        return switchCharge;
    }

    public bool isReady(){
        return getCharge() >= SwitchCost;
    }
    public void useEnergy(){
        update_switch_ui_bar(-SwitchCost);
    }

    void FixedUpdate(){
        if(autoCharging && _current_world == WorldType.BadWorld){
            update_switch_ui_bar(0.3f/10.0f);
        }
    }

    public void update_switch_ui_bar(double value){
        if(switchCharge + value > maxCharge) {
            switchCharge = maxCharge;
        }else{
            switchCharge += value;
        }
        Vector3 scale = dynamicBar.transform.localScale;
        scale.y = (float)dynamicBar_full_height * (float)(switchCharge / maxCharge);
        dynamicBar.transform.localScale = scale;

        if(!ready && switchCharge >= SwitchCost){
            ready = true;
            _renderer.material.color = Color.white; 
        }
        if(ready && switchCharge < SwitchCost){
            ready = false;
            _renderer.material.color = Color.blue; 
        }
    }
}
