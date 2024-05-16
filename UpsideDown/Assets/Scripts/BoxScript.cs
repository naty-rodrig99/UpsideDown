using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using gamespace;

public class BoxScript : MonoBehaviour
{
    WorldType current_world;
    private Rigidbody2D rb;

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
    }

    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {

    }
}
