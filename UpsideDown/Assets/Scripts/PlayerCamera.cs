using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System;
using gamespace;
using TarodevController;


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

    private int _looking_direction;
    public float front_look_amount = 0.0f;

    public float cameraSpeedX = 4.0f;
    public float cameraSpeedY = 3.0f;
    private Vector2 _initPos;
    private Vector2 _cameraCurrentMinHeight;
    public float cameraHeightMax;
    public float offset;

    private float _playerVelocityX;

    private float _good_world_cam_y = 0.0f;
    private float _bad_world_cam_y = -SETTINGS.worlds_offset;
    

    void OnEnable()
    {
        WorldController.OnWorldChanged += UpdateWorld;
        WorldController.OnPlayerDirectionChanged += UpdateDir;
    }
    void OnDisable()
    {
        WorldController.OnWorldChanged -= UpdateWorld;
        WorldController.OnPlayerDirectionChanged += UpdateDir;
    }
    
    void UpdateWorld(WorldType type){
        current_world = type;
        update_backgrounds();
        UpdateCameraYPos();
        UpdateLight();
    }
    void UpdateDir(int dir){
        _looking_direction = dir;
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
        _initPos = transform.position;
        transform.position = new Vector3(player.transform.position.x, _good_world_cam_y, cameraZ);

        _good_world_cam_y = _initPos.y;
        _bad_world_cam_y = _initPos.y + _bad_world_cam_y;

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
            transform.position = new Vector3(transform.position.x, _good_world_cam_y, cameraZ);
            _cameraCurrentMinHeight = new Vector2(0.0f, _good_world_cam_y);
        }
        if(current_world == WorldType.BadWorld){
            transform.position = new Vector3(transform.position.x, _bad_world_cam_y, cameraZ);
            _cameraCurrentMinHeight = new Vector2(0.0f, _bad_world_cam_y);
        }
    }
    PlayerController playerMovementController;
    void Awake(){
        playerMovementController = player.GetComponent<PlayerController>();
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
        _playerVelocityX = playerMovementController.getPlayerVelocity().x;
        handleCameraMovement();
    }

    void handleCameraMovement(){
        float delta_x = getDeltaX();
        float delta_y = getDeltaY();
        float maxY = _cameraCurrentMinHeight.y;
        float minY = _cameraCurrentMinHeight.y - cameraHeightMax;
        transform.position = new Vector3(transform.position.x + delta_x, Mathf.Max(maxY, Mathf.Min(minY, transform.position.y + delta_y )), cameraZ);
    }
    float getDeltaY(){
        float playerPositionY = player.transform.position.y;
        
        float goalPosY = playerPositionY + offset;
        float deltaY = ((goalPosY - transform.position.y) * Mathf.Abs(goalPosY - transform.position.y)) / (100.0f/cameraSpeedY);
        float threshold = 0.005f;

        return deltaY;
    }
    float getDeltaX(){
        float delta_x;
        Vector3 front_offset = new Vector3(front_look_amount * _playerVelocityX, 0.0f, 0.0f);
        Vector2 player_position = player.transform.position + front_offset;

        if(Mathf.Abs(player_position.x - transform.position.x) < 0.3f){
            delta_x = 0;
        }
        else{
            delta_x = Math.Max(-40.0f, Mathf.Min(40.0f,((player_position.x - transform.position.x) * Mathf.Abs(player_position.x - transform.position.x)) / (100.0f/cameraSpeedX)));
        }
        return delta_x;
    }
}
