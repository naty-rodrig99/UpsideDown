using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using gamespace;


public class PlayerCamera : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    WorldType current_world;
    float cameraZ;
    public GameObject[] Backgrounds;

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
        Debug.Log("playercamer update world");
        current_world = type;
        update_backgrounds();
        UpdateCameraYPos();
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
            delta_x = ((player_position.x - transform.position.x) * Mathf.Abs(player_position.x - transform.position.x)) / (100.0f/cameraSpeed);
        }

        transform.position = new Vector3(transform.position.x + delta_x, transform.position.y, cameraZ);
    }
}
