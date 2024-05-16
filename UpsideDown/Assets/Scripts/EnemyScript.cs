using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using gamespace;

public class Enemies : MonoBehaviour
{
    public Animator monkeyAnimator;
    public GameObject main_player;
    Rigidbody2D rb;
    BoxCollider2D boxCollider;
    string mode;
    int direction;
    public float speed;
    public int init_health = 3;
    private int _health;
    private Vector3 initial_pos;
    WorldType current_world;

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
        switchWorld(type);
    }

    public void hitByBullet(){
        _health -= 1;
        if(_health <= 0) die();
    }
    void die(){
        Destroy(gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        _health = init_health;
        current_world = WorldType.GoodWorld;
        initial_pos = transform.position;
        //monkeyAnimator.SetBool("goodWorld", false);
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        boxCollider = GetComponent<BoxCollider2D>();
        mode = "goodWorld";
        direction = 1;

    }

        // Update is called once per frame
        void FixedUpdate()
        {
        switch (mode)
        {
            case "goodWorld":
                monkeyAnimator.SetBool("goodWorld", true);
                monkeyAnimator.SetBool("monkeyMoves", true);
                break;
            
            case "badWorld":
                monkeyAnimator.SetBool("goodWorld", false);
                monkeyAnimator.SetBool("monkeyWalks", true);
                int layerMask = ~(LayerMask.GetMask("PlayerLayer"));
                rb.velocity = new Vector2((float)(direction * speed), rb.velocity.y);
                RaycastHit2D hit = Physics2D.Raycast(boxCollider.bounds.center, new Vector2(direction,-1),Mathf.Sqrt(Mathf.Pow(boxCollider.bounds.extents.y , 2) + Mathf.Pow(boxCollider.bounds.extents.x, 2)) + 1.0f, layerMask);
                
                if(hit.collider == null)
                {
                    direction = direction * -1;
                    this.transform.Rotate(new Vector3(0, 180, 0));
                    Debug.Log(direction);
                }
                if(Mathf.Abs(main_player.transform.position.x - this.transform.position.x) < 4)
                {
                    mode = "attack";
                }
                break;

            case "attack":
                monkeyAnimator.SetBool("goodWorld", false);
                monkeyAnimator.SetBool("monkeyWalks", true);
                int attackSpeed = 3;
                rb.velocity = new Vector2((float)(direction * attackSpeed), rb.velocity.y);
                if (Mathf.Abs(main_player.transform.position.x - this.transform.position.x) >= 4)
                {
                    mode = "badWorld";
                }
                else if (Mathf.Abs(main_player.transform.position.x - this.transform.position.x) < 1)
                {
                    direction = 0;
                }
                else if (main_player.transform.position.x < this.transform.position.x)
                {
                    direction = -1;
                    //this.transform.Rotate(new Vector3(0, 180, 0));
                }
                else if(main_player.transform.position.x > this.transform.position.x)
                {
                    direction = 1;
                }
                break;

            case "getsShot":
                monkeyAnimator.SetBool("monkeyGetsShot", true);
                break;
        }
            
    }

    public void switchWorld(WorldType world)
    {
        if (world == WorldType.GoodWorld)
        {
            this.transform.position += new Vector3(0, 20, 0);
            mode = "goodWorld";
        }
        else if (world == WorldType.BadWorld)
        {
            this.transform.position += new Vector3(0, -20, 0);
            mode = "badWorld";
        }
    }
}
