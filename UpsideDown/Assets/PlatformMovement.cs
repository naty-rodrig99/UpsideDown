using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static player;

public class PlatformMovement : MonoBehaviour
{
    public float speed =1.9f;
    private Vector3 pointA;
    private Vector3 pointB;

    // Start is called before the first frame update
    void Start()
    {
        pointA = transform.position;
        /*if(current_world == "good") {
            pointB = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);
        } 
        else
        {
            pointB = new Vector3(transform.position.x + 8, transform.position.y, transform.position.z);
        }*/
        pointB = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        float time = Mathf.PingPong(Time.time * speed, 1);
        transform.position = Vector3.Lerp(pointA, pointB, time);    
    }
       /* void OnEnable()
        {
            WorldController.OnWorldChanged += UpdateWorld;
        }
        void OnDisable()
        {
            WorldController.OnWorldChanged -= UpdateWorld;
        }

        void UpdateWorld(WorldType type)
        {
            current_world = type;
            update_backgrounds();
            UpdateCameraYPos();
        } */
}
