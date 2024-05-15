using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemies : MonoBehaviour
{
    public Animator monkeyAnimator;
    public GameObject player;
    Rigidbody2D rb;
    BoxCollider2D boxCollider;
    string mode;
    int direction;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        monkeyAnimator.SetBool("goodWorld", false);
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        boxCollider = GetComponent<BoxCollider2D>();
        mode = "attack";
        direction = 1;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (mode)
        {
            case "goodWorld":
                
                break;
            
            case "badWorld":
                int layerMask = ~(LayerMask.GetMask("PlayerLayer"));
                rb.velocity = new Vector2((float)(direction * speed), rb.velocity.y);
                RaycastHit2D hit = Physics2D.Raycast(boxCollider.bounds.center, new Vector2(direction,-1),Mathf.Sqrt(Mathf.Pow(boxCollider.bounds.extents.y , 2) + Mathf.Pow(boxCollider.bounds.extents.x, 2)) + 1.0f, layerMask);
                
                if(hit.collider == null)
                {
                    direction = direction * -1;
                }
                if(Mathf.Abs(player.transform.position.x - this.transform.position.x) < 4)
                {
                    mode = "attack";
                }
                break;

            case "attack":
                int attackSpeed = 3;
                rb.velocity = new Vector2((float)(direction * attackSpeed), rb.velocity.y);
                if (Mathf.Abs(player.transform.position.x - this.transform.position.x) >= 4)
                {
                    mode = "badWorld";
                }
                else if (Mathf.Abs(player.transform.position.x - this.transform.position.x) < 1)
                {
                    direction = 0;
                }
                else if (player.transform.position.x < this.transform.position.x)
                {
                    direction = -1;
                }
                else if(player.transform.position.x > this.transform.position.x)
                {
                    direction = 1;
                }
                break;
        }
            
    }
}
