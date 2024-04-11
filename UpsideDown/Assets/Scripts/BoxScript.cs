using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour
{
    string current_world;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void update_current_world(string state){
        current_world = state;
        Debug.Log(current_world);
        if(current_world == "good"){
            rb.mass = 1000;
        }
        if(current_world == "bad"){
            rb.mass = 15;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
