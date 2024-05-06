using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemies : MonoBehaviour
{
    public Animator monkeyAnimator;
    public GameObject main_player;
    Rigidbody2D rb;
    BoxCollider2D boxCollider;
    string mode;
    int direction;
    public float speed;
    private Vector3 initial_pos;

    // Start is called before the first frame update
    void Start()
    {
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

    public void switchWorld(string world)
    {
        if (world == "good")
        {
            this.transform.position += new Vector3(0, 20, 0);
            mode = "goodWorld";
        }
        else if (world == "bad")
        {
            this.transform.position += new Vector3(0, -20, 0);
            mode = "badWorld";
        }
    }
}
