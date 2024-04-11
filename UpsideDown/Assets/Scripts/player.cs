using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class player : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 movement = new Vector2(0, 0);
    public float speed;
    public float speedJump;
    bool onGround;
    string current_world;

    public GameObject Camera;
    PlayerCamera camera_controller;

    public GameObject bullet;

    private int looking_direction;

    public enum WorldType
    {
        GoodWorld,
        BadWorld
    }
    public WorldType start_world;
    private Vector3 initial_pos;
    BoxCollider2D boxCollider2d;

    public SoundManager soundManager;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider2d = GetComponent<BoxCollider2D>();

        initial_pos = transform.position;

        looking_direction = 1;
        rb = GetComponent<Rigidbody2D>();
        camera_controller = Camera.GetComponent<PlayerCamera>();
        rb.freezeRotation = true;


        if (start_world == WorldType.GoodWorld)
        {
            current_world = "good";
        }
        else
        {
            current_world = "bad";
        }
        camera_controller.update_current_world(current_world);
    }

    void Update()
    {
        if ((current_world == "good" && transform.position.y < -14) || current_world == "bad" && transform.position.y < -33)
        {
            transform.position = initial_pos;
            rb.velocity = new Vector3(0, 0, 0);
            return;
        }

        float horizontalMovement = Input.GetAxis("Horizontal") * speed;

        if (horizontalMovement > 0)
        {
            looking_direction = 1; // Right
        }
        else if (horizontalMovement < 0)
        {
            looking_direction = -1; // Left
        }
        rb.velocity = new Vector2(horizontalMovement, rb.velocity.y);

        if (Input.GetButtonDown("Jump"))
        {
            jump();
        }

        if (Input.GetButtonDown("Fire2")) // change from good to bad
        {
            change_world();
        }
        if (Input.GetButtonDown("Fire1") && current_world == "bad")
        {
            fire_bullet();
        }

    }
    void jump()
    {
        if (isGrounded())
        {
            rb.AddForce(new Vector2(0, speedJump), ForceMode2D.Impulse);
            soundManager.PlayJumpSound();
        }
    }
    void fire_bullet()
    {
        GameObject bullet_instance = Instantiate(bullet, transform.position, Quaternion.identity);
        Bullet bullet_script = bullet_instance.GetComponent<Bullet>();
        bullet_script.set_direction(looking_direction); // sets direction of bullet, 1 right -1 left
    }

    void change_world()
    {
        switch (current_world)
        {
            case "good":
                current_world = "bad";
                transform.position += new Vector3(0, -20, 0);
                break;
            case "bad":
                current_world = "good";
                transform.position += new Vector3(0, 20, 0);
                break;
        }
        camera_controller.update_current_world(current_world);
    }
    private bool isGrounded()
    {
        int layerMask = ~(LayerMask.GetMask("PlayerLayer"));

        RaycastHit2D hit = Physics2D.Raycast(boxCollider2d.bounds.center, Vector2.down, boxCollider2d.bounds.extents.y + 0.1f, layerMask);
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.tag);
            if (hit.collider.tag == "platform")
            {
                return true;
            }
        }
        //Debug.DrawRay(boxCollider2d.bounds.center, Vector2.down * (boxCollider2d.bounds.extents.y + 0.1f));

        return false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "platform" || other.gameObject.tag == "box")
        {
        }

    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "platform")
        {
        }

    }
}
