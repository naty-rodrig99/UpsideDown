using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using gamespace;

public class BoxScript : MonoBehaviour
{
    WorldType current_world;
    private Rigidbody2D _rb;


    void OnEnable()
    {
        WorldController.OnWorldChanged += UpdateWorld;
    }
    void OnDisable()
    {
        WorldController.OnWorldChanged -= UpdateWorld;
    }
    void UpdateWorld(WorldType type){
        current_world = type;
        change_world(type);
    }

    void change_world(WorldType world){       
        switch (world){
            case WorldType.BadWorld:
                transform.position += new Vector3(0, -20, 0);
                _rb.mass = 62.0f;
                break;
            case WorldType.GoodWorld:
                transform.position += new Vector3(0, 20, 0);
                _rb.mass = 1000.0f;
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.mass = 1000.0f;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
