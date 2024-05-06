using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiger : MonoBehaviour
{
    public Animator tigerAnimator;
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
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        boxCollider = GetComponent<BoxCollider2D>();
        mode = "goodWorld";
        direction = 1;
    }


    void FixedUpdate()
    {
        switch (mode)
        {
            case "goodWorld":
                tigerAnimator.SetBool("goodWorld", true);
                tigerAnimator.SetBool("tigerWalks", true);
                int layerMask1 = ~(LayerMask.GetMask("PlayerLayer"));
                rb.velocity = new Vector2((float)(direction * speed), rb.velocity.y);
                RaycastHit2D hit1 = Physics2D.Raycast(boxCollider.bounds.center, new Vector2(direction, -1), Mathf.Sqrt(Mathf.Pow(boxCollider.bounds.extents.y, 2) + Mathf.Pow(boxCollider.bounds.extents.x, 2)) + 1.0f, layerMask1);

                if (hit1.collider == null)
                {
                    direction = direction * -1;
                    this.transform.Rotate(new Vector3(0, 180, 0));
                    Debug.Log(direction);
                } 
                break;

            case "badWorld":
                tigerAnimator.SetBool("goodWorld", false);
                tigerAnimator.SetBool("tigerWalks", true);
                int layerMask = ~(LayerMask.GetMask("PlayerLayer"));
                rb.velocity = new Vector2((float)(direction * speed), rb.velocity.y);
                RaycastHit2D hit = Physics2D.Raycast(boxCollider.bounds.center, new Vector2(direction, -1), Mathf.Sqrt(Mathf.Pow(boxCollider.bounds.extents.y, 2) + Mathf.Pow(boxCollider.bounds.extents.x, 2)) + 1.0f, layerMask);

                if (hit.collider == null)
                {
                    direction = direction * -1;
                    this.transform.Rotate(new Vector3(0, 180, 0));
                    Debug.Log(direction);
                }
                if (Mathf.Abs(main_player.transform.position.x - this.transform.position.x) < 4)
                {
                    mode = "attack";
                }
                break;

            case "attack":
                tigerAnimator.SetBool("goodWorld", false);
                tigerAnimator.SetBool("tigerWalks", true);
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
                else if (main_player.transform.position.x > this.transform.position.x)
                {
                    direction = 1;
                }
                break;

            case "getsShot":
                tigerAnimator.SetBool("tigerGetsShot", true);
                break;
        }

    }
}
