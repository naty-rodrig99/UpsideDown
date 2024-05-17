using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System;
using gamespace;


public class PlayerCamera : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    WorldType current_world;
    float cameraZ;
    public GameObject[] Backgrounds;

    public GameObject[] goodLightObject;
    private Light2D[] LightGoodWorld;

    public GameObject[] FlashlightsObj;
    private Light2D[] Flashlights;

    public float cameraSpeed;

    public struct CAM_SETTINGS
    {
        public const float good_world_cam_y = -7.35f;
        public const float bad_world_cam_y = -7.35f - 20f;
    }

    void OnEnable()
    {
        WorldController.OnWorldChanged += UpdateWorld;
    }
    void OnDisable()
    {
        WorldController.OnWorldChanged -= UpdateWorld;
    }
    
    void UpdateWorld(WorldType type){
        current_world = type;
        update_backgrounds();
        UpdateCameraYPos();
        UpdateLight();
    }
    void UpdateLight()
    {
        if (current_world == WorldType.GoodWorld)
        {
            foreach (Light2D light in LightGoodWorld)
            {
                light.enabled = true;
            }
            foreach (Light2D light in Flashlights)
            {
                light.enabled = false;
            }
        }
        else if (current_world == WorldType.BadWorld)
        {
            foreach (Light2D light in LightGoodWorld)
            {
                light.enabled = false;
            }
            foreach (Light2D light in Flashlights)
            {
                light.enabled = true;
            }
        }
    }


    void UpdateCameraYPos(){
        if(current_world == WorldType.GoodWorld){
            transform.position = new Vector3(transform.position.x, CAM_SETTINGS.good_world_cam_y, cameraZ);
        }
        if(current_world == WorldType.BadWorld){
            transform.position = new Vector3(transform.position.x, CAM_SETTINGS.bad_world_cam_y, cameraZ);
        }
    }

    void Awake(){
    }

    
    void Start()
    {
        LightGoodWorld = new Light2D[goodLightObject.Length];
        for (int i = 0; i < goodLightObject.Length; i++)
        {
            LightGoodWorld[i] = goodLightObject[i].GetComponent<Light2D>();
        }
        Flashlights = new Light2D[FlashlightsObj.Length];
        for (int i = 0; i < FlashlightsObj.Length; i++)
        {
            Flashlights[i] = FlashlightsObj[i].GetComponent<Light2D>();
        }
        cameraZ = transform.position.z;
        transform.position = new Vector3(player.transform.position.x, CAM_SETTINGS.good_world_cam_y, cameraZ);

    }
    public void update_backgrounds()
    {
        if (current_world == WorldType.GoodWorld)
        {
            Backgrounds[0].SetActive(true);
            Backgrounds[1].SetActive(false);
        }
        else
        {
            Backgrounds[0].SetActive(false);
            Backgrounds[1].SetActive(true);
        }

    }

    void FixedUpdate()
    {
        handleCameraMovement();
    }

    void handleCameraMovement(){
        Vector2 player_position = player.transform.position;
        float delta_x;

        if(Mathf.Abs(player_position.x - transform.position.x) < 0.3f){
            delta_x = 0;
        }
        else{
            delta_x = Math.Max(-40.0f, Mathf.Min(40.0f,((player_position.x - transform.position.x) * Mathf.Abs(player_position.x - transform.position.x)) / (100.0f/cameraSpeed)));
        }

        transform.position = new Vector3(transform.position.x + delta_x, transform.position.y, cameraZ);
    }
}
