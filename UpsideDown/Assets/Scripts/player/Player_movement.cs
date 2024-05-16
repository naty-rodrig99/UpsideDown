using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player_movement : MonoBehaviour
{
    public float max_speed;
    public float acceleration;
    public float deceleration; 
    private float speed_dim = 0.1f;

    private Vector2 input;
    private Vector3 frame_movement;
    private Vector3 velocity;

    private Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        input = new Vector2(0.0f,0.0f);
        frame_movement = new Vector3(0.0f,0.0f,0.0f);
        velocity = new Vector3(0.0f,0.0f,0.0f);

    }

    void FixedUpdate()
    {
        get_horizontal_movement();
        apply_movement();
    }

    void get_horizontal_movement(){
        input.x = Input.GetAxisRaw("Horizontal");
        
        if(input.x != 0.0f){ // acceleration
            velocity.x = Mathf.Max(-max_speed*speed_dim, Mathf.Min(max_speed*speed_dim, velocity.x + acceleration*input.x*speed_dim)) ;
        }else{ // deceleration
            
            if(velocity.x > 0.0f){
                velocity.x = Mathf.Max(0.0f, velocity.x - deceleration*speed_dim) ;
            }
            if(velocity.x < 0.0f){
                velocity.x = Mathf.Min(0.0f, velocity.x + deceleration*speed_dim)  ;
            }  
        }
    }

    void apply_movement(){
        _rb.velocity = velocity;
    }
}
