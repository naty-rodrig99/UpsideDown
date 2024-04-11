using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloud_movement : MonoBehaviour
{
    public GameObject player;

    private float speed;
    public float parallax_speed;

    public Vector2 speed_range;

    Vector3 position;
    Vector3 initial_pos;
    // Start is called before the first frame update
    void Start()
    {
        position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        initial_pos = position;
        speed = Random.Range(speed_range.x, speed_range.y);
    }

    // Update is called once per frame
    void Update()
    {
        float player_x = player.transform.position.x;

        position.x = initial_pos.x + player_x * parallax_speed + (speed / 1000) * Time.frameCount;
        transform.position = position;

    }
}
