using gamespace;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.Experimental.GraphView.GraphView;
//using static player;

public class PlatformMovement : MonoBehaviour
{
    public float speed =1.9f;
    private Vector3 pointA;
    private Vector3 pointB;
    WorldType _current_world;

    void UpdateWorld(WorldType type)
    {
        _current_world = type;
        if (_current_world == WorldType.GoodWorld)
        {
            pointB = new Vector3(pointA.x, pointA.y + 5, pointA.z);
        }
        else
        {
            pointB = new Vector3(pointA.x + 10, pointA.y, pointA.z);
        }
    }

    private void OnEnable()
    {
        WorldController.OnWorldChanged += UpdateWorld;
    }

    private void OnDisable()
    {
        WorldController.OnWorldChanged -= UpdateWorld;
    }

    void Start()
    {
        pointA = transform.position;
    }

    void Update()
    {
        float time = Mathf.PingPong(Time.time * speed, 1);
        transform.position = Vector3.Lerp(pointA, pointB, time);    
    }

}
