using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hill_movement : MonoBehaviour
{
    public GameObject player;
    public GameObject camera;

    public float speed;

    Vector3 position;
    // Start is called before the first frame update
    void Start()
    {
        position = new Vector3(0, transform.position.y, transform.position.z);

    }

    // Update is called once per frame
    void Update()
    {
        float player_x = player.transform.position.x;

        position.x = player_x * speed;
        transform.position = position;

    }
}
