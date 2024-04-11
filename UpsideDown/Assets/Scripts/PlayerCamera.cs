using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    private string current_world;
    float cameraZ;
    public GameObject[] Backgrounds;

    public struct CAM_SETTINGS
    {
        public const float good_world_cam_y = -7.35f;
        public const float bad_world_cam_y = -7.35f - 20f;
    }
    void Start()
    {
        cameraZ = transform.position.z;
        update_current_world("good");
    }
    public void update_current_world(string value)
    {
        current_world = value;


        if (current_world == "good")
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
    void Update()
    {
        Vector2 player_position = player.transform.position;
        if (current_world == "good")
        {
            transform.position = new Vector3(player_position.x, CAM_SETTINGS.good_world_cam_y, cameraZ);
        }
        if (current_world == "bad")
        {
            transform.position = new Vector3(player_position.x, CAM_SETTINGS.bad_world_cam_y, cameraZ);
        }
    }
}
