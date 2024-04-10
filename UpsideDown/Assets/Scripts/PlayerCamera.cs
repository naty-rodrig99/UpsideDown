using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 player_position = player.transform.position;
        float cameraZ = transform.position.z;
        transform.position = new Vector3(player_position.x, player_position.y, cameraZ);
    }
}
